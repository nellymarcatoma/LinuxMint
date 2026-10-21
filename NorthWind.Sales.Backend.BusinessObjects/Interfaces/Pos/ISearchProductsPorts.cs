namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Pos;

public interface ISearchProductsInputPort
{
    Task Handle(ProductSearchFilterDto filter);
}

public interface ISearchProductsOutputPort
{
    PagedResult<PosProductDto> Result { get; }
    Task Handle(PagedResult<PosProductDto> result);
}
