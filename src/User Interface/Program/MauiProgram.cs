using BibTeXManager.ViewModels;
using BibTeXManager.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Maui.Storage;
using DigitalProduction.Maui;
using DigitalProduction.Maui.Services;
using DigitalProduction.Maui.Storage;
using DigitalProduction.Maui.UI;

namespace BibTeXManager;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		MauiAppBuilder builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseDigitalProductionMauiAppToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		LifecycleOptions lifecycleOptions = new()
		{
			EnsureOnScreen          = false,
			DisableMaximizeButton   = false,
			WindowTitle             = "BibTeX Manager"
		};
		DigitalProduction.Maui.UI.LifecycleEventsInstaller.ConfigureLifecycleEvents(builder, lifecycleOptions);

		RegisterViewsAndViewModels(builder.Services);
		RegisterEssentials(builder.Services);
		CreateServices(builder.Services);
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

		services.AddTransientPopup<ProgramOptionsView, ProgramOptionsViewModel>();
	}

	private static void CreateServices(IServiceCollection services)
	{
		services.AddSingleton<IBibTeXFilePicker, BibTeXFilePicker>();
		services.AddSingleton<IDialogService, DialogService>();
		services.AddSingleton<IRecentPathsManagerService, RecentPathsManagerService>();
		services.AddSingleton<ISaveFilePicker, SaveFilePicker>();
	}

	static void RegisterEssentials(in IServiceCollection services)
	{
		services.AddSingleton<IFileSaver>(FileSaver.Default);
		services.AddSingleton<IFolderPicker>(FolderPicker.Default);
		services.AddSingleton<ISpeechToText>(SpeechToText.Default);
		services.AddSingleton<ITextToSpeech>(TextToSpeech.Default);
	}
}