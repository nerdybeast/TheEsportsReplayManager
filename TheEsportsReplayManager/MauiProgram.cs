﻿using Microsoft.Extensions.Logging;
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
		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
}