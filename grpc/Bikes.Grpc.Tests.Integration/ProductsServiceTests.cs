using Bikes.Grpc.Products;

namespace Bikes.Grpc.Tests.Integration;

public sealed class ProductsServiceTests(ProductsWebApplicationFactory factory)
    : IClassFixture<ProductsWebApplicationFactory>
{
    [Fact]
    public void GetUnaryMessage()
    {
        //Arrange
        var client = factory.CreateGrpcClient();
        var request = new Request { Content = "Trek" };
        var response = new Response { Message = "We got Trek from products server" };

        //Act
        var result = client.Unary(request);

        //Assert
        result.Should().BeEquivalentTo(response);
    }
}