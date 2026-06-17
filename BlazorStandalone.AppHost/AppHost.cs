var builder = DistributedApplication.CreateBuilder(args);

var weatherApi = builder.AddProject<Projects.BlazorStandalone_WeatherApi>("weatherapi");

// Register the standalone Blazor WASM app as a resource.
// The resource name becomes the URL path prefix (e.g., "app" -> served at /app/).
var blazorApp = builder.AddBlazorWasmProject<Projects.BlazorStandalone>("app")
    .WithReference(weatherApi);

// The Blazor Gateway serves WASM static files and proxies API/OTLP traffic.
var gateway = builder.AddBlazorGateway("gateway")
    .WithExternalHttpEndpoints()
    .WithOtlpExporter(OtlpProtocol.HttpProtobuf)
    .WithBlazorClientApp(blazorApp);

builder.Build().Run();
