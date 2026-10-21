namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos;

public interface IGetSalesOrderInputPort
{
    Task Handle(int orderId);
}

public interface IGetSalesOrderOutputPort
{
    SalesOrderDto? Order { get; }
    Task Handle(SalesOrderDto? order);
}
