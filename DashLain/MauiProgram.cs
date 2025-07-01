using System.Globalization;
using System.Security.Claims;
using DashLain.Extensions;
using DashLain.Handlers;
using DashLain.State;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;

namespace DashLain;

public static class MauiProgram
{
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

        // Add authentication services
        builder.Services.AddOptions();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        
        // Add HTTP client for API calls
        builder.Services.AddHttpClient();
        
        // Add localization
        builder.Services.AddLocalization();
        var lang = Preferences.Get("lang", "en");
        var culture = new CultureInfo(lang);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
        await builder.Debugging();
#endif

        return builder.Build();
    }
}

// Custom AuthenticationStateProvider that uses our SessionState
public class CustomAuthStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var isAuthenticated = SessionState.IsAuthenticated;
            
            if (isAuthenticated)
            {
                var profile = SessionState.Get();
                var identity = new ClaimsIdentity(
                    authenticationType: "CustomAuth",
                    nameType: ClaimTypes.Name,
                    roleType: ClaimTypes.Role);
                
                identity.AddClaim(new Claim(ClaimTypes.Name, profile.Name));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, profile.Id.ToString()));
                
                var user = new ClaimsPrincipal(identity);
                return Task.FromResult(new AuthenticationState(user));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAuthenticationStateAsync: {ex.Message}");
        }
        
        // Return empty claims principal if not authenticated
        var anonymous = new ClaimsIdentity();
        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
    }
    
    public void NotifyAuthenticationStateChanged()
    {
        var authState = GetAuthenticationStateAsync();
        base.NotifyAuthenticationStateChanged(authState);
    }
}
