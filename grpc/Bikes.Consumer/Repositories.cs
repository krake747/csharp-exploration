using Bikes.Grpc.Products;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Serilog;
using static Bikes.Grpc.Products.ProductsService;

namespace Bikes.Consumer;

public sealed class ProductsRepository(ILogger logger, GrpcClientFactory factory)
{
    private readonly ProductsServiceClient client = factory.CreateClient<ProductsServiceClient>("Products");
        
    public void Unary(Request request, DateTime? deadline = null)
    {
        var metaData = new Metadata { { "grpc-accept-encoding", "gzip" } };
        var response = client.Unary(request, metaData, deadline);
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
        using var serverStream = client.ServerStream(new Request { Content = string.Empty }, cancellationToken: token);
        await foreach (var response in serverStream.ResponseStream.ReadAllAsync(token))
        {
            logger.Information("{Message}", response.Message);
        } 
    }

    public async Task BiDirectionalStreaming(DateTime? deadline = null, CancellationToken token = default)
    {
        using var biDirectionalStream = client.BiDirectionalStream(cancellationToken: token);
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