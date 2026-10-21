namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<ICommandsRepository, CommandsRepository>();
        services.AddScoped<IQueriesRepository, QueriesRepository>();
        services.AddScoped<IDomainLogsRepository, DomainLogsRepository>();
        services.AddScoped<NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.IPosCommandsRepository,
            NorthWind.Sales.Backend.Repositories.Repositories.PosCommandsRepository>();
        services.AddScoped<NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.IPosQueriesRepository,
            NorthWind.Sales.Backend.Repositories.Repositories.PosQueriesRepository>();
        return services;
    }
}
