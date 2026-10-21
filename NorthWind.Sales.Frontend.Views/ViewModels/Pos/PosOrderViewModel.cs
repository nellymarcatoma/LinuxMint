using NorthWind.Sales.Frontend.BusinessObjects.Exceptions;

namespace NorthWind.Sales.Frontend.Views.ViewModels.Pos;

public class PosOrderViewModel(IPosGateway gateway)
{
    public PosCustomerDto? SelectedCustomer { get; private set; }
    public List<PosOrderDetailItemViewModel> Details { get; set; } = [];

    public decimal Subtotal => Details.Sum(d => d.Subtotal);
    public decimal Tax => Math.Round(Subtotal * 0.15m, 2);
    public decimal TotalAmount => Subtotal + Tax;

    public bool IsSaving { get; private set; }
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }
    public int? LastCreatedOrderId { get; set; }
    public bool HasCustomerDebt => SelectedCustomer?.CurrentBalance > 0;
    public bool HasInvalidQuantity => Details.Any(d => d.Quantity <= 0 || d.AvailableStock <= 0 || d.Quantity > d.AvailableStock);

    public void SelectCustomer(PosCustomerDto customer)
    {
        SelectedCustomer = customer;
        ErrorMessage = null;
    }

    public void ClearCustomer()
    {
        SelectedCustomer = null;
    }

    public void AddOrUpdateProduct(PosProductDto product, int quantity = 1)
    {
        ErrorMessage = null;
        if (product.Stock <= 0)
        {
            ErrorMessage = $"Ya no hay stock de {product.Name}. No puedes agregarlo a la venta.";
            return;
        }

        var existing = Details.FirstOrDefault(d => d.ProductId == product.ProductId);
        if (existing != null)
        {
            existing.AvailableStock = product.Stock;
            if (existing.Quantity + quantity <= product.Stock)
            {
                existing.Quantity += quantity;
            }
            else
            {
                existing.Quantity = product.Stock;
                ErrorMessage = $"Ya agregaste todas las unidades disponibles de {product.Name}.";
            }
        }
        else
        {
            Details.Add(new PosOrderDetailItemViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.Name,
                UnitPrice = product.UnitPrice,
                AvailableStock = product.Stock,
                Quantity = Math.Min(quantity, product.Stock)
            });
        }
    }

    public void ReplaceProduct(PosOrderDetailItemViewModel item, PosProductDto newProduct)
    {
        ErrorMessage = null;
        if (newProduct.Stock <= 0)
        {
            ErrorMessage = $"Ya no hay stock de {newProduct.Name}. El producto no se cambió.";
            return;
        }

        var existingOther = Details.FirstOrDefault(d => d.ProductId == newProduct.ProductId && d != item);
        if (existingOther != null)
        {
            existingOther.Quantity = Math.Min(existingOther.Quantity + item.Quantity, newProduct.Stock);
            Details.Remove(item);
        }
        else
        {
            item.ProductId = newProduct.ProductId;
            item.ProductName = newProduct.Name;
            item.UnitPrice = newProduct.UnitPrice;
            item.AvailableStock = newProduct.Stock;
            item.Quantity = Math.Min(item.Quantity, newProduct.Stock);
        }
    }

    public void RemoveDetail(PosOrderDetailItemViewModel item)
    {
        Details.Remove(item);
        ErrorMessage = null;
    }

    public void Reset()
    {
        SelectedCustomer = null;
        Details.Clear();
        SuccessMessage = null;
        ErrorMessage = null;
        LastCreatedOrderId = null;
    }

    public void DismissSuccessMessage()
    {
        SuccessMessage = null;
        LastCreatedOrderId = null;
    }

    public async Task<bool> SaveOrderAsync()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        if (SelectedCustomer == null)
        {
            ErrorMessage = "Debe seleccionar un cliente antes de guardar la orden.";
            return false;
        }

        if (Details.Count == 0)
        {
            ErrorMessage = "Debe agregar al menos un producto al detalle de la orden.";
            return false;
        }

        foreach (var item in Details)
        {
            if (item.Quantity <= 0)
            {
                ErrorMessage = $"Ingresa al menos una unidad de {item.ProductName}.";
                return false;
            }

            if (item.AvailableStock <= 0)
            {
                ErrorMessage = $"Ya no hay stock de {item.ProductName}. No puedes registrar la venta.";
                return false;
            }

            if (item.Quantity > item.AvailableStock)
            {
                ErrorMessage = $"No hay suficientes unidades de {item.ProductName}. Disponibles: {item.AvailableStock}.";
                return false;
            }
        }

        IsSaving = true;
        try
        {
            var currentCustomer = await gateway.GetCustomerByIdAsync(SelectedCustomer);
            if (currentCustomer is null)
            {
                ErrorMessage = "No encontramos este cliente. Selecciona otro para continuar.";
                SelectedCustomer = null;
                return false;
            }

            SelectedCustomer.CurrentBalance = currentCustomer.CurrentBalance;
            if (currentCustomer.CurrentBalance > 0)
            {
                ErrorMessage = "Este cliente tiene una deuda pendiente y no puede realizar compras.";
                return false;
            }

            foreach (var item in Details)
            {
                var currentProduct = await gateway.GetProductByIdAsync(item.ProductId);
                if (currentProduct is null || currentProduct.Stock <= 0)
                {
                    item.AvailableStock = 0;
                    ErrorMessage = $"Ya no hay stock de {item.ProductName}. No puedes registrar la venta.";
                    return false;
                }

                item.AvailableStock = currentProduct.Stock;
                if (item.Quantity > currentProduct.Stock)
                {
                    ErrorMessage = $"De {item.ProductName} quedan {currentProduct.Stock} unidades. Ajusta la cantidad para continuar.";
                    return false;
                }
            }

            var dto = new CreateSalesOrderDto
            {
                CustomerId = SelectedCustomer.CustomerId,
                Details = Details.Select(d => new CreateSalesOrderDetailDto
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };

            int orderId = await gateway.CreateSalesOrderAsync(dto);
            LastCreatedOrderId = orderId;
            SuccessMessage = $"Venta #{orderId} registrada correctamente.";
            Details.Clear();
            SelectedCustomer = null;
            return true;
        }
        catch (CustomerHasDebtException)
        {
            if (SelectedCustomer != null)
            {
                SelectedCustomer.CurrentBalance = Math.Max(SelectedCustomer.CurrentBalance, 0.01m);
            }

            ErrorMessage = "Este cliente tiene una deuda pendiente y no puede realizar compras.";
            return false;
        }
        catch (StockUnavailableException ex)
        {
            var item = Details.FirstOrDefault(d => d.ProductId == ex.ProductId);
            if (item != null)
            {
                item.AvailableStock = ex.AvailableStock;
            }

            ErrorMessage = ex.AvailableStock == 0
                ? $"Ya no hay stock de {item?.ProductName ?? "uno de los productos"}. No puedes registrar la venta."
                : $"De {item?.ProductName ?? "este producto"} quedan {ex.AvailableStock} unidades. Ajusta la cantidad para continuar.";
            return false;
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = ex.StatusCode == System.Net.HttpStatusCode.Unauthorized
                ? "Tu sesión terminó. Inicia sesión de nuevo para continuar."
                : "No se pudo guardar la venta. Revisa tu conexión e inténtalo de nuevo.";
            return false;
        }
        catch (Exception)
        {
            ErrorMessage = "No se pudo guardar la venta. Inténtalo de nuevo.";
            return false;
        }
        finally
        {
            IsSaving = false;
        }
    }
}
