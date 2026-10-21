namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddUseCasesServices(
        this IServiceCollection services)
    {
        services.AddScoped<ICreateOrderInputPort,
            CreateOrderInteractor>();
        services.AddModelValidator<CreateOrderDto,
            CreateOrderCustomerValidator>();
        services.AddModelValidator<CreateOrderDto,
            CreateOrderProductValidator>();
        services.AddScoped<IDomainEventHandler<SpecialOrderCreatedEvent>,
            SendEMailWhenSpecialOrderCreatedEventHandler>();
        services.AddScoped<NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ICreateSalesOrderInputPort,
            NorthWind.Sales.Backend.UseCases.Pos.CreateSalesOrderInteractor>();
        services.AddScoped<NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ISearchCustomersInputPort,
            NorthWind.Sales.Backend.UseCases.Pos.SearchCustomersInteractor>();
        services.AddScoped<NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ISearchProductsInputPort,
            NorthWind.Sales.Backend.UseCases.Pos.SearchProductsInteractor>();
        services.AddScoped<NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ISearchSalesOrdersInputPort,
            NorthWind.Sales.Backend.UseCases.Pos.SearchSalesOrdersInteractor>();
        services.AddScoped<NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.IGetSalesOrderInputPort,
            NorthWind.Sales.Backend.UseCases.Pos.GetSalesOrderInteractor>();
        return services;
    }
}
