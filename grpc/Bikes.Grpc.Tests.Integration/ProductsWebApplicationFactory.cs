using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using static Bikes.Grpc.Products.ProductsService;

namespace Bikes.Grpc.Tests.Integration;

public sealed class ProductsWebApplicationFactory : WebApplicationFactory<IGrpcMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        builder.UseTestServer();
    }

    public ProductsServiceClient CreateGrpcClient()
    {
        var httpClient = CreateClient();
        var channel = GrpcChannel.ForAddress(httpClient.BaseAddress!, new GrpcChannelOptions
        {
            HttpClient = httpClient
        });

        return new ProductsServiceClient(channel);
    }
}