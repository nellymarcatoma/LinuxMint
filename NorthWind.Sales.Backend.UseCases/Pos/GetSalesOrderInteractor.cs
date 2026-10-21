namespace NorthWind.Sales.Backend.UseCases.Pos;

internal class GetSalesOrderInteractor(
    IGetSalesOrderOutputPort outputPort,
    IPosQueriesRepository repository) : IGetSalesOrderInputPort
{
    public async Task Handle(int orderId)
    {
        var order = await repository.GetSalesOrderByIdAsync(orderId);
        await outputPort.Handle(order);
    }
}
