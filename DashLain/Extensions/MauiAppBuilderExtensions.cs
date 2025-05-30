using DashLain.Services;
using DashLain.Entities;
using DashLain.Handlers;
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
        var workingDirectory = Path.GetDirectoryName(AppContext.BaseDirectory)!;
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

    public static async Task<MauiAppBuilder> Debugging(this MauiAppBuilder builder)
    {
        var serviceProvider = builder.Services.BuildServiceProvider().CreateScope().ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var vaultService = serviceProvider.GetRequiredService<VaultService>();
        var profileService = serviceProvider.GetRequiredService<ProfileService>();

        var loginCommand = new LoginMasterPasswordCommand
        {
            Name = "Work",
            Password = "test",
        };
        await profileService.LoginWithMasterPassword(loginCommand);

        var addEntryCommand = new AddVaultEntryCommand
        {
            Email = "makis@gmail.com",
            Title = "Work",
            Password = "test",
            Username = "makis"
        };
        var x = await vaultService.AddEntry(addEntryCommand);
        return builder;
    }
}
