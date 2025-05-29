using DashLain.Extensions;
using DashLain.Handlers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DashLain;

public static class MauiProgram {
    public static MauiApp CreateMauiApp()
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
        var x = builder.Services.BuildServiceProvider().CreateScope().ServiceProvider;
        var mediator = x.GetRequiredService<IMediator>();
        var c = new CreateProfileMasterPasswordCommand { Name = "", Password = "1" };
        var r = mediator.Send(c).Result;
#endif
        return builder.Build();
    }
}
