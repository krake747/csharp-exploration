using Bikes.Grpc.Consumer.Repositories;
using Bikes.Grpc.Products;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static Bikes.Grpc.Products.ProductsService;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var services = new ServiceCollection();

services.AddSingleton(Log.Logger);
services.AddGrpcClient<ProductsServiceClient>("Products", o => o.Address = new Uri("https://localhost:7016"));
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


namespace Bikes.Grpc.Consumer.Repositories
{
    public sealed class ProductsRepository(ILogger logger, GrpcClientFactory factory)
    {
        private readonly ProductsServiceClient client = factory.CreateClient<ProductsServiceClient>("Products");
        
        public void Unary(Request request, DateTime? deadline = null)
        {
            var response = client.Unary(request, deadline: deadline);
            logger.Information("Unary: {Response}", response.Message);
        }

        public async Task ClientStreaming(CancellationToken token = default)
        {
            using var clientStream = client.ClientStream();
            for (var i = 0; i < 200; i++)
            {
                await clientStream.RequestStream.WriteAsync(new Request { Content = i.ToString() }, token);
            }

            await clientStream.RequestStream.CompleteAsync();
            var response = await clientStream;
            logger.Information("Unary: {Response}", response.Message);
        }

        public async Task ServerStreaming(CancellationToken token = default)
        {
            using var serverStream = client.ServerStream(new Request { Content = string.Empty });
            await foreach (var response in serverStream.ResponseStream.ReadAllAsync(token))
            {
                logger.Information("{Message}", response.Message);
            } 
        }

        public async Task BiDirectionalStreaming(DateTime? deadline = null, CancellationToken token = default)
        {
            using var biDirectionalStream = client.BiDirectionalStream();
            for (var i = 0; i < 10; i++)
            {
                var request = new Request { Content = i.ToString() };
                logger.Information("{Request}", request);
                await biDirectionalStream.RequestStream.WriteAsync(request, token);
            }

            while (await biDirectionalStream.ResponseStream.MoveNext())
            {
                var response = biDirectionalStream.ResponseStream.Current;
                logger.Information("{Response}", response);
            }
        }
    }
}