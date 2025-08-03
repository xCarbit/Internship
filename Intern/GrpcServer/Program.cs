using MyGrpcService.Services;
using MyGrpcService;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5007, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});


builder.WebHost.UseUrls("http://localhost:5007");


builder.Services.AddGrpc();


builder.Services.AddSingleton<MongoService>();

var app = builder.Build();

app.MapGrpcService<GreeterService>(); 
app.MapGet("/", () => "gRPC server je aktivan na http://localhost:5007");

app.Run();