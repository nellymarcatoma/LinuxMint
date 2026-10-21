namespace NorthWind.Sales.Entities.Dtos.Pos;

public class CreateSalesOrderDto
{
    public string CustomerId { get; set; } = string.Empty;
    public List<CreateSalesOrderDetailDto> Details { get; set; } = [];
}
