var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.BlazorStandalone_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

// Register the standalone Blazor WebAssembly app as a resource.
// The resource name becomes the URL path prefix (e.g., "app" -> served at /app/).
var blazorApp = builder.AddBlazorWasmProject<Projects.BlazorStandalone>("app")
    .WithReference(apiService);

// The Blazor Gateway serves the WASM static files and proxies API/OTLP traffic.
builder.AddBlazorGateway("gateway")
    .WithExternalHttpEndpoints()
    .WithOtlpExporter()
    .WithBlazorClientApp(blazorApp);

builder.Build().Run();
