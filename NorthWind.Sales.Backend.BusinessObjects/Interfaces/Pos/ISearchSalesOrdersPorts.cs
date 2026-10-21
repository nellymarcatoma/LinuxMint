namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos;

public interface ISearchSalesOrdersInputPort
{
    Task Handle(SalesOrderSearchFilterDto filter);
}

public interface ISearchSalesOrdersOutputPort
{
    PagedResult<SalesOrderDto> Result { get; }
    Task Handle(PagedResult<SalesOrderDto> result);
}
