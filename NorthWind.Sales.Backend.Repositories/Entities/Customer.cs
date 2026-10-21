namespace NorthWind.Sales.Backend.Repositories.Entities;

public class Customer
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Dni { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}
