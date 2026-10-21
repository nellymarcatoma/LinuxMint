namespace NorthWind.Sales.Backend.UseCases.Pos;

internal class SearchProductsInteractor(
    ISearchProductsOutputPort outputPort,
    IPosQueriesRepository repository) : ISearchProductsInputPort
{
    public async Task Handle(ProductSearchFilterDto filter)
    {
        // Enforce only products with stock > 0 as required by requirement 8
        filter.OnlyInStock = true;
        var result = await repository.SearchProductsAsync(filter);
        await outputPort.Handle(result);
    }
}
