namespace NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

internal class NorthWindDomainLogsContextFactory :
    IDesignTimeDbContextFactory<NorthWindDomainLogsContext>
{
    public NorthWindDomainLogsContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "NorthWind.Sales.WebApi", "appsettings.Development.json"), optional: true)
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json"), optional: true)
            .AddEnvironmentVariables()
            .Build();

        IOptions<DBOptions> DbOptions =
            Microsoft.Extensions.Options.Options.Create(
                new DBOptions
                {
                    DomainLogsConnectionString = configuration["DBOptions:DomainLogsConnectionString"]
                        ?? "Server=DESKTOP-6JKC7OO\\Sitio_A;Database=NorthWindLogsDB;User Id=sa;TrustServerCertificate=True"
                });
        return new NorthWindDomainLogsContext(DbOptions);
    }
}
