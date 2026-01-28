using DailyJournal.Services;
using DailyJournal.Services.Interfaces;
using Microsoft.Extensions.Logging;
using QuestPDF.Infrastructure; 
using SQLitePCL;
using DailyJournal.Data;

namespace DailyJournal
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            //  Initialize SQLite
            Batteries_V2.Init();

            // Set PDF License (Fixes "LicenseType" error)
            QuestPDF.Settings.License = LicenseType.Community;

            //  Register Services
            builder.Services.AddSingleton<IJournalService, JournalService>();
            builder.Services.AddSingleton<PdfExportService>();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();

            builder.Services.AddSingleton<JournalDatabase>(s =>
            {
                var dbPath = Path.Combine(
                    FileSystem.AppDataDirectory,
                    "journal.db"
                );
                return new JournalDatabase(dbPath);
            });

        }
    }
}