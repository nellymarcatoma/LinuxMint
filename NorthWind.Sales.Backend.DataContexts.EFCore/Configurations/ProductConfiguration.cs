namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

internal class ProductConfiguration :
    IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(p => p.UnitPrice)
            .HasPrecision(8, 2);

        builder.Property(p => p.Stock);

        builder.HasData(
            new Product
            {
                Id = 1,
                Name = "Chai",
                UnitPrice = 35,
                Stock = 20,
                UnitsInStock = 20
            },
            new Product
            {
                Id = 2,
                Name = "Chang",
                UnitPrice = 55,
                Stock = 0,
                UnitsInStock = 0
            },
            new Product
            {
                Id = 3,
                Name = "Aniseed Syrup",
                UnitPrice = 65,
                Stock = 20,
                UnitsInStock = 20
            },
            new Product
            {
                Id = 4,
                Name = "Chef Anton's Cajun Seasoning",
                UnitPrice = 75,
                Stock = 40,
                UnitsInStock = 40
            },
            new Product
            {
                Id = 5,
                Name = "Chef Anton's Gumbo Mix",
                UnitPrice = 50,
                Stock = 20,
                UnitsInStock = 20
            });
    }
}
