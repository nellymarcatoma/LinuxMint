namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddWebApiGateways(
        this IServiceCollection services,
        Action<HttpClient> configureHttpClient,
        Action<IHttpClientBuilder> configureHttpClientBuilder)
    {
        services.AddExceptionDelegatingHandler();
        var Builder = services
            .AddHttpClient<ICreateOrderGateway,
                CreateOrderGateway>(configureHttpClient)
            .AddHttpMessageHandler<ExceptionDelegatingHandler>();

        var posBuilder = services
            .AddHttpClient<NorthWind.Sales.Frontend.BusinessObjects.Interfaces.IPosGateway,
                PosGateway>(configureHttpClient)
            .AddHttpMessageHandler<ExceptionDelegatingHandler>();

        configureHttpClientBuilder(posBuilder);

        return services;
    }
}
