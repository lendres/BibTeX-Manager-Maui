using BibTeXLibrary;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;
using DigitalProduction.Maui.Enums;
using DigitalProduction.Maui.Services;
using DigitalProduction.Maui.Storage;
using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Maui.Views;
using System.Timers;

namespace BibTeXManager.Views;

[QueryProperty(nameof(NavigationCommand), "NavigationCommand")]
[QueryProperty(nameof(NavigationObject), "NavigationObject")]
public partial class MainView : DigitalProductionMainPage
{
	#region Fields

	private readonly MainViewModel					_viewModel;
	private readonly IBibTeXFilePicker				_filePicker;
	private readonly ISaveFilePicker				_saveFilePicker;
	private readonly ISaveService					_saveBeforeExitService;

	private readonly List<IBibliographyPartView>	_bibliographyPartViews;

	private System.Threading.Timer?					_timer;

	#endregion

	#region Construction

	public MainView(MainViewModel viewModel, IBibTeXFilePicker filePicker, ISaveFilePicker saveFilePicker, ISaveService saveBeforeExitService)
	{
		InitializeComponent();

		_filePicker					= filePicker;
		_saveFilePicker				= saveFilePicker;
		_saveBeforeExitService		= saveBeforeExitService;

		BindingContext				= viewModel;
		_viewModel					= viewModel;
		_viewModel.MenuHostingPage	= this;

		_bibliographyPartViews		= [_headerView, _stringsEditView, _bibliographyEditView];

		// We seem to need to add a delay to allow everything to be fully setp and connected before trying to open the last project.
		// If we try to open it immediately, the views don't seem to be fully connected to the view model and they don't update with
		// the opened project.
		_timer = new System.Threading.Timer((obj) => { _ = OpenLastProject(); _timer?.Dispose(); }, null, 500, Timeout.Infinite);
	}

	#endregion

	#region Properties

	public string NavigationCommand { get; set; } = string.Empty;

	public BibEntry? NavigationObject { get; set; } = null;

	#endregion

	#region Menu Events

	#region File

	private async void OnNew(object sender, EventArgs eventArgs)
	{
		if (await TryCloseProject())
		{
			foreach (IBibliographyPartView bibliographyPartView in _bibliographyPartViews)
			{
				bibliographyPartView.New();
			}
			_viewModel.New();
		}
	}

	private async void OnRecentPathClicked(object? sender, PathClickedEventArgs eventArgs)
	{
		await Open(eventArgs.Path);
	}

	async void OnOpen(object sender, EventArgs eventArgs)
	{
		// Only proceed if the user actually selected a file.  If they cancelled, the file will be null or empty and we do nothing.
		// If they did select a file, we try to close the current project. If that succeeds (they didn't cancel out of closing), then we open the new file.
		string file = await _filePicker.BrowseForBibliographyFile();
		await Open(file);
	}

	private async Task Open(string path)
	{
		if (!string.IsNullOrEmpty(path))
		{
			if (await TryCloseProject())
			{
				await _viewModel.OpenWithPathSave(path);
				foreach (IBibliographyPartView bibliographyPartView in _bibliographyPartViews)
				{
					bibliographyPartView.Open();
				}
			}
		}
	}

