namespace NorthWind.Sales.Entities.Dtos.Pos;

public class ProductSearchFilterDto
{
    public string? SearchTerm { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public bool OnlyInStock { get; set; } = true;
}
