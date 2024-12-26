using BibTexManager.ViewModels;
using BibTexManager.Views;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Maui.Storage;
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

	static void RegisterViewsAndViewModels(IServiceCollection services)
	{
		services.AddSingleton<MainViewModel>();
		services.AddSingleton<MainPage>();

		services.AddTransient<EditRawBibEntryForm>();
		services.AddTransient<BibEntryViewModel>();

		//services.AddTransientPopup<ConfigurationView, ConfigurationViewModel>();
	}

	static void RegisterEssentials(in IServiceCollection services)
	{
		services.AddSingleton<IFileSaver>(FileSaver.Default);
		services.AddSingleton<IFolderPicker>(FolderPicker.Default);
		services.AddSingleton<ISpeechToText>(SpeechToText.Default);
		services.AddSingleton<ITextToSpeech>(TextToSpeech.Default);
	}
}