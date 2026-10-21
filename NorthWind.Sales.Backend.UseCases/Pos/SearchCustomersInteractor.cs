namespace NorthWind.Sales.Backend.UseCases.Pos;

internal class SearchCustomersInteractor(
    ISearchCustomersOutputPort outputPort,
    IPosQueriesRepository repository) : ISearchCustomersInputPort
{
    public async Task Handle(CustomerSearchFilterDto filter)
    {
        var result = await repository.SearchCustomersAsync(filter);
        await outputPort.Handle(result);
    }
}
