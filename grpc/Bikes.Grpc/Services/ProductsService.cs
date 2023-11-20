using Bikes.Grpc.Products;
using Grpc.Core;
using static Bikes.Grpc.Products.ProductsService;
using ILogger = Serilog.ILogger;

namespace Bikes.Grpc.Services;

public sealed class ProductsService : ProductsServiceBase
{
    private readonly ILogger _logger;

    public ProductsService(ILogger logger)
    {
        _logger = logger;
    }

    public override Task<Response> Unary(Request request, ServerCallContext context)
    {
        context.WriteOptions = new WriteOptions(WriteFlags.NoCompress);
        var response = new Response
        {
            Message = $"We got {request.Content} from products server"
        };

        return Task.FromResult(response);
    }

    public override async Task<Response> ClientStream(IAsyncStreamReader<Request> requestStream,
        ServerCallContext context)
    {
        var response = new Response { Message = string.Empty };
        while (await requestStream.MoveNext())
        {
            var requestPayLoad = requestStream.Current;
            _logger.Information("{RequestPayLoad}", requestPayLoad);
            response.Message = $"Got Id {requestPayLoad}";
        }

        return response;
    }

    public override async Task ServerStream(Request request, IServerStreamWriter<Response> responseStream,
        ServerCallContext context)
    {
        for (var id = 0; id < 100; id++)
        {
            await responseStream.WriteAsync(new Response { Message = $"ProductId = {id}" });
        }
    }

    public override async Task BiDirectionalStream(IAsyncStreamReader<Request> requestStream,
        IServerStreamWriter<Response> responseStream, ServerCallContext context)
    {
        var response = new Response { Message = string.Empty };
        while (await requestStream.MoveNext())
        {
            var requestPayload = requestStream.Current;
            response.Message = requestPayload.ToString();
            await responseStream.WriteAsync(response);
        }
    }
}