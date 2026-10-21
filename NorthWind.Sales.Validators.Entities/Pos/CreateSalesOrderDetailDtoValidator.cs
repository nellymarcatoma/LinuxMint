namespace NorthWind.Sales.Validators.Entities.Pos;

internal class CreateSalesOrderDetailDtoValidator : AbstractModelValidator<CreateSalesOrderDetailDto>
{
    public CreateSalesOrderDetailDtoValidator(
        IValidationService<CreateSalesOrderDetailDto> validator) : base(validator)
    {
        AddRuleFor(d => d.ProductId)
            .GreaterThan(0, PosMessages.ProductIdGreaterThanZero);

        AddRuleFor<int>(d => d.Quantity)
            .GreaterThan(0, PosMessages.QuantityGreaterThanZero);

        AddRuleFor(d => d.UnitPrice)
            .GreaterThan(0m, PosMessages.UnitPriceGreaterThanZero);
    }
}
