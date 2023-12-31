using System.IO.Compression;
using Bikes.Grpc.Interceptors;
using Bikes.Grpc.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);
builder.Services.AddSingleton(Log.Logger);
builder.Services.AddGrpc(o =>
{
    o.Interceptors.Add<LoggingInterceptor>();
    o.ResponseCompressionAlgorithm = "gzip";
    o.ResponseCompressionLevel = CompressionLevel.SmallestSize;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductsService>();
app.MapGet("/", () =>
    """
    Communication with gRPC endpoints must be made through a gRPC client.
    To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909")
    """);

app.Run();