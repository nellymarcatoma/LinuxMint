namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddPresenters(
        this IServiceCollection services)
    {
        services.AddScoped<ICreateOrderOutputPort, CreateOrderPresenter>();
        services.AddScoped<NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ICreateSalesOrderOutputPort,
            NorthWind.Sales.Backend.Presenters.Pos.CreateSalesOrderPresenter>();
        services.AddScoped<NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ISearchCustomersOutputPort,
            NorthWind.Sales.Backend.Presenters.Pos.SearchCustomersPresenter>();
        services.AddScoped<NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ISearchProductsOutputPort,
            NorthWind.Sales.Backend.Presenters.Pos.SearchProductsPresenter>();
        services.AddScoped<NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ISearchSalesOrdersOutputPort,
            NorthWind.Sales.Backend.Presenters.Pos.SearchSalesOrdersPresenter>();
        services.AddScoped<NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.IGetSalesOrderOutputPort,
            NorthWind.Sales.Backend.Presenters.Pos.GetSalesOrderPresenter>();
        return services;
    }
}
