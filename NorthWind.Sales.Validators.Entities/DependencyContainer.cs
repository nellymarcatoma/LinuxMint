namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddValidators(
        this IServiceCollection services)
    {
        services.AddModelValidator<CreateOrderDto,
            CreateOrderDtoValidator>();
        services.AddModelValidator<CreateOrderDetailDto,
            CreateOrderDetailDtoValidator>();
        services.AddModelValidator<NorthWind.Sales.Entities.Dtos.Pos.CreateSalesOrderDto,
            NorthWind.Sales.Validators.Entities.Pos.CreateSalesOrderDtoValidator>();
        services.AddModelValidator<NorthWind.Sales.Entities.Dtos.Pos.CreateSalesOrderDetailDto,
            NorthWind.Sales.Validators.Entities.Pos.CreateSalesOrderDetailDtoValidator>();
        return services;
    }
}