	async void OnSave(object sender, EventArgs eventArgs)
	{
		if (_viewModel.SavePathRequired)
		{
			string? file = await _saveFilePicker.PickAsync(new PickOptions() { FileTypes=_filePicker.CreateBibliographyFilePickerFileType() } );
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

	private async void OnSaveAs(object sender, EventArgs eventArgs)
	{
		string? file = await _saveFilePicker.PickAsync(new PickOptions() { FileTypes=_filePicker.CreateBibliographyFilePickerFileType() } );
		if (!string.IsNullOrEmpty(file))
		{
			try
			{
				_viewModel.Save(file);
			}
			catch (Exception exception)
			{
				await DisplayAlert("Write Error", exception.Message, "OK");
			}
		}
	}

	private async void OnClose(object sender, EventArgs eventArgs)
	{
		_ = await TryCloseProject();
	}

	private async Task<bool> TryCloseProject()
	{
		SaveChoice closeChoice = await _saveBeforeExitService.PromptSaveChangesAsync();

		switch (closeChoice)
		{
			case SaveChoice.Cancel:
				return false;
		}

		foreach (IBibliographyPartView bibliographyPartView in _bibliographyPartViews)
		{
			bibliographyPartView.Close();
		}

		_viewModel.Close();
		return true;
	}

	#endregion

	#region Edit

	async void OnNewEntry(object sender, EventArgs eventArgs)
	{
		GetActiveGridView().OnNewEntry(sender, eventArgs);
	}

	async void OnNewBibEntryFromTemplate(object sender, EventArgs eventArgs)
	{
		_bibliographyEditView.OnNewBibEntryFromTemplate(sender, eventArgs);
	}

	async void OnEditEntry(object sender, EventArgs eventArgs)
	{
		GetActiveGridView().OnEditEntry(sender, eventArgs);
	}

	async void OnDeleteEntry(object sender, EventArgs eventArgs)
	{
		GetActiveGridView().OnDeleteEntry(sender, eventArgs);
	}

	void OnFind(object sender, EventArgs eventArgs)
	{
		ShowFindDialogBox();
	}

	void OnFindNext(object sender, EventArgs eventArgs)
	{
		if (_viewModel.RequireSearchString)
		{
			ShowFindDialogBox();
		}
		else
		{
			SelectNextFoundItem();
		}
	}

	private async void ShowFindDialogBox()
	{
		SearchTermsViewModel    viewModel   = new();
		SearchTermsView         view        = new(viewModel);
		object?                 result      = await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResut && boolResut)
		{
			_viewModel.SearchString = viewModel.SearchTermsString;
			_stringsEditView.Find(viewModel.SearchTermsString);
			_bibliographyEditView.Find(viewModel.SearchTermsString);

			SearchResult searchResult = GetActiveGridView().Find(viewModel.SearchTermsString);
			if (searchResult == SearchResult.NoItemsFound)
			{
				await DisplayAlert("Not Found", "No entries found for the specified search term(s).\nSearch string: "+viewModel.SearchTermsString , "OK");
			}
			else
			{
				SelectNextFoundItem();
			}
		}
	}

	private void SelectNextFoundItem()
	{
		switch (GetActiveGridView().SelectNextFoundItem())
		{
			case SearchResult.NoMoreFoundItems:
				DisplayAlert("Find", "No more items were found.", "OK");
				break;
			case SearchResult.NoItemsFound:
				DisplayAlert("Find", "No items were found.", "OK");
				break;
		};
	}

	private void OnScrollToSelection(object sender, EventArgs eventArgs)
	{
		GetActiveGridView().OnScrollToSelection(sender, eventArgs);
	}

    #endregion

    #region Settings

    async void OnOptions(object sender, EventArgs eventArgs)
    {
		await Shell.Current.GoToAsync(nameof(SettingsView), true);
    }

	async void OnEditBibliographyNameMapping(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync(nameof(NameMappingView), true);
	}

	async void OnEditBibliographyTemplates(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync(nameof(TemplatesEditView), true);
	}

	async void OnConfigureFieldQualityProcessing(object sender, EventArgs eventArgs)
	{
		//await Shell.Current.GoToAsync(nameof(FieldQualityProcessingView), true);
	}

	#endregion

	#region Tools

	async void OnCheckFieldQuality(object sender, EventArgs eventArgs)
	{
		bool breakNext = false;

		MessageBoxYesNoToAllResult lastDialogResult = MessageBoxYesNoToAllResult.Cancel;

		foreach (FieldProcessingData tagProcessingData in _viewModel.CheckQuality())
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

	#region Navigation

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
				System.Diagnostics.Debug.Assert(NavigationObject != null);
				_bibliographyEditView.Insert(NavigationObject);
				break;

			case "Replace":
				System.Diagnostics.Debug.Assert(NavigationObject != null);
				_bibliographyEditView.ReplaceSelected(NavigationObject);
				break;
			case "Cancel":
			default:
				// Nothing to do.
				break;
		}
	}

	#endregion

	#region Methods

	private async Task OpenLastProject()
	{
		if (Preferences.LoadLastProjectAtStartUp)
		{
			string path = Preferences.RecentPathsManagerService.GetTop();
			if (System.IO.Path.Exists(path))
			{
				await Open(path);
			}
		}
	}

	private IBibliographyPartDataGridView GetActiveGridView()
	{
		return (IBibliographyPartDataGridView)_bibliographyPartViews[(int)_viewModel.ActiveBibliographyPart];
	}

	#endregion
}