using Microsoft.Extensions.Logging;

namespace FirstAppTest
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            #if ANDROID
            builder.ConfigureMauiHandlers(handlers =>
            {
                // CHANGE: Map your custom handler directly to the HybridWebView control type
                handlers.AddHandler<Microsoft.Maui.Controls.HybridWebView,
                    FirstAppTest.CustomWebViewHandler>();
            });
            #endif

            #if DEBUG
            builder.Logging.AddDebug();
            #endif

            return builder.Build();
        }
    }
}
