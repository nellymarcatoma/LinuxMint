namespace NorthWind.Sales.Backend.Repositories.Entities;

public class SalesOrderDetail
{
    public int OrderDetailId { get; set; }
    public int OrderId { get; set; }
    public SalesOrder? SalesOrder { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}
