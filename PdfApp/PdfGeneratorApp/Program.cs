using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

QuestPDF.Settings.License = LicenseType.Community;

var doc = Document.Create(container => container.Page(page =>
{
    page.Size(PageSizes.A4.Landscape());
    page.Margin(2, Unit.Centimetre);
    page.DefaultTextStyle(x => x.FontSize(12));

    page.Header()
        .Text("Hello from PDF Generator")
        .SemiBold()
        .FontSize(24);
    
    page.Content()
        .Column(x => x.Item().Text(Placeholders.Paragraph()));
}));

doc.ShowInPreviewer();

// doc.GeneratePdf("simple.pdf");
