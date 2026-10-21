namespace NorthWind.Sales.Backend.Repositories.Repositories;

internal class PosQueriesRepository(INorthWindSalesQueriesDataContext context) : IPosQueriesRepository
{
    public async Task<PagedResult<PosCustomerDto>> SearchCustomersAsync(CustomerSearchFilterDto filter)
    {
        var query = context.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string term = filter.SearchTerm.Trim().ToLower();
            query = query.Where(c =>
                (c.Dni != null && c.Dni.ToLower().Contains(term)) ||
                c.Name.ToLower().Contains(term) ||
                (c.FirstName != null && c.FirstName.ToLower().Contains(term)) ||
                (c.LastName != null && c.LastName.ToLower().Contains(term)));
        }

        int totalCount = await context.CountAsync(query);

        int pageIndex = filter.PageIndex > 0 ? filter.PageIndex : 1;
        int pageSize = filter.PageSize > 0 ? filter.PageSize : 5;

        var pagedQuery = query
            .OrderBy(c => c.Name)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new PosCustomerDto
            {
                CustomerId = c.Id,
                FirstName = c.FirstName ?? c.Name,
                LastName = c.LastName ?? string.Empty,
                Dni = c.Dni ?? string.Empty,
                Email = c.Email ?? string.Empty,
                Phone = c.Phone ?? string.Empty,
                Address = c.Address ?? string.Empty,
                CurrentBalance = c.CurrentBalance
            });

        var items = await context.ToListAsync(pagedQuery);

        return new PagedResult<PosCustomerDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<PagedResult<PosProductDto>> SearchProductsAsync(ProductSearchFilterDto filter)
    {
        var query = context.Products.AsQueryable();

        if (filter.OnlyInStock)
        {
            query = query.Where(p => p.Stock > 0);
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string term = filter.SearchTerm.Trim().ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                p.Id.ToString().Contains(term));
        }

        int totalCount = await context.CountAsync(query);

        int pageIndex = filter.PageIndex > 0 ? filter.PageIndex : 1;
        int pageSize = filter.PageSize > 0 ? filter.PageSize : 5;

        var pagedQuery = query
            .OrderBy(p => p.Name)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PosProductDto
            {
                ProductId = p.Id,
                Name = p.Name,
                UnitPrice = p.UnitPrice,
                Stock = p.Stock
            });

        var items = await context.ToListAsync(pagedQuery);

        return new PagedResult<PosProductDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<PagedResult<SalesOrderDto>> SearchSalesOrdersAsync(SalesOrderSearchFilterDto filter)
    {
        var ordersQuery = context.SalesOrders.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string term = filter.SearchTerm.Trim().ToLower();
            ordersQuery = ordersQuery.Where(o =>
                o.OrderId.ToString().Contains(term) ||
                (o.Customer != null &&
                    (o.Customer.Name.ToLower().Contains(term) ||
                     (o.Customer.Dni != null && o.Customer.Dni.ToLower().Contains(term)) ||
                     (o.Customer.FirstName != null && o.Customer.FirstName.ToLower().Contains(term)) ||
                     (o.Customer.LastName != null && o.Customer.LastName.ToLower().Contains(term)))));
        }

        int totalCount = await context.CountAsync(ordersQuery);

        int pageIndex = filter.PageIndex > 0 ? filter.PageIndex : 1;
        int pageSize = filter.PageSize > 0 ? filter.PageSize : 5;

        var pagedQuery = ordersQuery
            .OrderByDescending(o => o.OrderDate)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new SalesOrderDto
            {
                OrderId = o.OrderId,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer != null ? o.Customer.Name : o.CustomerId,
                CustomerDni = o.Customer != null && o.Customer.Dni != null ? o.Customer.Dni : string.Empty,
                CustomerEmail = o.Customer != null && o.Customer.Email != null ? o.Customer.Email : string.Empty,
                CustomerPhone = o.Customer != null && o.Customer.Phone != null ? o.Customer.Phone : string.Empty,
                CustomerAddress = o.Customer != null && o.Customer.Address != null ? o.Customer.Address : string.Empty,
                OrderDate = o.OrderDate,
                Subtotal = o.Subtotal,
                Tax = o.Tax,
                TotalAmount = o.TotalAmount,
                UserId = o.UserId,
                Details = o.Details.Select(d => new SalesOrderDetailDto
                {
                    OrderDetailId = d.OrderDetailId,
                    OrderId = d.OrderId,
                    ProductId = d.ProductId,
                    ProductName = d.Product != null ? d.Product.Name : d.ProductId.ToString(),
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Subtotal = d.Subtotal
                }).ToList()
            });

        var items = await context.ToListAsync(pagedQuery);

        return new PagedResult<SalesOrderDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<SalesOrderDto?> GetSalesOrderByIdAsync(int orderId)
    {
        var query = context.SalesOrders
            .Where(o => o.OrderId == orderId)
            .Select(o => new SalesOrderDto
            {
                OrderId = o.OrderId,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer != null ? o.Customer.Name : o.CustomerId,
                CustomerDni = o.Customer != null && o.Customer.Dni != null ? o.Customer.Dni : string.Empty,
                CustomerEmail = o.Customer != null && o.Customer.Email != null ? o.Customer.Email : string.Empty,
                CustomerPhone = o.Customer != null && o.Customer.Phone != null ? o.Customer.Phone : string.Empty,
                CustomerAddress = o.Customer != null && o.Customer.Address != null ? o.Customer.Address : string.Empty,
                OrderDate = o.OrderDate,
                Subtotal = o.Subtotal,
                Tax = o.Tax,
                TotalAmount = o.TotalAmount,
                UserId = o.UserId,
                Details = o.Details.Select(d => new SalesOrderDetailDto
                {
                    OrderDetailId = d.OrderDetailId,
                    OrderId = d.OrderId,
                    ProductId = d.ProductId,
                    ProductName = d.Product != null ? d.Product.Name : d.ProductId.ToString(),
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Subtotal = d.Subtotal
                }).ToList()
            });

        return await context.FirstOrDefaultAsync(query);
    }

    public async Task<PosCustomerDto?> GetCustomerByIdAsync(string customerId)
    {
        var query = context.Customers
            .Where(c => c.Id == customerId)
            .Select(c => new PosCustomerDto
            {
                CustomerId = c.Id,
                FirstName = c.FirstName ?? c.Name,
                LastName = c.LastName ?? string.Empty,
                Dni = c.Dni ?? string.Empty,
                Email = c.Email ?? string.Empty,
                Phone = c.Phone ?? string.Empty,
                Address = c.Address ?? string.Empty,
                CurrentBalance = c.CurrentBalance
            });

        return await context.FirstOrDefaultAsync(query);
    }

    public async Task<PosProductDto?> GetProductByIdAsync(int productId)
    {
        var query = context.Products
            .Where(p => p.Id == productId)
            .Select(p => new PosProductDto
            {
                ProductId = p.Id,
                Name = p.Name,
                UnitPrice = p.UnitPrice,
                Stock = p.Stock
            });

        return await context.FirstOrDefaultAsync(query);
    }

    public async Task<IEnumerable<PosProductDto>> GetProductsByIdsAsync(IEnumerable<int> productIds)
    {
        var query = context.Products
            .Where(p => productIds.Contains(p.Id))
            .Select(p => new PosProductDto
            {
                ProductId = p.Id,
                Name = p.Name,
                UnitPrice = p.UnitPrice,
                Stock = p.Stock
            });

        return await context.ToListAsync(query);
    }
}
