namespace NorthWind.Sales.Entities.Dtos.Pos;

public class CreateSalesOrderDetailDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
