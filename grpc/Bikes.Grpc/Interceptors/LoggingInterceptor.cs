using Grpc.Core;
using Grpc.Core.Interceptors;
using ILogger = Serilog.ILogger;

namespace Bikes.Grpc.Interceptors;

public sealed class LoggingInterceptor(ILogger logger) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            logger.Information("Server intercepting the call type of: {Method}, {Status}", context.Method, context.Status);
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error thrown by {Method}", context.Method);
            throw;
        }
    }
}