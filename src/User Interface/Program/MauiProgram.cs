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
	public static IServiceProvider Services { get; private set; } = null!;

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
			WindowTitle             = "BibTeX Manager",
			PromptToSaveBeforeClose	= true
		};
		DigitalProduction.Maui.UI.LifecycleEventsInstaller.ConfigureLifecycleEvents(builder, lifecycleOptions);

		RegisterViewsAndViewModels(builder.Services);
		RegisterServices(builder.Services);
		RegisterEssentials(builder.Services);
		#if DEBUG
			builder.Logging.AddDebug();
		#endif

		BibTeXProject.New(Preferences.ProjectSettings);

		MauiApp mauiApp	= builder.Build();
		Services		= mauiApp.Services;
		return mauiApp;
	}

	static void RegisterViewsAndViewModels(IServiceCollection services)
	{
		services.AddSingleton<MainView>();
		services.AddSingleton<MainViewModel>();

		services.AddSingleton<HeaderView>();
		services.AddSingleton<HeaderViewModel>();

		services.AddSingleton<StringsEditView>();
		services.AddSingleton<StringsEditViewModel>();

		services.AddSingleton<BibliographyEditView>();
		services.AddSingleton<BibliographyEditViewModel>();

		services.AddTransient<EditRawBibEntryForm>();
		services.AddTransient<BibEntryViewModel>();

		services.AddTransient<SettingsView>();
		services.AddTransient<SettingsViewModel>();

		services.AddTransient<NameMappingView>();
		services.AddTransient<NameMappingViewModel>();

		services.AddTransient<TemplatesEditView>();
		services.AddTransient<TemplatesEditViewModel>();
	}

	private static void RegisterServices(IServiceCollection services)
	{
		services.AddSingleton<IBibTeXFilePicker, BibTeXFilePicker>();
		services.AddSingleton<IDialogService, DialogService>();
		services.AddSingleton<IRecentPathsManagerService, RecentPathsManagerService>();
		services.AddSingleton<ISaveFilePicker, SaveFilePicker>();
		services.AddSingleton<ISaveService, SaveService>();
		services.AddSingleton<IPageProvider, CurrentPageProvider>();
	}

	static void RegisterEssentials(in IServiceCollection services)
	{
		services.AddSingleton<IFileSaver>(FileSaver.Default);
		services.AddSingleton<IFolderPicker>(FolderPicker.Default);
		services.AddSingleton<ISpeechToText>(SpeechToText.Default);
		services.AddSingleton<ITextToSpeech>(TextToSpeech.Default);
	}
}