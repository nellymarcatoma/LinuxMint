namespace NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

internal class NorthWindContext : DbContext
{
    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "NorthWind.Sales.WebApi", "appsettings.Development.json"), optional: true)
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json"), optional: true)
            .AddEnvironmentVariables()
            .Build();

        optionsBuilder.UseSqlServer(
            configuration["DBOptions:ConnectionString"]
                ?? "Server=DESKTOP-6JKC7OO\\Sitio_A;Database=NorthWindDB;User Id=sa;TrustServerCertificate=True");
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Repositories.Entities.OrderDetail> OrderDetails { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Repositories.Entities.SalesOrder> SalesOrders { get; set; }
    public DbSet<Repositories.Entities.SalesOrderDetail> SalesOrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());
    }
}
