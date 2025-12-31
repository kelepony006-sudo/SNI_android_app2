using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SkullNetworkItalia
{
    [Activity(Theme = "@style/Maui.SplashTheme",
              MainLauncher = true,
              ConfigurationChanges = ConfigChanges.ScreenSize |
                                   ConfigChanges.Orientation |
                                   ConfigChanges.UiMode |
                                   ConfigChanges.ScreenLayout |
                                   ConfigChanges.SmallestScreenSize |
                                   ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Check for updates on app start
            Task.Run(async () =>
            {
                await Task.Delay(3000); // Wait for app to initialize
                var updateService = new AutoUpdateService();
                await updateService.CheckForUpdates();
            });
        }
    }
}