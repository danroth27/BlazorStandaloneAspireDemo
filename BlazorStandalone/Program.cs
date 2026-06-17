using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using BlazorStandalone;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// The gateway injects service endpoints and OTLP settings as environment variables
// (via the ClientServiceDefaults JS initializer). Service discovery reads from
// IConfiguration, so bridge those environment variables into configuration.
builder.Configuration.AddEnvironmentVariables();

// Add Aspire client service defaults (OpenTelemetry + service discovery).
builder.AddBlazorClientServiceDefaults();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Named HttpClient for the weather API, resolved through Aspire service discovery.
builder.Services.AddHttpClient("apiservice", client =>
{
    client.BaseAddress = new Uri("https+http://apiservice");
});

var host = builder.Build();

await host.RunAsync();
