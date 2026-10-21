namespace NorthWind.Sales.Backend.UseCases.CreateOrder;

internal class CreateOrderInteractor(
    ICreateOrderOutputPort outputPort,
    ICommandsRepository repository,
    IModelValidatorHub<CreateOrderDto> modelValidatorHub,
    IDomainEventHub<SpecialOrderCreatedEvent> domainEventHub,
    IDomainLogger domainLogger,
    IDomainTransaction domainTransaction,
    IUserService userService) :
    ICreateOrderInputPort
{
    public async Task Handle(CreateOrderDto orderDto)
    {
        GuardUser.AgainstUnauthenticated(userService);

        await GuardModel.AgainstNotValid(modelValidatorHub, orderDto);

        await domainLogger.LogInformation(new DomainLog(
            CreateOrderMessages.StartingPurchaseOrderCreation,
            userService.UserName));

        OrderAggregate Order = OrderAggregate.From(orderDto);

        try
        {
            // Iniciar la transacción
            domainTransaction.BeginTransaction();

            await repository.CreateOrder(Order);
            await repository.SaveChanges();

            await domainLogger.LogInformation(new DomainLog(string.Format(
                CreateOrderMessages.PurchaseOrderCreatedTemplate, Order.Id),
                userService.UserName));

            await outputPort.Handle(Order);

            if (new SpecialOrderSpecification().IsSatisfiedBy(Order))
            {
                await domainEventHub.Raise(
                    new SpecialOrderCreatedEvent(
                        Order.Id, Order.OrderDetails.Count));
            }

            // Aceptar la transacción
            domainTransaction.CommitTransaction();
        }
        catch
        {
            // Cancelar la transacción
            domainTransaction.RollbackTransaction();

            string Information = string.Format(
                CreateOrderMessages.OrderCreationCancelledTemplate,
                Order.Id);

            await domainLogger.LogInformation(
                new DomainLog(Information, userService.UserName));
            throw;
        }
    }
}