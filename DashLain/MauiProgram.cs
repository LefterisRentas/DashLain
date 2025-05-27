using DashLain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DashLain;

public static class MauiProgram {
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        var workingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        var configBuilder = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(workingDirectory, "dashlain.json"))
            .Build();
        builder.Configuration.AddConfiguration(configBuilder);

        var dbConnectionString = builder.Configuration.GetConnectionString("DashLainDb");
        builder.Services.AddDbContext<AppDbContext>(x =>
        {
            x.UseSqlite(dbConnectionString);
        });

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

        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(dbConnectionString)
            .Options;
        using var db = new AppDbContext(dbOptions);
        db.Database.EnsureCreated();
#endif

        return builder.Build();
    }
}