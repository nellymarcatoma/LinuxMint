namespace NorthWind.Sales.Backend.UseCases.Pos;

internal class SearchSalesOrdersInteractor(
    ISearchSalesOrdersOutputPort outputPort,
    IPosQueriesRepository repository) : ISearchSalesOrdersInputPort
{
    public async Task Handle(SalesOrderSearchFilterDto filter)
    {
        var result = await repository.SearchSalesOrdersAsync(filter);
        await outputPort.Handle(result);
    }
}
