using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.LifecycleEvents;

namespace DashLain;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new MainPage())
        {
            Title = "DashLain"
        };

        // Handle app activation (like deep linking)
        // Note: In .NET MAUI, deep linking is handled through AppActions or AppDelegate/Activity
        
        return window;
    }

    protected override void OnStart()
    {
        base.OnStart();
        // Perform any app startup logic here
        Console.WriteLine("App started");
    }

    protected override void OnSleep()
    {
        base.OnSleep();
        // Handle when your app sleeps
        Console.WriteLine("App sleeping");
    }

    protected override void OnResume()
    {
        base.OnResume();
        // Handle when your app resumes
        Console.WriteLine("App resuming");
    }
}