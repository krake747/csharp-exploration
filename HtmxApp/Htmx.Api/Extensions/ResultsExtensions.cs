using System.Net.Mime;
using System.Text;

namespace Htmx.Api.Extensions;

public static class ResultsExtensions
{
    public static IResult Html(this IResultExtensions extensions, string html) => new HtmlResult(html);

    private sealed class HtmlResult(string html) : IResult
    {
        public Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = MediaTypeNames.Text.Html;
            httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(html);
            return httpContext.Response.WriteAsync(html);
        }
    }
}