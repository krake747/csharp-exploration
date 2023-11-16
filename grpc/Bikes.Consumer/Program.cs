using Bikes.Grpc.Products;
using Grpc.Net.Client;
using Serilog;
using static Bikes.Grpc.Products.ProductsService;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Console.WriteLine("Hello, Bikes!");

using var channel = GrpcChannel.ForAddress("https://localhost:7016", new GrpcChannelOptions());
var client = new ProductsServiceClient(channel);

var response = client.Unary(new Request { Content = "Trek Top Fuel SL 9.8" });
Log.Information("Unary: {Response}", response.Message);