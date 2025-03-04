using BibTeXLibrary;
using BibTexManager.ViewModels;
using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;
using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Maui.Views;
using DigitalProduction.Projects;
using Google.Apis.CustomSearchAPI.v1.Data;

namespace BibTexManager.Views;

public partial class MainPage : DigitalProductionMainPage
{
	#region Fields

	private MainViewModel				_viewModel;

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

	#region Construction

	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

		_viewModel = viewModel;

		if (Preferences.LoadLastProjectAtStartUp)
		{
			OpenLastProject();
		}
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
			_viewModel.OpenProjectWithPathSave(result.FullPath);
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

	void OnClose(object sender, EventArgs eventArgs)
	{
		_viewModel.CloseProject();
	}

	async void OnShowProgramOptions(object sender, EventArgs eventArgs)
	{
		ProgramOptionsViewModel viewModel = new();

		ProgramOptionsView	view	= new(viewModel);
		object?				result	= await Shell.Current.ShowPopupAsync(view);

		//if (result is bool boolResult && boolResult)
		//{
		//}
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

	#endregion

	#region Button Events

	async void OnNew(object sender, EventArgs eventArgs)
	{
		//TranslationMatrix translationMatrix = TranslationMatrix.CreateNewTranslationMatrix(TranslationMatrixNewName, InputProcessor, InputFile);

		await Shell.Current.GoToAsync(nameof(EditRawBibEntryForm), true, new Dictionary<string, object>
		{
			{"AddMode",  true},
		});


		//ConfigurationsViewModel? configurationsViewModel = BindingContext as ConfigurationsViewModel;
		//System.Diagnostics.Debug.Assert(configurationsViewModel != null);

		//ConfigurationViewModel	viewModel	= new(Interface.ConfigurationList?.ConfigurationNames ?? []);
		//ConfigurationView		view		= new(viewModel);
		//object?					result		= await Shell.Current.ShowPopupAsync(view);

		//if (result is bool boolResult && boolResult)
		//{
		//	configurationsViewModel?.Insert(viewModel.Configuration);
		//}
	}

	async void OnNewFromTemplate(object sender, EventArgs eventArgs)
	{
		TemplateSelectionViewModel	viewModel	= new(_viewModel.Project.BibEntryInitialization.TemplateNames);
		TemplateSelectionView		view		= new(viewModel);
		object?						result		= await Shell.Current.ShowPopupAsync(view);
		if (result is bool boolResult && boolResult)
		{
			BibEntry entry = BibEntry.NewBibEntryTemplate(_viewModel.Project.BibEntryInitialization, viewModel.Template);
	
			await Shell.Current.GoToAsync(nameof(EditRawBibEntryForm), true, new Dictionary<string, object>
			{
				{"AddMode",  true},
				{"BibEntry", entry}
			});

			//ConfigurationsViewModel? configurationsViewModel = BindingContext as ConfigurationsViewModel;
			//System.Diagnostics.Debug.Assert(configurationsViewModel != null);

			//ConfigurationViewModel	viewModel	= new(Interface.ConfigurationList?.ConfigurationNames ?? []);
			//ConfigurationView		view		= new(viewModel);
			//object?					result		= await Shell.Current.ShowPopupAsync(view);

			//if (result is bool boolResult && boolResult)
			//{
			//	configurationsViewModel?.Insert(viewModel.Configuration);
			//}
		}

	}


	async void OnEdit(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync(nameof(EditRawBibEntryForm), true, new Dictionary<string, object>
		{
			{"AddMode",  false},
			{"BibEntry", _viewModel.SelectedItem!}
		});

		//ConfigurationsViewModel? configurationsViewModel = BindingContext as ConfigurationsViewModel;
		//System.Diagnostics.Debug.Assert(configurationsViewModel != null);

		//ConfigurationViewModel	viewModel	= new(Interface.ConfigurationList?.ConfigurationNames ?? []);
		//ConfigurationView		view		= new(viewModel);
		//object?					result		= await Shell.Current.ShowPopupAsync(view);

		//if (result is bool boolResult && boolResult)
		//{
		//	configurationsViewModel?.Insert(viewModel.Configuration);
		//}
	}

	async void OnDelete(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			_viewModel.Delete();
		}
	}

	#endregion

	#region Methods

	private void OpenLastProject()
	{
		//_viewModel.OpenProjectWithPathSave(Preferences.RecentPathsManagerService.TopPath);
		List<string> paths = Preferences.RecentPathsManagerService.GetRecentPaths();
		if (paths.Count > 0)
		{
			_viewModel.OpenProject(paths[0]);
		}
	}

	#endregion
}