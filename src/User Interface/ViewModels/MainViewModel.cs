using BibTeXLibrary;
using BibTeXManager;
using BibTeXManager.Project;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Services;
using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Http;

namespace BibTeXManager.ViewModels;

public partial class MainViewModel : DataGridBaseViewModel<BibEntry>
{
	#region Fields

	private string?                     _findString;
	private int                         _findIndex			= 0;
	private List<BibEntry>?				_findResults;

	private readonly IDialogService		_dialogService;

	#endregion

	#region Construction

	public MainViewModel(IRecentPathsManagerService recentPathsManagerService, IDialogService dialogService)
    {
		RecentPathsManagerService	= recentPathsManagerService;
		_dialogService				= dialogService;

		CustomSearch.SetCxAndKey(Preferences.CustomSearchEngineIdentifier, Preferences.SearchEngineApiKey);
	}

	#endregion

	#region Properties

	public BibTeXProject							Project { get => BibTeXProject.Instance ?? throw new NullReferenceException("Project is null."); }
	public bool										SavePathRequired { get => !(BibTeXProject.Instance?.IsSaveable) ?? false; }

	public IRecentPathsManagerService				RecentPathsManagerService { get; set; }

	[ObservableProperty]
	public partial bool								ProjectOpen { get; set; }					= false;

	[ObservableProperty]
	public partial bool								CanSave { get; set; }						= false;

	[ObservableProperty]
	public partial bool								HasTemplates { get; set; }					= false;

	[ObservableProperty]
	public partial bool								IsSubmittable { get; set; }					= false;

	public bool										RequireSearchString { get => _findString == null; }

	#endregion

	#region Validation

	private void ValidateCanSave()
	{
		CanSave = Modified && ProjectOpen;
	}

	private void ValidateHasTemplates()
	{
		HasTemplates = ProjectOpen && BibTeXProject.Instance?.BibEntryInitialization.TemplateNames.Count > 0;
	}

	#endregion

	#region Events

	private void OnProjectModifiedChanged(object sender, bool modified)
	{
		Modified = modified;
		ValidateCanSave();
		ValidateHasTemplates();
	}

	private void OnProjectPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		ValidateHasTemplates();
	}

	partial void OnProjectOpenChanged(bool value)
	{
		ValidateCanSave();
		ValidateHasTemplates();
	}

	#endregion

	#region Methods and Commands

	#region File Menu

	public void NewProject(string bibliographyFile)
	{
		BibTeXProject.New(bibliographyFile);
		if (BibTeXProject.Instance != null)
		{
			Items = BibTeXProject.Instance.Bibliography.Entries;
		}
		ProjectInitialization();
		Modified = true;
		ValidateCanSave();
	}

	public void OpenProjectWithPathSave(string projectFile)
	{
		RecentPathsManagerService.PushTop(projectFile);
		OpenProject(projectFile);
	}

	[RelayCommand]
	public void OpenProject(string projectFile)
	{
		BibTeXProject.Deserialize(projectFile);
		if (BibTeXProject.Instance != null)
		{
			Items = BibTeXProject.Instance.Bibliography.Entries;
		}
		ProjectInitialization();
	}

	void ProjectInitialization()
	{
		Project.ModifiedChanged += OnProjectModifiedChanged;
		Project.PropertyChanged += OnProjectPropertyChanged;
		ProjectOpen = true;
	}

	[RelayCommand]
	void ShowRemovedMessage(string path)
	{
		_dialogService.ShowMessage("File Not Found", $"The path \"{path}\" was was not found.", "OK");
	}

	public void Save(string path)
	{
		RecentPathsManagerService.PushTop(path);
		Project.Serialize(path);
	}

	[RelayCommand]
	public void Save()
	{
		Project.Serialize();
	}

	public void CloseProject()
	{
		Project.Close();
		Items?.Clear();
		Items = null;
		ProjectOpen = false;
	}

	#endregion

	#region Edit Menu

	/// <summary>
	/// Searches the bibliography for the specified search string in the author and title fields.
	/// </summary>
	/// <param name="search">Search term.</param>
	/// <returns>True if at least one BibEntry is found, false if no entries are found.</returns>
	public bool Find(string search)
	{
		// Reset index for new search.	
		_findIndex  = 0;
		_findString = search;

		List<string> tagNames	= ["author", "title"];
		_findResults			= Project.Bibliography.SearchBibEntries(tagNames, true, search);

		if (_findResults.Count > 0)
		{
			_findString = search;
			return true;
		}
		else
		{
			_findString		= null;
			_findResults	= null;
			return false;
		}
	}

	/// <summary>
	/// Selects the next found item in the search results.
	/// </summary>
	public void SelectNextFoundItem()
	{
		BibEntry searchBibEntry = _findResults![_findIndex++];
		SelectedItem = searchBibEntry;
		
		// Reset index if we reach the end of the list.
		if (_findIndex >= _findResults.Count)
		{
			_findIndex = 0;
		}
	}

	#endregion

	#region Tools Menu

	[RelayCommand]
	public void SortBibliographyEntries()
	{
		Project.SortBibliographyEntries();
	}

	/// <summary>
	/// Check the quality of the text in the text box.
	/// </summary>
	public IEnumerable<TagProcessingData> CheckQuality()
	{
		// Cleaning.
		foreach (TagProcessingData tagProcessingData in Project.CleanAllEntries())
		{
			yield return tagProcessingData;
		}
	}

	/// <summary>
	/// Do bulk importing of BibTeX entries using the specified importer.
	/// </summary>
	public IEnumerable<ImportResult> BulkImport(IBulkImporter importer)
	{
		importer.SetBibliographyInitialization(Project.Settings.UseBibEntryInitialization, Project.BibEntryInitialization);

		foreach (ImportResult importResult in importer.BulkImport())
		{
			System.Diagnostics.Debug.Assert(importResult.BibEntry != null);

			switch (importResult.Result)
			{
				case ResultType.Successful:
					Project.ApplyAllCleaning(importResult.BibEntry);
					int index = Project.GetEntryInsertIndex(importResult.BibEntry, 0);
					Insert(importResult.BibEntry, index);
					break;

				case ResultType.NotFound:
					yield return importResult;
					break;

				case ResultType.Error:
					yield return importResult;
					break;
			}
		}
	}

	/// <summary>
	/// Do bulk importing of BibTeX entries using the specified importer.
	/// </summary>
	public BibEntry? SingleImport(ISingleImporter importer, string searchTerms)
	{
		return importer.Import(searchTerms);
	}

	#endregion

	#endregion

} // End class.