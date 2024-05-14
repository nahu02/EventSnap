using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using CommunityToolkit.Mvvm.Messaging;

namespace EventSnap;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTask,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)
    ]
[IntentFilter(new[] { Intent.ActionSend, Intent.ActionView },
    Categories = new[]
    {
        Intent.CategoryDefault,
    },
    DataMimeType = "text/plain")]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }

    protected override void OnNewIntent(Intent intent)
    {
        ArgumentNullException.ThrowIfNull(intent);

        base.OnNewIntent(intent);
        HandleIntent(intent);
    }

    private void HandleIntent(Intent intent)
    {
        if (intent.Action == Intent.ActionSend && "text/plain".Equals(intent.Type))
        {
            var sharedText = intent.GetStringExtra(Intent.ExtraText);

            WeakReferenceMessenger.Default.Send(sharedText);
        }
    }
}
