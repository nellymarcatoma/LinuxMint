namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;

internal class NorthWindSalesQueriesDataContext :
    NorthWindSalesContext,
    INorthWindSalesQueriesDataContext
{
    public NorthWindSalesQueriesDataContext(IOptions<DBOptions> dbOptions)
        : base(dbOptions)
    {
        ChangeTracker.QueryTrackingBehavior =
            QueryTrackingBehavior.NoTracking;
    }

    public new IQueryable<Customer> Customers => base.Customers;
    public new IQueryable<Product> Products => base.Products;
    public new IQueryable<Repositories.Entities.SalesOrder> SalesOrders => base.SalesOrders;
    public new IQueryable<Repositories.Entities.SalesOrderDetail> SalesOrderDetails => base.SalesOrderDetails;

    public async Task<ReturnType?> FirstOrDefaultAync<ReturnType>(
        IQueryable<ReturnType> queryable) =>
        await queryable.FirstOrDefaultAsync();

    public async Task<ReturnType?> FirstOrDefaultAsync<ReturnType>(
        IQueryable<ReturnType> queryable) =>
        await queryable.FirstOrDefaultAsync();

    public async Task<IEnumerable<ReturnType>> ToListAsync<ReturnType>(
        IQueryable<ReturnType> queryable) =>
        await queryable.ToListAsync();

    public async Task<int> CountAsync<ReturnType>(
        IQueryable<ReturnType> queryable) =>
        await queryable.CountAsync();
}
