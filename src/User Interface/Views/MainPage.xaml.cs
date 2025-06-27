using BibTeXLibrary;
using BibtexManager;
using BibTexManager.ViewModels;
using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;
using DigitalProduction.Maui.Storage;
using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Maui.Views;
using DigitalProduction.Projects;
using Google.Apis.CustomSearchAPI.v1.Data;

namespace BibTexManager.Views;

[QueryProperty(nameof(NavigationCommand), "NavigationCommand")]
[QueryProperty(nameof(NavigationObject), "NavigationObject")]
public partial class MainPage : DigitalProductionMainPage
{
	#region Fields

	private MainViewModel		_viewModel;
	private IBibTexFilePicker	_filePicker			= DigitalProduction.Maui.Services.ServiceProvider.GetService<IBibTexFilePicker>();
	private ISaveFilePicker		_saveFilePicker		= DigitalProduction.Maui.Services.ServiceProvider.GetService<ISaveFilePicker>();

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

	#region Properties

	public string NavigationCommand { get; set; } = string.Empty;

	public BibEntry NavigationObject { get; set; } = new();

	#endregion

	#region Menu Events

	async void OnNew(object sender, EventArgs eventArgs)
	{
		string file = await _filePicker.BrowseForBibliographyFile();
		if (!string.IsNullOrEmpty(file))
		{
			_viewModel.NewProject(file);
		}
	}

	async void OnOpen(object sender, EventArgs eventArgs)
	{
		string file = await _filePicker.BrowseForProjectFile();
		if (!string.IsNullOrEmpty(file))
		{
			_viewModel.OpenProjectWithPathSave(file);
		}
	}

	async void OnSave(object sender, EventArgs eventArgs)
	{
		if (_viewModel.SavePathRequired)
		{
			string? file = await _saveFilePicker.PickAsync(new PickOptions() { FileTypes=_filePicker.CreateBibliographyProjectFileType() } );
			if (!string.IsNullOrEmpty(file))
			{
				_viewModel.Save(file);
			}
		}
		else
		{
			_viewModel.Save();
		}
	}

	async void OnSaveAs(object sender, EventArgs eventArgs)
	{
		string? file = await _saveFilePicker.PickAsync(new PickOptions() { FileTypes=_filePicker.CreateBibliographyProjectFileType() } );
		if (!string.IsNullOrEmpty(file))
		{
			_viewModel.Save(file);
		}
	}

	void OnClose(object sender, EventArgs eventArgs)
	{
		_viewModel.CloseProject();
	}

	async void OnProjectOptions(object sender, EventArgs eventArgs)
	{
		ProjectOptionsViewModel viewModel = new(BibtexProject.Instance!.Settings);

		ProjectOptionsView	view	= new(viewModel);
		object?				result	= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResut && boolResut)
		{
			BibtexProject.Instance.Settings = viewModel.Settings;
		}
	}

	async void OnProgramOptions(object sender, EventArgs eventArgs)
	{
		ProgramOptionsViewModel	viewModel	= new();
		ProgramOptionsView		view		= new(viewModel);
		object?					result		= await Shell.Current.ShowPopupAsync(view);
	}

	async void OnWebSearchSettings(object sender, EventArgs eventArgs)
	{
		WebSettingsViewModel	viewModel	= new();
		WebSearchSettingsView	view		= new(viewModel);
		object?					result		= await Shell.Current.ShowPopupAsync(view);
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

	async void OnNewBibEntry(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync(nameof(EditRawBibEntryForm), true, new Dictionary<string, object>
		{
			{ "AddMode",  true }
		});
	}

	async void OnNewBibEntryFromTemplate(object sender, EventArgs eventArgs)
	{
		TemplateSelectionViewModel	viewModel	= new(_viewModel.Project.BibEntryInitialization.TemplateNames);
		TemplateSelectionView		view		= new(viewModel);
		object?						result		= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			BibEntry entry = BibEntry.NewBibEntryFromTemplate(_viewModel.Project.BibEntryInitialization, viewModel.Template);
	
			await Shell.Current.GoToAsync(nameof(EditRawBibEntryForm), true, new Dictionary<string, object>
			{
				{ "AddMode",  true },
				{ "BibEntry", entry }
			});
		}
	}

	/// <summary>
	/// Navigation back from the bibliography edit page.  The NavigationCommand and NavigationObject get set and this gets called.
	/// </summary>
	/// <param name="args"></param>
	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		switch (NavigationCommand)
		{
			case "Save":
				_viewModel.Insert(NavigationObject);
				break;

			case "Replace":
				_viewModel.ReplaceSelected(NavigationObject);
				break;
		}
	}

	async void OnEditBibEntry(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync(nameof(EditRawBibEntryForm), true, new Dictionary<string, object>
		{
			{ "AddMode",  false },
			{ "BibEntry", _viewModel.SelectedItem! }
		});
	}

	async void OnDeleteBibEntry(object sender, EventArgs eventArgs)
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