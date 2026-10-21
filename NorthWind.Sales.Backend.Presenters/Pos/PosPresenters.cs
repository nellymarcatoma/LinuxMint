namespace NorthWind.Sales.Backend.Presenters.Pos;

internal class CreateSalesOrderPresenter : ICreateSalesOrderOutputPort
{
    public int OrderId { get; private set; }

    public Task Handle(int orderId)
    {
        OrderId = orderId;
        return Task.CompletedTask;
    }
}

internal class SearchCustomersPresenter : ISearchCustomersOutputPort
{
    public PagedResult<PosCustomerDto> Result { get; private set; } = new();

    public Task Handle(PagedResult<PosCustomerDto> result)
    {
        Result = result;
        return Task.CompletedTask;
    }
}

internal class SearchProductsPresenter : ISearchProductsOutputPort
{
    public PagedResult<PosProductDto> Result { get; private set; } = new();

    public Task Handle(PagedResult<PosProductDto> result)
    {
        Result = result;
        return Task.CompletedTask;
    }
}

internal class SearchSalesOrdersPresenter : ISearchSalesOrdersOutputPort
{
    public PagedResult<SalesOrderDto> Result { get; private set; } = new();

    public Task Handle(PagedResult<SalesOrderDto> result)
    {
        Result = result;
        return Task.CompletedTask;
    }
}

internal class GetSalesOrderPresenter : IGetSalesOrderOutputPort
{
    public SalesOrderDto? Order { get; private set; }

    public Task Handle(SalesOrderDto? order)
    {
        Order = order;
        return Task.CompletedTask;
    }
}
