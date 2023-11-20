using Bikes.Consumer;
using Bikes.Grpc.Products;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static Bikes.Consumer.Interceptors;
using static Bikes.Grpc.Products.ProductsService;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var services = new ServiceCollection();

services.AddSingleton(Log.Logger);
services.AddTransient<LoggerInterceptor>();
services.AddGrpcClient<ProductsServiceClient>("Products", o => o.Address = new Uri("https://localhost:7016"))
    .AddInterceptor<LoggerInterceptor>();
services.AddSingleton<ProductsRepository>();

var serviceProvider = services.BuildServiceProvider();

Log.Information("Hello, Bikes!");

// using var channel = GrpcChannel.ForAddress("https://localhost:7016", new GrpcChannelOptions());
// var client = new ProductsServiceClient(channel);

var repo = serviceProvider.GetRequiredService<ProductsRepository>();

Log.Information("Grpc - Unary Call");
repo.Unary(new Request { Content = "Trek Top Fuel SL 9.8" });

Log.Information("Grpc - Client Streaming");
await repo.ClientStreaming();

Log.Information("Grpc - Server Streaming");
await repo.ServerStreaming();

Log.Information("Grpc - Bi Directional Streaming");
await repo.BiDirectionalStreaming();


