namespace NorthWind.Sales.Entities.Dtos.Pos;

public class SalesOrderDto
{
    public int OrderId { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerDni { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal TotalAmount { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<SalesOrderDetailDto> Details { get; set; } = [];
}
