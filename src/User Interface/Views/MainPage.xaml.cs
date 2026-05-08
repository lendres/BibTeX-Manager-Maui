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
public partial class MainPage : DigitalProductionMainPage
{
	#region Fields

	private readonly MainViewModel		_viewModel;

	private readonly IBibTeXFilePicker	_filePicker;
	private readonly ISaveFilePicker	_saveFilePicker;

	private readonly bool				_animateScrollToSelection		= false;

	#endregion

	#region Construction

	public MainPage(MainViewModel viewModel, IPageProvider pageProvider, IBibTeXFilePicker filePicker, ISaveFilePicker saveFilePicker)
	{
		InitializeComponent();

		pageProvider.CurrentPage	= this;
		_filePicker					= filePicker;
		_saveFilePicker				= saveFilePicker;

		BindingContext				= viewModel;
		_viewModel					= viewModel;
		_viewModel.MenuHostingPage	= this;

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

	async void OnNew(object sender, EventArgs eventArgs)
	{
		_viewModel.New();
	}

	async void OnOpen(object sender, EventArgs eventArgs)
	{
		string file = await _filePicker.BrowseForBibliographyFile();
		if (!string.IsNullOrEmpty(file))
		{
			await _viewModel.OpenWithPathSave(file);
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

	async void OnSaveAs(object sender, EventArgs eventArgs)
	{
		string? file = await _saveFilePicker.PickAsync(new PickOptions() { FileTypes=_filePicker.CreateBibliographyFilePickerFileType() } );
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

	#region Edit

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
			FindInDataGridView();
		}
	}

	private async void ShowFindDialogBox()
	{
		SearchTermsViewModel    viewModel   = new();
		SearchTermsView         view        = new(viewModel);
		object?                 result      = await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResut && boolResut)
		{
			bool foundEntries = _viewModel.Find(viewModel.SearchTermsString);
			if (!foundEntries)
			{
				await DisplayAlert("Not Found", "No entries found for the specified search term(s).\nSearch string: "+viewModel.SearchTermsString , "OK");
			}
			else
			{
				FindInDataGridView();
			}
		}
	}

	private void FindInDataGridView()
	{
		_viewModel.SelectNextFoundItem();
		BibliographyDataGrid.ScrollTo(_viewModel.SelectedItem!, ScrollToPosition.Center, _animateScrollToSelection);
	}

	private void OnScrollToSelection(object sender, EventArgs eventArgs)
	{
		if (_viewModel.SelectedItem != null)
		{
			BibliographyDataGrid.ScrollTo(_viewModel.SelectedItem, ScrollToPosition.Center, _animateScrollToSelection);
		}
	}

    #endregion

	#region Configuration

	async void OnEditStringConstants(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync(nameof(StringConstantsView), true);
	}

	#endregion

    #region Settings

    async void OnProjectOptions(object sender, EventArgs eventArgs)
    {
		await Shell.Current.GoToAsync(nameof(SettingsView), true);


        //ProjectOptionsViewModel viewModel = new(BibTeXProject.Instance!.Settings);

        //SettingsView view = new(viewModel);
        //object? result = await Shell.Current.ShowPopupAsync(view);

        //if (result is bool boolResut && boolResut)
        //{
        //    BibTeXProject.Instance.Settings = viewModel.Settings;
        //}
    }

    async void OnProgramOptions(object sender, EventArgs eventArgs)
	{
		ProgramOptionsViewModel viewModel = new();
		ProgramOptionsView view = new(viewModel);
		_ = await Shell.Current.ShowPopupAsync(view);
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
			BibliographyDataGrid.ScrollTo(_viewModel.SelectedItem!, ScrollToPosition.Center, _animateScrollToSelection);
		}
	}

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
				_viewModel.Insert(NavigationObject);
				BibliographyDataGrid.ScrollTo(_viewModel.SelectedItem!, ScrollToPosition.Center, _animateScrollToSelection);
				break;

			case "Replace":
				System.Diagnostics.Debug.Assert(NavigationObject != null);
				_viewModel.ReplaceSelected(NavigationObject);
				BibliographyDataGrid.ScrollTo(_viewModel.SelectedItem!, ScrollToPosition.Center, _animateScrollToSelection);
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