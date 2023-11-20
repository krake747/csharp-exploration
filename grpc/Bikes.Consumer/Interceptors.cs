using Grpc.Core.Interceptors;
using Serilog;

namespace Bikes.Consumer;

public static class Interceptors
{
    public sealed class LoggerInterceptor(ILogger logger) : Interceptor
    {
        public override TResponse BlockingUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            logger.Information("Consumer intercepting the call type of: {MethodName}, {MethodType}",
                context.Method.FullName, context.Method.Type);
            return continuation(request, context);
        }
    }
}