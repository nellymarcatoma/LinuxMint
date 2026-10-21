namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

internal class CustomerConfiguration :
    IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder
            .Property(c => c.Id)
            .HasMaxLength(5)
            .IsFixedLength();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(c => c.CurrentBalance)
            .HasPrecision(8, 2);

        builder.Property(c => c.FirstName)
            .HasMaxLength(50);

        builder.Property(c => c.LastName)
            .HasMaxLength(50);

        builder.Property(c => c.Dni)
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .HasMaxLength(100);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Address)
            .HasMaxLength(150);

        builder.HasData(
            new Customer
            {
                Id = "ALFKI",
                Name = "Alfreds Futterkiste",
                CurrentBalance = 0
            },
            new Customer
            {
                Id = "ANATR",
                Name = "Ana Trujillo Emparedados y helados",
                CurrentBalance = 0
            },
            new Customer
            {
                Id = "ANTON",
                Name = "Antonio Moreno Taquería",
                CurrentBalance = 100
            });
    }
}
