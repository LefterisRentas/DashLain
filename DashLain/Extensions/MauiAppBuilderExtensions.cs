using DashLain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DashLain.Extensions;

public static class MauiAppBuilderExtensions {
    public static MauiAppBuilder AddConfigurationDefaults(this MauiAppBuilder builder)
    {
        var workingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
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
}
