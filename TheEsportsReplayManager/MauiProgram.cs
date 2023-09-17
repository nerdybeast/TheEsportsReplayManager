using Microsoft.Extensions.Logging;
using TheEsportsReplayManager.Data;
using TheEsportsReplayManager.Pages;
using TheEsportsReplayManager.Repositories;
using TheEsportsReplayManager.Services;
using Microsoft.AspNetCore.Components.Authorization;
using TheEsportsReplayManager.Auth;

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

            builder.Services.AddSingleton(new Auth0Client(new Auth0ClientOptions
            {
                Domain = "sith-oath.auth0.com",
                ClientId = "UGNoLg04JZecHilz1zmuLp0MGJpHne3J",
                Scope = "openid profile email",
                RedirectUri = "myapp://callback"
            }));

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, Auth0AuthenticationStateProvider>();

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