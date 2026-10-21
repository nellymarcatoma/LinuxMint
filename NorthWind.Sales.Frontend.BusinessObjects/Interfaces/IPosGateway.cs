namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface IPosGateway
{
    Task<int> CreateSalesOrderAsync(CreateSalesOrderDto order);
    Task<PagedResult<PosCustomerDto>> SearchCustomersAsync(CustomerSearchFilterDto filter);
    Task<PosCustomerDto?> GetCustomerByIdAsync(PosCustomerDto customer);
    Task<PagedResult<PosProductDto>> SearchProductsAsync(ProductSearchFilterDto filter);
    Task<PosProductDto?> GetProductByIdAsync(int productId);
    Task<PagedResult<SalesOrderDto>> SearchSalesOrdersAsync(SalesOrderSearchFilterDto filter);
    Task<SalesOrderDto?> GetSalesOrderByIdAsync(int orderId);
}
