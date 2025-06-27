using BibTeXLibrary;
using BibtexManager;
using BibtexManager.Project;
using BibTexManager.ViewModels;
using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;
using DigitalProduction.Maui.Storage;
using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTexManager.Views;

[QueryProperty(nameof(NavigationCommand), "NavigationCommand")]
[QueryProperty(nameof(NavigationObject), "NavigationObject")]
public partial class MainPage : DigitalProductionMainPage
{
	#region Fields

	private readonly MainViewModel		_viewModel;
	private readonly IBibTexFilePicker	_filePicker			= DigitalProduction.Maui.Services.ServiceProvider.GetService<IBibTexFilePicker>();
	private readonly ISaveFilePicker	_saveFilePicker		= DigitalProduction.Maui.Services.ServiceProvider.GetService<ISaveFilePicker>();

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

	#region File

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

	#endregion

	#region Project

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
		_									= await Shell.Current.ShowPopupAsync(view);
	}

	async void OnWebSearchSettings(object sender, EventArgs eventArgs)
	{
		WebSettingsViewModel	viewModel	= new();
		WebSearchSettingsView	view		= new(viewModel);
		_									= await Shell.Current.ShowPopupAsync(view);
	}

	#endregion

	#region Tools

	async void OnCheckTagQuality(object sender, EventArgs eventArgs)
	{
		bool breakNext = false;

		MessageBoxYesNoToAllResult lastDialogResult = MessageBoxYesNoToAllResult.Cancel;

		foreach (TagProcessingData tagProcessingData in _viewModel.CheckQuality())
		{
			// If the processing was cancelled, we break.  We have to loop back around here to give the
			// processing a chance to finish (it was yielded).  Now exit before processing another entry.
			if (breakNext)
			{
				break;
			}

			CorrectionViewModel	viewModel = new(tagProcessingData);

			if (lastDialogResult == MessageBoxYesNoToAllResult.YesToAll)
			{
				viewModel.SetResult(MessageBoxYesNoToAllResult.YesToAll);
				continue;
			}

			CorrectionView		view		= new(viewModel);
			object?				result		= await Shell.Current.ShowPopupAsync(view);

			if (result is MessageBoxYesNoToAllResult messageBoxResult)
			{
				lastDialogResult	= messageBoxResult;
				breakNext			= messageBoxResult == MessageBoxYesNoToAllResult.Cancel;
			}
		}
	}

	async void OnBulkSpeImport(object sender, EventArgs eventArgs)
	{
		string? file = await BrowseForInputFile();

		if (file != null)
		{
			BulkImport(new SpeBulkTitleImporter(file));
		}
	}

	async void OnSpeConferenceImport(object sender, EventArgs eventArgs)
	{
		string? file = await BrowseForInputFile();

		if (file != null)
		{
			BulkImport(new SpeConferenceImporter(file));
		}
	}

	private async void BulkImport(IBulkImporter importer)
	{
		ImportErrorViewModel viewModel;
		ImportErrorView		 view;

		foreach (ImportResult importResult in _viewModel.BulkImport(importer))
		{
			switch (importResult.Result)
			{
				case ResultType.Successful:
					break;

				case ResultType.NotFound:
				case ResultType.Error:
					viewModel	= new(importer, importResult);
					view		= new ImportErrorView(viewModel);
					_			= await Shell.Current.ShowPopupAsync(view);
					break;
			}
		}
	}

	#endregion

	#region Help

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
	/// <param name="eventArgs"></param>
	protected override void OnNavigatedTo(NavigatedToEventArgs eventArgs)
	{
		base.OnNavigatedTo(eventArgs);

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

	private async Task<string?> BrowseForInputFile()
	{
		MainViewModel? viewModel = BindingContext as MainViewModel;
		System.Diagnostics.Debug.Assert(viewModel != null);

		try
		{
			PickOptions pickOptions = new() { PickerTitle="Select an Input File" }; //, FileTypes=viewModel.GetInputFileTypes() };
			FileResult? result      = await BrowseForFile(pickOptions);

			if (result != null)
			{
				return result.FullPath;
			}
		}
		catch (Exception exception)
		{
			await DisplayAlert("Error", "An exception occured:"+Environment.NewLine+exception.Message, "OK");
		}

		return null;
	}

	public static async Task<FileResult?> BrowseForFile(PickOptions options)
	{
		try
		{
			return await FilePicker.PickAsync(options);
		}
		catch
		{
			// The user canceled or something went wrong.
		}

		return null;
	}

	#endregion
}