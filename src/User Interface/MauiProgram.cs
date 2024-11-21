using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace BibTexManager;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		MauiAppBuilder builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		#if WINDOWS
			builder.ConfigureLifecycleEvents(lifecycle =>  
			{
				lifecycle.AddWindows((builder) =>  
				{  
					builder.OnWindowCreated(del =>  
					{  
						del.Title = "XSLT Processor";
					});  
				});  
			});
		#endif

		#if DEBUG
			builder.Logging.AddDebug();
		#endif

		return builder.Build();
	}
}
