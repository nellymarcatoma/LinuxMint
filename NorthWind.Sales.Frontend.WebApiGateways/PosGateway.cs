using System.Text.RegularExpressions;
using NorthWind.Sales.Frontend.BusinessObjects.Exceptions;

namespace NorthWind.Sales.Frontend.WebApiGateways;

internal class PosGateway(HttpClient client) : IPosGateway
{
    public async Task<int> CreateSalesOrderAsync(CreateSalesOrderDto order)
    {
        var response = await client.PostAsJsonAsync(Endpoints.CreateSalesOrder, order);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            var stockError = Regex.Match(
                errorContent,
                @"Insufficient stock for product ID\s+(\d+)\.\s*Available:\s*(\d+),\s*Requested:\s*(\d+)",
                RegexOptions.IgnoreCase);

            if (stockError.Success)
            {
                throw new StockUnavailableException(
                    int.Parse(stockError.Groups[1].Value),
                    int.Parse(stockError.Groups[2].Value),
                    int.Parse(stockError.Groups[3].Value));
            }

            if (errorContent.Contains("has outstanding balance", StringComparison.OrdinalIgnoreCase))
            {
                throw new CustomerHasDebtException();
            }

            response.EnsureSuccessStatusCode();
        }

        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<PagedResult<PosCustomerDto>> SearchCustomersAsync(CustomerSearchFilterDto filter)
    {
        string query = $"?searchTerm={Uri.EscapeDataString(filter.SearchTerm ?? string.Empty)}&pageIndex={filter.PageIndex}&pageSize={filter.PageSize}";
        var response = await client.GetFromJsonAsync<PagedResult<PosCustomerDto>>($"{Endpoints.SearchCustomers}{query}");
        return response ?? new PagedResult<PosCustomerDto>();
    }

    public async Task<PosCustomerDto?> GetCustomerByIdAsync(PosCustomerDto customer)
    {
        string searchTerm = !string.IsNullOrWhiteSpace(customer.Dni)
            ? customer.Dni
            : customer.FullName;
        var customers = await SearchCustomersAsync(new CustomerSearchFilterDto
        {
            SearchTerm = searchTerm,
            PageIndex = 1,
            PageSize = 1000
        });

        return customers.Items.FirstOrDefault(result => result.CustomerId == customer.CustomerId);
    }

    public async Task<PagedResult<PosProductDto>> SearchProductsAsync(ProductSearchFilterDto filter)
    {
        string query = $"?searchTerm={Uri.EscapeDataString(filter.SearchTerm ?? string.Empty)}&pageIndex={filter.PageIndex}&pageSize={filter.PageSize}&onlyInStock={filter.OnlyInStock}";
        var response = await client.GetFromJsonAsync<PagedResult<PosProductDto>>($"{Endpoints.SearchProducts}{query}");
        return response ?? new PagedResult<PosProductDto>();
    }

    public async Task<PosProductDto?> GetProductByIdAsync(int productId)
    {
        var products = await SearchProductsAsync(new ProductSearchFilterDto
        {
            SearchTerm = productId.ToString(),
            PageIndex = 1,
            PageSize = 1000,
            OnlyInStock = true
        });

        return products.Items.FirstOrDefault(product => product.ProductId == productId);
    }

    public async Task<PagedResult<SalesOrderDto>> SearchSalesOrdersAsync(SalesOrderSearchFilterDto filter)
    {
        string query = $"?searchTerm={Uri.EscapeDataString(filter.SearchTerm ?? string.Empty)}&pageIndex={filter.PageIndex}&pageSize={filter.PageSize}";
        var response = await client.GetFromJsonAsync<PagedResult<SalesOrderDto>>($"{Endpoints.SearchSalesOrders}{query}");
        return response ?? new PagedResult<SalesOrderDto>();
    }

    public async Task<SalesOrderDto?> GetSalesOrderByIdAsync(int orderId)
    {
        try
        {
            return await client.GetFromJsonAsync<SalesOrderDto>($"{Endpoints.GetSalesOrder}/{orderId}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }
}
