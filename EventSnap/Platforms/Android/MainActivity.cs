﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

namespace EventSnap;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTask,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)
    ]
[IntentFilter(new[] { Intent.ActionSend },
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
        HandleIntent(Intent);
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

            // TODO: Handle shared text
            Toast.MakeText(this, $"Shared Text: {sharedText}", ToastLength.Long).Show();
        }
    }
}
