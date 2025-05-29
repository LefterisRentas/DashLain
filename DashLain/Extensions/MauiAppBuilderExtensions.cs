using DashLain.Services;
using DashLain.Validation;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using DashLain.Data;

namespace DashLain.Extensions;

public static class MauiAppBuilderExtensions {
    public static MauiAppBuilder AddConfigurationDefaults(this MauiAppBuilder builder)
    {
        var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly()!.Location)!;
        var configBuilder = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(workingDirectory, "dashlain.json"))
            .Build();
        builder.Configuration.AddConfiguration(configBuilder);
        return builder;
    }

    public static MauiAppBuilder AddSqliteDefaults(this MauiAppBuilder builder)
    {
        var dbConnectionString = builder.Configuration.GetConnectionString("DashLainDb");
        builder.Services.AddDbContext<AppDbContext>(x =>
        {
            x.UseSqlite(dbConnectionString);
        });
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(dbConnectionString)
            .Options;
        using var db = new AppDbContext(dbOptions);
        db.Database.EnsureCreated();
        return builder;
    }

    public static MauiAppBuilder AddCryptographyServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<Cryptographer>();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        builder.Services.AddSingleton<MasterPasswordService>();
        builder.Services.AddSingleton<ProfileService>();
        builder.Services.AddSingleton<VaultService>();

        return builder;
    }
}
