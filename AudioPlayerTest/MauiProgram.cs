using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using NLog;
using NLog.Extensions.Logging;
using Plugin.Maui.Audio;

namespace AudioPlayerTest;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        NLog.LogManager.Setup().RegisterMauiLog().LoadConfigurationFromAssemblyResource(typeof(App).Assembly);

        var builder = MauiApp.CreateBuilder();

        // Add NLog for Logging
        builder.Logging.ClearProviders();
        builder.Logging.AddNLog();

        builder
            .UseMauiApp<App>()
			.UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
	}
}

