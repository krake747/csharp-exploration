using Grpc.Core;

namespace Bikes.Grpc.Services;

public sealed class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) =>
        Task.FromResult(new HelloReply
        {
            Message = $"Hello {request.Name}"
        });
}