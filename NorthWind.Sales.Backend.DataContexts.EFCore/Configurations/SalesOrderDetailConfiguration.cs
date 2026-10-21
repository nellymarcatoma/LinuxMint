namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

internal class SalesOrderDetailConfiguration : IEntityTypeConfiguration<Repositories.Entities.SalesOrderDetail>
{
    public void Configure(EntityTypeBuilder<Repositories.Entities.SalesOrderDetail> builder)
    {
        builder.ToTable("SalesOrderDetails");
        builder.HasKey(d => d.OrderDetailId);

        builder.Property(d => d.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(d => d.Subtotal)
            .HasPrecision(18, 2);

        builder.HasOne(d => d.SalesOrder)
            .WithMany(o => o.Details)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Product)
            .WithMany()
            .HasForeignKey(d => d.ProductId);
    }
}
