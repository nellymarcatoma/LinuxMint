namespace NorthWind.Sales.Entities.ValueObjects;

public class Endpoints
{
    public const string CreateOrder = $"/{nameof(CreateOrder)}";
    public const string CreateSalesOrder = "/CreateSalesOrder";
    public const string SearchCustomers = "/SearchCustomers";
    public const string SearchProducts = "/SearchProducts";
    public const string SearchSalesOrders = "/SearchSalesOrders";
    public const string GetSalesOrder = "/GetSalesOrder";
}
