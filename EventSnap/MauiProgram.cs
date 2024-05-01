using DevExpress.Maui;
using EventSnap.Services;
using Microsoft.Extensions.Logging;

namespace EventSnap;

public static class MauiProgram
{
    public static IServiceProvider Services { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseDevExpress()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .Services
                .AddSingleton<SettingsService>();

        Services = builder.Services.BuildServiceProvider();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
