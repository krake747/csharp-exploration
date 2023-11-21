using Bikes.Grpc.Products;
using Bikes.Grpc.Services;
using ProductsService = Bikes.Grpc.Services.ProductsService;

namespace Bikes.Grpc.Tests.Unit;

public sealed class ProductsServiceTests
{
    private readonly IProductsService _sut = new ProductsService();

    [Fact]
    public async Task Unary_ShouldReturnResponse()
    {
        //Arrange
        var callContext = TestServerCallContext.Create();
        var request = new Request { Content = "Trek" };
        var response = new Response { Message = "We got Trek from products server" };

        //Act
        var result = await _sut.Unary(request, callContext);

        //Assert
        result.Should().BeEquivalentTo(response);
    }
}