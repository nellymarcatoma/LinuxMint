namespace NorthWind.Sales.Backend.Repositories.Interfaces;

public interface INorthWindSalesCommandsDataContext
{
    Task AddOrderAsync(Order order);
    Task AddOrderDetailsAsync(IEnumerable<Entities.OrderDetail> orderDetails);
    Task AddSalesOrderAsync(Entities.SalesOrder salesOrder);
    Task AddSalesOrderDetailsAsync(IEnumerable<Entities.SalesOrderDetail> salesOrderDetails);
    Task UpdateProductAsync(Entities.Product product);
    Task<Entities.Product?> FindProductAsync(int productId);
    Task<Entities.Customer?> FindCustomerAsync(string customerId);
    Task SaveChangesAsync();
}
