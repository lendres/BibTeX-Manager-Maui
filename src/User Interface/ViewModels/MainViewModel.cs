using BibTeXLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Services;
using DigitalProduction.Maui.ViewModels;

using System.Collections.ObjectModel;

namespace BibTeXManager.ViewModels;

public partial class MainViewModel : DataGridBaseViewModel<BibEntry>
{
	#region Fields

	private readonly IDialogService		_dialogService;

	#endregion

	#region Construction

	public MainViewModel(IRecentPathsManagerService recentPathsManagerService, IDialogService dialogService)
    {
		RecentPathsManagerService	= recentPathsManagerService;
		_dialogService				= dialogService;

		ISaveService saveBeforeExitService			= DigitalProduction.Maui.Services.ServiceProvider.GetService<ISaveService>();
		saveBeforeExitService.IsModifiedFunction	= IsModified;
		saveBeforeExitService.SaveFunction			= SaveAsync;

		BibTeXProject.New(Preferences.ProjectSettings);
		ProjectInitialization();
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

	private void OnProjectPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs eventArgs)
	{
		ValidateHasTemplates();
	}

	partial void OnProjectOpenChanged(bool value)
	{
		ValidateCanSave();
		ValidateHasTemplates();
	}

	[RelayCommand]
	private void CopyCiteKeyToClipboard()
	{
		System.Diagnostics.Debug.Assert(SelectedItem != null);
		Clipboard.Default.SetTextAsync(SelectedItem.Key);
	}

	#endregion

	#region Methods and Commands

	#region DataGridBaseViewModel Overrides

	public override void Insert(BibEntry item, int position = 0, bool select = true)
	{
		if (Project.Settings.SortBibliography)
		{
			// If sorting, ignore the position and add based on the sort method.
			Project.Bibliography.Insert(item, Project.Settings.BibliographySortMethod);
		}
		else
		{

			if (position == 0)
			{
				// If we are adding new (position == 0) and not sorting, add to the end of the list.
				Project.Bibliography.Add(item);
			}
			else
			{
				// If we are not sorting, then add at the specified position.
				Project.Bibliography.Insert(item, position);
			}
		}

		FinalizeInsert(item, select);
	}

	#endregion

	#region File Menu

	public void New()
	{
		Project.NewBibliographyFile();
		if (Project.Bibliography != null)
		{
			Items = Project.Bibliography.Entries;
		}
		Modified = true;
		ValidateCanSave();
	}

	public void OpenWithPathSave(string projectFile)
	{
		RecentPathsManagerService.PushTop(projectFile);
		Open(projectFile);
	}

	[RelayCommand]
	public void Open(string file)
	{
		System.Diagnostics.Debug.Assert(BibTeXProject.Instance != null);
		Items?.Clear();
		Project.NewBibliographyFile();
		Project.ReadBibliographyFile(file);
		Items		=  Project.Bibliography.Entries;
		ProjectOpen	= true;
	}

	void ProjectInitialization()
	{
		Project.ModifiedChanged += OnProjectModifiedChanged;
		Project.PropertyChanged += OnProjectPropertyChanged;
	}

	[RelayCommand]
	void ShowRemovedMessage(string path)
	{
		_dialogService.ShowMessage("File Not Found", $"The path \"{path}\" was was not found.", "OK");
	}

	public void Save(string path)
	{
		RecentPathsManagerService.PushTop(path);
		Project.WriteBibliographyFile(path);
	}

	[RelayCommand]
	public void Save()
	{
		Project.WriteBibliographyFile();
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
	public override bool Find(string search)
	{
		List<string> tagNames		= ["author", "title"];
		List<BibEntry> findResults	= Project.Bibliography.SearchBibEntries(tagNames, true, search);
		return SetSearchResults(search, findResults);

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

	#endregion

	#region Save Before Exit

	/// <summary>
	/// Interface function for the save before exit service to check if the project is modified and needs to be saved before exiting.
	/// </summary>
	/// <returns>True if the project is modified, false otherwise.</returns>
	public bool IsModified()=> Modified;

	/// <summary>
	/// Interface function for the save before exit service to.  Asynchronously saves the current state or changes to the underlying data store.
	/// </summary>
	/// <remarks>
	/// If the operation is canceled via the provided cancellation token, the returned task will be in a canceled state.
	/// </remarks>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the save operation.</param>
	/// <returns>A task that represents the asynchronous save operation.</returns>
	async Task<bool> SaveAsync(CancellationToken cancellationToken = default)
	{
		Save();
		return true;
	}

	#endregion

	#endregion

} // End class.