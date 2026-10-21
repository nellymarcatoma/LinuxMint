namespace NorthWind.Sales.Backend.Repositories.Repositories;

internal class PosCommandsRepository(INorthWindSalesCommandsDataContext context) : IPosCommandsRepository
{
    public async Task<int> CreateSalesOrderAsync(CreateSalesOrderDto orderDto, string userId)
    {
        decimal subtotal = orderDto.Details.Sum(d => d.UnitPrice * d.Quantity);
        decimal tax = Math.Round(subtotal * 0.15m, 2);
        decimal totalAmount = subtotal + tax;

        var salesOrder = new Entities.SalesOrder
        {
            CustomerId = orderDto.CustomerId,
            OrderDate = DateTime.Now,
            Subtotal = subtotal,
            Tax = tax,
            TotalAmount = totalAmount,
            UserId = userId
        };

        await context.AddSalesOrderAsync(salesOrder);

        var details = new List<Entities.SalesOrderDetail>();

        foreach (var detailDto in orderDto.Details)
        {
            var product = await context.FindProductAsync(detailDto.ProductId);
            if (product == null)
            {
                throw new ValidationException(
                [
                    new ValidationError(
                        nameof(detailDto.ProductId),
                        string.Format(PosMessages.ProductNotFound, detailDto.ProductId))
                ]);
            }

            if (product.Stock < detailDto.Quantity)
            {
                throw new ValidationException(
                [
                    new ValidationError(
                        nameof(detailDto.Quantity),
                        string.Format(PosMessages.InsufficientStock, product.Id, product.Stock, detailDto.Quantity))
                ]);
            }

            // Deduce inventory stock automatically
            product.Stock -= detailDto.Quantity;
            product.UnitsInStock = (short)Math.Max(0, product.Stock);
            await context.UpdateProductAsync(product);

            details.Add(new Entities.SalesOrderDetail
            {
                SalesOrder = salesOrder,
                ProductId = detailDto.ProductId,
                UnitPrice = detailDto.UnitPrice,
                Quantity = detailDto.Quantity,
                Subtotal = detailDto.UnitPrice * detailDto.Quantity
            });
        }

        await context.AddSalesOrderDetailsAsync(details);
        await context.SaveChangesAsync();

        return salesOrder.OrderId;
    }

    public async Task SaveChangesAsync() =>
        await context.SaveChangesAsync();
}
