namespace NorthWind.Sales.Backend.Repositories.Entities;

public class SalesOrder
{
    public int OrderId { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public Customer? Customer { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal TotalAmount { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ICollection<SalesOrderDetail> Details { get; set; } = [];
}
