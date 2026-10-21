namespace NorthWind.Sales.Validators.Entities.Pos;

internal class CreateSalesOrderDtoValidator : AbstractModelValidator<CreateSalesOrderDto>
{
    public CreateSalesOrderDtoValidator(
        IValidationService<CreateSalesOrderDto> validationService,
        IModelValidator<CreateSalesOrderDetailDto> detailValidator) : base(validationService)
    {
        AddRuleFor(c => c.CustomerId)
            .NotEmpty(PosMessages.CustomerIdRequired);

        AddRuleFor(c => c.Details)
            .NotNull(PosMessages.DetailsRequired)
            .NotEmpty(PosMessages.DetailsRequired)
            .Must(details => details == null || details.Select(d => d.ProductId).Distinct().Count() == details.Count,
                PosMessages.DuplicateProductInDetails);

        AddRuleForEach(c => c.Details)
            .SetValidator(detailValidator);
    }
}
