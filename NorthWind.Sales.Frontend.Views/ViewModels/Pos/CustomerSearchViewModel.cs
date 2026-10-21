namespace NorthWind.Sales.Frontend.Views.ViewModels.Pos;

public class CustomerSearchViewModel(IPosGateway gateway)
{
    public string SearchTerm { get; set; } = string.Empty;
    public PagedResult<PosCustomerDto> PagedResult { get; private set; } = new();
    public bool IsLoading { get; private set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public string? ErrorMessage { get; set; }

    public async Task SearchAsync(int page = 1)
    {
        CurrentPage = page;
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            PagedResult = await gateway.SearchCustomersAsync(new CustomerSearchFilterDto
            {
                SearchTerm = SearchTerm,
                PageIndex = CurrentPage,
                PageSize = PageSize
            });
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            ErrorMessage = "Tu sesión terminó. Inicia sesión de nuevo para continuar.";
            PagedResult = new();
        }
        catch (Exception)
        {
            ErrorMessage = "No se pudieron cargar los clientes. Revisa tu conexión e inténtalo de nuevo.";
            PagedResult = new();
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task GoToPageAsync(int page)
    {
        if (page >= 1 && page <= PagedResult.TotalPages)
        {
            await SearchAsync(page);
        }
    }
}
