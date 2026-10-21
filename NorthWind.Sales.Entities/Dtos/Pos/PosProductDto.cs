namespace NorthWind.Sales.Entities.Dtos.Pos;

public class PosProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Stock { get; set; }
}
