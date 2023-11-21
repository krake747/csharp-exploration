using Bikes.Grpc.Products;
using Grpc.Core;

namespace Bikes.Grpc.Services;

public interface IProductsService
{
    Task<Response> Unary(Request request, ServerCallContext context);

    Task<Response> ClientStream(IAsyncStreamReader<Request> requestStream, ServerCallContext context);

    Task ServerStream(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context);

    Task BiDirectionalStream(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Response> responseStream, 
        ServerCallContext context);
}