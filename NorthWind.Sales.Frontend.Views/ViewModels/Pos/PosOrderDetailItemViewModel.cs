namespace NorthWind.Sales.Frontend.Views.ViewModels.Pos;

public class PosOrderDetailItemViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int AvailableStock { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal Subtotal => Math.Round(UnitPrice * (Quantity > 0 ? Quantity : 0), 2);
    public bool IsQuantityInvalid => Quantity <= 0;
    public bool IsStockExceeded => AvailableStock <= 0 || Quantity > AvailableStock;
    public bool IsValid => Quantity > 0 && AvailableStock > 0 && Quantity <= AvailableStock;
}
