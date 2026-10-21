namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos;

public interface IPosQueriesRepository
{
    Task<PagedResult<PosCustomerDto>> SearchCustomersAsync(CustomerSearchFilterDto filter);
    Task<PagedResult<PosProductDto>> SearchProductsAsync(ProductSearchFilterDto filter);
    Task<PagedResult<SalesOrderDto>> SearchSalesOrdersAsync(SalesOrderSearchFilterDto filter);
    Task<SalesOrderDto?> GetSalesOrderByIdAsync(int orderId);
    Task<PosCustomerDto?> GetCustomerByIdAsync(string customerId);
    Task<PosProductDto?> GetProductByIdAsync(int productId);
    Task<IEnumerable<PosProductDto>> GetProductsByIdsAsync(IEnumerable<int> productIds);
}
