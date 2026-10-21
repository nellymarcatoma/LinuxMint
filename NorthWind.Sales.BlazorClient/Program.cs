using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NorthWind.Sales.BlazorClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string webApiAddress = builder.Configuration["WebApiAddress"] ?? "https://localhost:7154";
if (builder.HostEnvironment.BaseAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
    webApiAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
{
    webApiAddress = "http://localhost:5163";
}

builder.Services.AddNorthWindSalesServices(client =>
{
    client.BaseAddress = new Uri(webApiAddress);
},
membershipHttpClient =>
{
    membershipHttpClient.BaseAddress = new Uri(webApiAddress);
},
httpClientBuilder =>
    httpClientBuilder.AddMembershipBearerTokenHandler());

await builder.Build().RunAsync();
