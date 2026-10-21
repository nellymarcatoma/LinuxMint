namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;

internal class NorthWindSalesCommandsDataContext(
    IOptions<DBOptions> dbOptions) :
    NorthWindSalesContext(dbOptions),
    INorthWindSalesCommandsDataContext
{
    public async Task AddOrderAsync(Order order) =>
        await AddAsync(order);

    public async Task AddOrderDetailsAsync(
        IEnumerable<Repositories.Entities.OrderDetail> orderDetails) =>
        await AddRangeAsync(orderDetails);

    public async Task AddSalesOrderAsync(Repositories.Entities.SalesOrder salesOrder) =>
        await AddAsync(salesOrder);

    public async Task AddSalesOrderDetailsAsync(
        IEnumerable<Repositories.Entities.SalesOrderDetail> salesOrderDetails) =>
        await AddRangeAsync(salesOrderDetails);

    public Task UpdateProductAsync(Repositories.Entities.Product product)
    {
        Products.Update(product);
        return Task.CompletedTask;
    }

    public async Task<Repositories.Entities.Product?> FindProductAsync(int productId) =>
        await Products.FindAsync(productId);

    public async Task<Repositories.Entities.Customer?> FindCustomerAsync(string customerId) =>
        await Customers.FindAsync(customerId);

    public async Task SaveChangesAsync() =>
        await GuardDBContext.AgainstSaveChangesErrorAsync(this);
}