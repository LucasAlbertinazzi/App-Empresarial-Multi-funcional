using Microsoft.Extensions.Logging;
#if __ANDROID__
using Plugin.Maui.Audio;
#endif

namespace AppEmpresa;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Roboto-Medium.ttf", "PadraoRoboto");
                fonts.AddFont("fontello.ttf", "IconsFont");
            });

#if __ANDROID__
        builder.Services.AddSingleton(AudioManager.Current);
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
