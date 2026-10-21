namespace Microsoft.AspNetCore.Builder;

public static class PosController
{
    public static WebApplication UsePosController(this WebApplication app)
    {
        app.MapPost(Endpoints.CreateSalesOrder, CreateSalesOrder)
            .RequireAuthorization();

        app.MapGet(Endpoints.SearchCustomers, SearchCustomers)
            .RequireAuthorization();

        app.MapGet(Endpoints.SearchProducts, SearchProducts)
            .RequireAuthorization();

        app.MapGet(Endpoints.SearchSalesOrders, SearchSalesOrders)
            .RequireAuthorization();

        app.MapGet($"{Endpoints.GetSalesOrder}/{{id:int}}", GetSalesOrderById)
            .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> CreateSalesOrder(
        CreateSalesOrderDto orderDto,
        NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ICreateSalesOrderInputPort inputPort,
        NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ICreateSalesOrderOutputPort presenter)
    {
        await inputPort.Handle(orderDto);
        return Results.Ok(presenter.OrderId);
    }

    private static async Task<IResult> SearchCustomers(
        string? searchTerm,
        int? pageIndex,
        int? pageSize,
        NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ISearchCustomersInputPort inputPort,
        NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ISearchCustomersOutputPort presenter)
    {
        var filter = new CustomerSearchFilterDto
        {
            SearchTerm = searchTerm,
            PageIndex = pageIndex ?? 1,
            PageSize = pageSize ?? 5
        };
        await inputPort.Handle(filter);
        return Results.Ok(presenter.Result);
    }

    private static async Task<IResult> SearchProducts(
        string? searchTerm,
        int? pageIndex,
        int? pageSize,
        bool? onlyInStock,
        NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ISearchProductsInputPort inputPort,
        NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ISearchProductsOutputPort presenter)
    {
        var filter = new ProductSearchFilterDto
        {
            SearchTerm = searchTerm,
            PageIndex = pageIndex ?? 1,
            PageSize = pageSize ?? 5,
            OnlyInStock = onlyInStock ?? true
        };
        await inputPort.Handle(filter);
        return Results.Ok(presenter.Result);
    }

    private static async Task<IResult> SearchSalesOrders(
        string? searchTerm,
        int? pageIndex,
        int? pageSize,
        NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ISearchSalesOrdersInputPort inputPort,
        NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.ISearchSalesOrdersOutputPort presenter)
    {
        var filter = new SalesOrderSearchFilterDto
        {
            SearchTerm = searchTerm,
            PageIndex = pageIndex ?? 1,
            PageSize = pageSize ?? 5
        };
        await inputPort.Handle(filter);
        return Results.Ok(presenter.Result);
    }

    private static async Task<IResult> GetSalesOrderById(
        int id,
        NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.IGetSalesOrderInputPort inputPort,
        NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos.IGetSalesOrderOutputPort presenter)
    {
        await inputPort.Handle(id);
        return presenter.Order != null ? Results.Ok(presenter.Order) : Results.NotFound();
    }
}
