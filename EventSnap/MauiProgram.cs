using DevExpress.Maui;
using DevExpress.Maui.Core;
using EventSnap.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace EventSnap;

public static class MauiProgram
{
    public static IServiceProvider Services { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        ThemeManager.Theme = new Theme(Color.FromHex("#F6AB00"));

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseDevExpress()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureLifecycleEvents(lifecycle =>
            {
#if ANDROID
                lifecycle.AddAndroid(android =>
                {
                    android.OnCreate((activity, bundle) =>
                    {
                        var action = activity.Intent?.Action;

                        if (action == Android.Content.Intent.ActionSend)
                        {
                            var sharedText = activity.Intent.GetStringExtra(Android.Content.Intent.ExtraText);
                            Task.Run(() => HandleSharedText(sharedText));
                        }
                    });
                });
#endif
            })
            .Services
                .AddSingleton<SettingsService>()
                .AddSingleton<IcalCreatorService>()
                .AddSingleton<AiCommunicatorService>();

        Services = builder.Services.BuildServiceProvider();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static async void HandleSharedText(string data)
    {
        if (App.Current?.MainPage is AppShell shell)
        {
            await shell.GoToAsync($"//EventDetailsPage?sharedText={data}");
        }
        else
        {
            throw new InvalidOperationException("MainPage is not AppShell");
        }
    }
}
