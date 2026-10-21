namespace NorthWind.Sales.Frontend.BusinessObjects.Exceptions;

public sealed class StockUnavailableException(
    int productId,
    int availableStock,
    int requestedQuantity) : Exception
{
    public int ProductId { get; } = productId;
    public int AvailableStock { get; } = availableStock;
    public int RequestedQuantity { get; } = requestedQuantity;
}

public sealed class CustomerHasDebtException : Exception { }
