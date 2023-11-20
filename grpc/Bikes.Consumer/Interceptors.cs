using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Bikes.Consumer;

public static class Interceptors
{
    public sealed class LoggerInterceptor : Interceptor
    {
        private readonly ILogger _logger;
        
        public LoggerInterceptor(ILogger logger)
        {
            _logger = logger;
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(
            TRequest request, 
            ClientInterceptorContext<TRequest, TResponse> context, 
            BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            _logger.Information("Intercepting the call type of: {MethodName}, {MethodType}", context.Method.FullName, context.Method.Type);
            return continuation(request, context);
        }
    }
}