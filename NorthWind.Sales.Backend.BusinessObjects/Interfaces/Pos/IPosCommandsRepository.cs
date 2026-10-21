namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos;

public interface IPosCommandsRepository
{
    Task<int> CreateSalesOrderAsync(CreateSalesOrderDto orderDto, string userId);
    Task SaveChangesAsync();
}
