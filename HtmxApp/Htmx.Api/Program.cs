using Htmx.Api.Domain.Bikes;
using Htmx.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    // lang=html
    const string html =
        """
        <html lang="en">

        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>
                Htmx WebApp + Api
            </title>
        </head>

        <body>

        </body>
        <h1>Hello Htmx!</h1>
        <p>This is .NET Minimal Api and Htmx in action.</p>
        </html>
        """;

    return Results.Extensions.Html(string.Join(Environment.NewLine, "<!DOCTYPE html>", html));
});

app.MapGet("/bike-brands", () =>
{
    var bikeBrands = new[] { new BikeBrand(1, "Giant"), new BikeBrand(2, "Orbea"), new BikeBrand(3, "Trek") };

    // lang=html
    var html =
        $"""
         <table>
             <thead>
                 <tr>
                     <th>BikeBrandId</th>
                     <th>Name</th>
                 </tr>
                 {string.Join(Environment.NewLine, bikeBrands.Select(BikeBrandTableRow))}
             </thead>
         </table>
         """;

    return Results.Extensions.Html(html);

    static string BikeBrandTableRow(BikeBrand bikeBrand) =>
        $"""
         <tr>
             <td>{bikeBrand.Id.ToString()}</td>
             <td>{bikeBrand.Name}</td>
         </tr>
         """;
});

app.Run();