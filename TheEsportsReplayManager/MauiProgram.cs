//using EsportsRocketLeagueReplayParser.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using TheEsportsReplayManager.Data;
using TheEsportsReplayManager.Pages;
using TheEsportsReplayManager.Repositories;
using TheEsportsReplayManager.Services;

namespace TheEsportsReplayManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            //builder.Services.AddRocketLeagueReplayParser();

            builder.Services.AddLogging((ILoggingBuilder builder) =>
            {
                //See: https://github.com/serilog/serilog-extensions-logging
                //Couldn't use "Serilog.AspNetCore" or "Serilog.Extensions.Hosting" as suggested, trying to install
                //either of those throws some errors in this project because they are not Mac or Android compatable
                builder.AddSerilog();
            });

            //Serilog will create the logs directory if it doesn't exist
            string logFile = Path.Combine(FileSystem.Current.AppDataDirectory, "Logs", "application.log");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(new CompactJsonFormatter(), logFile, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //Pages
            builder.Services.AddTransient<Counter>();
            builder.Services.AddTransient<Dashboard>();

            builder.Services.AddSingleton<ILocalFileSystemService, LocalFileSystemService>();
            builder.Services.AddSingleton<ILocalReplayRepository, LocalReplayRepository>();
            builder.Services.AddSingleton<IReplayManagementService, ReplayManagementService>();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug(); //Adds logging for things like Debug.WriteLine()
#endif

            builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
}