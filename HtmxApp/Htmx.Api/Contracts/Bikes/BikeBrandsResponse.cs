using Htmx.Api.Domain.Bikes;

namespace Htmx.Api.Contracts.Bikes;

public sealed class BikeBrandsResponse
{
    public IEnumerable<BikeBrand> Items { get; init; } = Enumerable.Empty<BikeBrand>();
}