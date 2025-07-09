using MyGrpcService;// Obavezno: tvoj namespace za GreeterService
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});

// Eksplicitno postavi HTTP/2 na portu 5007
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5007, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

// Postavi URL (nije potreban launchSettings.json)
builder.WebHost.UseUrls("http://localhost:5007");

// Registruj gRPC servise
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GreeterService>(); // Tvoj konkretni servis
app.MapGet("/", () => "gRPC server je aktivan na http://localhost:5007");

app.Run();