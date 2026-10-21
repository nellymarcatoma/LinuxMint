namespace NorthWind.Membership.Backend.AspNetIdentity.DataContexts;

internal class NorthWindMembershipContextFactory :
    IDesignTimeDbContextFactory<NorthWindMembershipContext>
{
    public NorthWindMembershipContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "NorthWind.Sales.WebApi", "appsettings.Development.json"), optional: true)
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json"), optional: true)
            .AddEnvironmentVariables()
            .Build();

        IOptions<MembershipDBOptions> DbOptions =
            Microsoft.Extensions.Options.Options.Create(
                new MembershipDBOptions()
                {
                    ConnectionString = configuration["MembershipDBOptions:ConnectionString"]
                        ?? "Server=DESKTOP-6JKC7OO\\Sitio_A;Database=NorthWindUsersDB;User Id=sa;TrustServerCertificate=True"
                });
        return new NorthWindMembershipContext(DbOptions);
    }
}
