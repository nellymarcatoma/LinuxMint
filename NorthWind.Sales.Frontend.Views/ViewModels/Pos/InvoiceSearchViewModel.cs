namespace NorthWind.Sales.Frontend.Views.ViewModels.Pos;

public class InvoiceSearchViewModel(IPosGateway gateway)
{
    public string SearchTerm { get; set; } = string.Empty;
    public PagedResult<SalesOrderDto> PagedResult { get; private set; } = new();
    public SalesOrderDto? SelectedOrder { get; private set; }
    public bool IsLoading { get; private set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public string? ErrorMessage { get; set; }
    public bool IsUnauthorized { get; set; }

    public async Task SearchAsync(int page = 1)
    {
        CurrentPage = page;
        IsLoading = true;
        ErrorMessage = null;
        IsUnauthorized = false;
        try
        {
            PagedResult = await gateway.SearchSalesOrdersAsync(new SalesOrderSearchFilterDto
            {
                SearchTerm = SearchTerm,
                PageIndex = CurrentPage,
                PageSize = PageSize
            });
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            IsUnauthorized = true;
            ErrorMessage = "Tu sesión terminó. Inicia sesión de nuevo para continuar.";
            PagedResult = new();
        }
        catch (Exception)
        {
            ErrorMessage = "No se pudieron cargar las facturas. Revisa tu conexión e inténtalo de nuevo.";
            PagedResult = new();
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task SelectOrderAsync(int orderId)
    {
        IsLoading = true;
        ErrorMessage = null;
        IsUnauthorized = false;
        try
        {
            SelectedOrder = await gateway.GetSalesOrderByIdAsync(orderId);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            IsUnauthorized = true;
            ErrorMessage = "Tu sesión terminó. Inicia sesión de nuevo para continuar.";
        }
        catch (Exception)
        {
            ErrorMessage = "No se pudo abrir la factura. Inténtalo de nuevo.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void ClearSelectedOrder()
    {
        SelectedOrder = null;
    }

    public async Task GoToPageAsync(int page)
    {
        if (page >= 1 && page <= PagedResult.TotalPages)
        {
            await SearchAsync(page);
        }
    }
}
