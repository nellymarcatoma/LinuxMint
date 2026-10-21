namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos;

public interface ISearchCustomersInputPort
{
    Task Handle(CustomerSearchFilterDto filter);
}

public interface ISearchCustomersOutputPort
{
    PagedResult<PosCustomerDto> Result { get; }
    Task Handle(PagedResult<PosCustomerDto> result);
}
