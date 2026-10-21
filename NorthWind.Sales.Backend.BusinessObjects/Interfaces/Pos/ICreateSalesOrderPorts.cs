namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos;

public interface ICreateSalesOrderInputPort
{
    Task Handle(CreateSalesOrderDto orderDto);
}

public interface ICreateSalesOrderOutputPort
{
    int OrderId { get; }
    Task Handle(int orderId);
}
