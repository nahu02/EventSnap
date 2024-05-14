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

    /// <summary>
    /// Handles the incoming intent if the application was already running when the intent was received.
    /// It should not be called if the application was started by the intent.
    /// 
    /// If the intent is an ACTION_SEND with a MIME type of "text/plain", the shared text is sent to the application using <see cref="WeakReferenceMessenger.Default"/>.
    /// </summary>
    /// <param name="intent">The incoming intent.</param>
    private void HandleIntent(Intent intent)
    {
        if (intent.Action == Intent.ActionSend && "text/plain".Equals(intent.Type))
        {
            var sharedText = intent.GetStringExtra(Intent.ExtraText);

            WeakReferenceMessenger.Default.Send(sharedText);
        }
    }
}
