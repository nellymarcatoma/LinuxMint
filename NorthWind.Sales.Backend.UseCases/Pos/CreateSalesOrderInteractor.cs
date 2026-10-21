namespace NorthWind.Sales.Backend.UseCases.Pos;

internal class CreateSalesOrderInteractor(
    ICreateSalesOrderOutputPort outputPort,
    IPosCommandsRepository repository,
    IPosQueriesRepository queriesRepository,
    IQueriesRepository customerBalanceRepository,
    IModelValidatorHub<CreateSalesOrderDto> modelValidatorHub,
    IDomainLogger domainLogger,
    IDomainTransaction domainTransaction,
    IUserService userService,
    IMailService mailService) : ICreateSalesOrderInputPort
{
    public async Task Handle(CreateSalesOrderDto orderDto)
    {
        GuardUser.AgainstUnauthenticated(userService);
        await GuardModel.AgainstNotValid(modelValidatorHub, orderDto);

        var customer = await queriesRepository.GetCustomerByIdAsync(orderDto.CustomerId);
        if (customer == null)
        {
            throw new ValidationException(
            [
                new ValidationError(
                    nameof(orderDto.CustomerId),
                    string.Format(PosMessages.CustomerNotFound, orderDto.CustomerId))
            ]);
        }

        var currentBalance = await customerBalanceRepository.GetCustomerCurrentBalance(orderDto.CustomerId);
        if (currentBalance is null)
        {
            throw new ValidationException(
            [
                new ValidationError(
                    nameof(orderDto.CustomerId),
                    string.Format(PosMessages.CustomerNotFound, orderDto.CustomerId))
            ]);
        }

        if (currentBalance > 0)
        {
            throw new ValidationException(
            [
                new ValidationError(
                    nameof(orderDto.CustomerId),
                    string.Format(PosMessages.CustomerHasOutstandingBalance, customer.FullName, currentBalance.Value))
            ]);
        }

        await domainLogger.LogInformation(new DomainLog(
            $"Starting Point of Sale order creation for customer {customer.FullName} ({customer.Dni}).",
            userService.UserName));

        try
        {
            domainTransaction.BeginTransaction();

            int orderId = await repository.CreateSalesOrderAsync(orderDto, userService.UserName);

            await domainLogger.LogInformation(new DomainLog(
                $"Point of Sale order #{orderId} created successfully for customer {customer.FullName} by {userService.UserName}.",
                userService.UserName));

            await outputPort.Handle(orderId);

            domainTransaction.CommitTransaction();

            // Enviar notificación a Mailtrap cuando la compra sea de más de 3 productos
            int totalQuantity = orderDto.Details.Sum(d => d.Quantity);
            int totalProducts = orderDto.Details.Count;
            if (totalProducts > 3 || totalQuantity > 3)
            {
                try
                {
                    int productsCount = Math.Max(totalProducts, totalQuantity);
                    await mailService.SendMailToAdministrator(
                        CreateOrderMessages.SendEmailSubject,
                        string.Format(CreateOrderMessages.SendEmailBodyTemplate, orderId, productsCount));

                    await domainLogger.LogInformation(new DomainLog(
                        $"Notificación enviada a Mailtrap para orden #{orderId} con {productsCount} productos.",
                        userService.UserName));
                }
                catch (Exception mailEx)
                {
                    await domainLogger.LogInformation(new DomainLog(
                        $"Aviso de correo: no se pudo enviar a Mailtrap para la orden #{orderId}: {mailEx.Message}",
                        userService.UserName));
                }
            }
        }
        catch
        {
            domainTransaction.RollbackTransaction();

            await domainLogger.LogInformation(new DomainLog(
                $"Point of Sale order creation rolled back for customer {orderDto.CustomerId}.",
                userService.UserName));
            throw;
        }
    }
}
