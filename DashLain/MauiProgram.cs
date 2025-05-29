using DashLain.Entities;
using DashLain.Extensions;
using DashLain.Handlers;
using DashLain.Models;
using DashLain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DashLain;

public static class MauiProgram {
    public static async Task<MauiApp> CreateMauiApp()
    {
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

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
        await builder.Debugging();
#endif
        return builder.Build();
    }
}