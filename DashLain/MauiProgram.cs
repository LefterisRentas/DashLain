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

        // testing mediatR
        var serviceProvider = builder.Services.BuildServiceProvider().CreateScope().ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var vaultService = serviceProvider.GetRequiredService<VaultService>();

        var command = new AddVaultEntryCommand
        {
            Email = "makis@gmail.com",
            Title = "Work",
            Password = "test",
            Username = "makis"
        };
        await vaultService.AddEntry(command);
#endif
        return builder.Build();
    }
}