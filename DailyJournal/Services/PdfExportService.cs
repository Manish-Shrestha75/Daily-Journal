using DailyJournal.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;


using QuestColors = QuestPDF.Helpers.Colors;
using QuestPDF.Helpers;

namespace DailyJournal.Services
{
    public class PdfExportService
    {
        public async Task<string> GeneratePdfAsync(List<JournalEntry> entries, DateTime start, DateTime end)
        {
            var fileName = $"Journal_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

            await Task.Run(() =>
            {
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        
                        page.Header()
                            .Text($"Journal Export ({start.ToShortDateString()} - {end.ToShortDateString()})")
                            .SemiBold().FontSize(20).FontColor(QuestColors.Blue.Medium);

                        page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                        {
                            foreach (var entry in entries)
                            {
                                
                                column.Item().BorderBottom(1).BorderColor(QuestColors.Grey.Lighten1).PaddingBottom(5).Column(c =>
                                {
                                    c.Item().Text($"{entry.EntryDate.ToLongDateString()}").Bold();
                                    c.Item().Text($"Mood: {entry.PrimaryMood}").Italic().FontSize(10);
                                    c.Item().Text(entry.Content ?? "");
                                });
                                column.Item().PaddingBottom(10);
                            }
                        });

                        page.Footer().AlignCenter().Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                    });
                });

                document.GeneratePdf(filePath);
            });

            return filePath;
        }
    }
}