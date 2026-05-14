using BibTeXLibrary;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;
using DigitalProduction.Maui.Services;
using DigitalProduction.Maui.Storage;
using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Maui.Views;

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

	#endregion

	#region Construction

	public MainView(MainViewModel viewModel, IPageProvider pageProvider, IBibTeXFilePicker filePicker, ISaveFilePicker saveFilePicker, ISaveService saveBeforeExitService)
	{
		InitializeComponent();

		pageProvider.CurrentPage		= this;
		_filePicker						= filePicker;
		_saveFilePicker					= saveFilePicker;
		_saveBeforeExitService			= saveBeforeExitService;

		BindingContext					= viewModel;
		_viewModel						= viewModel;
		_viewModel.MenuHostingPage		= this;

		if (Preferences.LoadLastProjectAtStartUp)
		{
			_ = OpenLastProject();
		}
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
			_bibliographyEditGridView.New();
			_viewModel.New();
		}
	}

	private async void OnRecentPathClicked(object? sender, PathClickedEventArgs eventArgs)
	{
		await Open(eventArgs.Path);
	}

	async void OnOpen(object sender, EventArgs eventArgs)
	{
		if (await TryCloseProject())
		{
			string file = await _filePicker.BrowseForBibliographyFile();
			if (!string.IsNullOrEmpty(file))
			{
				await Open(file);
			}
		}
	}

	private async Task Open(string path)
	{
		await _viewModel.OpenWithPathSave(path);
		_bibliographyEditGridView.Open();
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

		_bibliographyEditGridView.Close();
		_viewModel.Close();
		return true;
	}

	#endregion

	#region Edit

	async void OnNewBibEntry(object sender, EventArgs eventArgs)
	{
		_bibliographyEditGridView.OnNewBibEntry(sender, eventArgs);
	}

	async void OnNewBibEntryFromTemplate(object sender, EventArgs eventArgs)
	{
		_bibliographyEditGridView.OnNewBibEntryFromTemplate(sender, eventArgs);
	}

	async void OnEditBibEntry(object sender, EventArgs eventArgs)
	{
		_bibliographyEditGridView.OnEditBibEntry(sender, eventArgs);
	}

	async void OnDeleteBibEntry(object sender, EventArgs eventArgs)
	{
		_bibliographyEditGridView.OnDeleteBibEntry(sender, eventArgs);
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
			bool foundEntries = _bibliographyEditGridView.Find(viewModel.SearchTermsString);
			if (!foundEntries)
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
		_bibliographyEditGridView.SelectNextFoundItem();
	}

	private void OnScrollToSelection(object sender, EventArgs eventArgs)
	{
		_bibliographyEditGridView.OnScrollToSelection(sender, eventArgs);
	}

    #endregion

	#region Configuration

	async void OnEditStringConstants(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync(nameof(StringsEditView), true);
	}

	#endregion

    #region Settings

    async void OnOptions(object sender, EventArgs eventArgs)
    {
		await Shell.Current.GoToAsync(nameof(SettingsView), true);
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
				_bibliographyEditGridView.Insert(NavigationObject);
				break;

			case "Replace":
				System.Diagnostics.Debug.Assert(NavigationObject != null);
				_bibliographyEditGridView.ReplaceSelected(NavigationObject);
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
		string path = Preferences.RecentPathsManagerService.GetTop();
		if (System.IO.Path.Exists(path))
		{
			await _viewModel.Open(path);
		}
	}

	#endregion
}