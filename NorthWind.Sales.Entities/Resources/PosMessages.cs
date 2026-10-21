namespace NorthWind.Sales.Entities.Resources;

public static class PosMessages
{
    public const string CustomerIdRequired = "Customer ID is required.";
    public const string DetailsRequired = "Sales order must contain at least one product.";
    public const string ProductIdGreaterThanZero = "Product ID must be greater than zero.";
    public const string QuantityGreaterThanZero = "Quantity must be greater than zero.";
    public const string UnitPriceGreaterThanZero = "Unit price must be greater than zero.";
    public const string DuplicateProductInDetails = "A product cannot appear more than once in the order details.";
    public const string InsufficientStock = "Insufficient stock for product ID {0}. Available: {1}, Requested: {2}.";
    public const string ProductNotFound = "Product ID {0} was not found.";
    public const string CustomerNotFound = "Customer '{0}' was not found.";
    public const string CustomerHasOutstandingBalance = "Customer '{0}' has outstanding balance of {1}.";
    public const string SalesOrderCreatedTemplate = "Sales Order #{0} created successfully.";
}
