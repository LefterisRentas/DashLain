using System.Globalization;
using DashLain.Extensions;
using DashLain.Handlers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DashLain;

public static class MauiProgram {
    public static async Task<MauiApp> CreateMauiApp()
    {
#pragma warning disable CA1416
        var builder = MauiApp.CreateBuilder();

        builder.AddConfigurationDefaults();
        builder.AddSqliteDefaults();
        builder.AddCryptographyServices();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddLocalization();
        var lang = Preferences.Get("lang", "en");
        var culture = new CultureInfo(lang); // change this to "en", "es", etc.
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
        await builder.Debugging();
#endif
        return builder.Build();
#pragma warning restore CA1416
    }
}
