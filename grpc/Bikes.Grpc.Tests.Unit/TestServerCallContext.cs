using Grpc.Core;

namespace Bikes.Grpc.Tests.Unit;

public sealed class TestServerCallContext : ServerCallContext
{
    private readonly Dictionary<object, object> _userState;

    private TestServerCallContext(Metadata requestHeaders, CancellationToken cancellationToken)
    {
        RequestHeadersCore = requestHeaders;
        CancellationTokenCore = cancellationToken;
        ResponseTrailersCore = new Metadata();
        AuthContextCore = new AuthContext(string.Empty, new Dictionary<string, List<AuthProperty>>());
        _userState = new Dictionary<object, object>();
    }

    public Metadata? ResponseHeaders { get; private set; }

    protected override string MethodCore => "MethodName";
    protected override string HostCore => "HostName";
    protected override string PeerCore => "PeerName";
    protected override DateTime DeadlineCore => throw new NotImplementedException();
    protected override Metadata RequestHeadersCore { get; }
    protected override CancellationToken CancellationTokenCore { get; }
    protected override Metadata ResponseTrailersCore { get; }
    protected override Status StatusCore { get; set; }
    protected override WriteOptions? WriteOptionsCore { get; set; }
    protected override AuthContext AuthContextCore { get; }

    protected override IDictionary<object, object> UserStateCore => _userState;

    protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions? options) =>
        throw new NotImplementedException();

    protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
    {
        if (ResponseHeaders is not null)
        {
            throw new InvalidOperationException("Response headers have already been written.");
        }

        ResponseHeaders = responseHeaders;
        return Task.CompletedTask;
    }

    public static TestServerCallContext Create(Metadata? requestHeaders = null,
        CancellationToken cancellationToken = default) =>
        new(requestHeaders ?? new Metadata(), cancellationToken);
}