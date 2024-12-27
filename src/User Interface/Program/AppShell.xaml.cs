using BibTexManager.ViewModels;
using BibTexManager.Views;
using CommunityToolkit.Maui.Views;
using DigitalProduction.ViewModels;
using DigitalProduction.Views;

namespace BibTexManager;

public partial class AppShell : Shell
{
	#region Fields

	private readonly FilePickerFileType _bibliographyProjectFileType = new(new Dictionary<DevicePlatform, IEnumerable<string>>
	{
		{
			DevicePlatform.WinUI, new[]
			{
				".bibproj"
			}
		},
	});

	#endregion

	#region Contruction

	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(EditRawBibEntryForm), typeof(EditRawBibEntryForm));
	}

	#endregion

	#region Menu Events

	async void OnOpen(object sender, EventArgs eventArgs)
	{
		PickOptions pickOptions = new()
		{
			PickerTitle = "Select an Bibliography Project File",
			FileTypes   = _bibliographyProjectFileType
		};
		FileResult? result = await BrowseForFile(pickOptions);
		if (result != null)
		{
			MainViewModel? viewModel = BindingContext as MainViewModel;
			viewModel?.OpenProject(result.FullPath);
		}
	}

	public static async Task<FileResult?> BrowseForFile(PickOptions options)
	{
		try
		{
			return await FilePicker.PickAsync(options);
		}
		catch
		{
			//(Exception exception)
			//string message = exception.Message;
			// The user canceled or something went wrong.
		}
		return null;
	}

	async void OnShowProgramOptions(object sender, EventArgs eventArgs)
	{
		ProgramOptionsViewModel viewModel = new();

		ProgramOptionsView	view	= new(viewModel);
		object?				result	= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
		}
	}

	async void OnHelp(object sender, EventArgs eventArgs)
	{
		System.Reflection.Assembly? entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
		System.Diagnostics.Debug.Assert(entryAssembly != null);
		string url = DigitalProduction.Reflection.Assembly.DocumentationAddress(entryAssembly);
		await Launcher.OpenAsync(url);
	}

	async void OnAbout(object sender, EventArgs eventArgs)
	{
		AboutView1 view = new(new AboutViewModel(true));
		_ = await Shell.Current.ShowPopupAsync(view);
	}

	/// <summary>
	/// Exit command.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="eventArgs">Event arguments.</param>
	protected void OnExit(object sender, EventArgs eventArgs)
	{
		Application.Current?.Quit();
	}

	#endregion
}