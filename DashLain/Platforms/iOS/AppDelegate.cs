using Foundation;

namespace DashLain;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate {
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp().Result;
}
