namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

internal class SalesOrderConfiguration : IEntityTypeConfiguration<Repositories.Entities.SalesOrder>
{
    public void Configure(EntityTypeBuilder<Repositories.Entities.SalesOrder> builder)
    {
        builder.ToTable("SalesOrders");
        builder.HasKey(o => o.OrderId);

        builder.Property(o => o.CustomerId)
            .IsRequired()
            .HasMaxLength(5)
            .IsFixedLength();

        builder.Property(o => o.Subtotal)
            .HasPrecision(18, 2);

        builder.Property(o => o.Tax)
            .HasPrecision(18, 2);

        builder.Property(o => o.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(o => o.UserId)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId);
    }
}
