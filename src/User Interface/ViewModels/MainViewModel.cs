using BibTeXLibrary;
using BibtexManager;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Services;
using DigitalProduction.Maui.ViewModels;

namespace BibTexManager.ViewModels;

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

		InitializeValues();
		AddValidations();
		//ValidateSubmittable();
	}

	private void InitializeValues()
	{
	}

	private void AddValidations()
	{
	}

	#endregion

	#region Properties

	public BibtexProject							Project { get => BibtexProject.Instance ?? throw new NullReferenceException("Project is null."); }
	public bool										SavePathRequired { get => !(BibtexProject.Instance?.IsSaveable) ?? false; }

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
		HasTemplates = ProjectOpen && BibtexProject.Instance?.BibEntryInitialization.TemplateNames.Count > 0;
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

	public void NewProject(string bibliographyFile)
	{
		BibtexProject.New(bibliographyFile);
		if (BibtexProject.Instance != null)
		{
			Items = BibtexProject.Instance.Bibliography.Entries;
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
		BibtexProject.Deserialize(projectFile);
		if (BibtexProject.Instance != null)
		{
			Items = BibtexProject.Instance.Bibliography.Entries;
		}
		ProjectInitialization();
	}

	void ProjectInitialization()
	{
		BibtexProject.Instance!.ModifiedChanged += OnProjectModifiedChanged;
		BibtexProject.Instance!.PropertyChanged += OnProjectPropertyChanged;
		ProjectOpen = true;
	}

	[RelayCommand]
	void ShowRemovedMessage(string path)
	{
		_dialogService.ShowMessage("File Not Found", $"The path \"{path}\" was was not found.", "OK");
	}

	public void Save(string path)
	{
		System.Diagnostics.Debug.Assert(BibtexProject.Instance != null);
		RecentPathsManagerService.PushTop(path);
		BibtexProject.Instance.Serialize(path);
	}

	[RelayCommand]
	public void Save()
	{
		System.Diagnostics.Debug.Assert(BibtexProject.Instance != null);
		BibtexProject.Instance.Serialize();
	}

	public void CloseProject()
	{
		System.Diagnostics.Debug.Assert(BibtexProject.Instance != null);
		BibtexProject.Instance.Close();
		Items?.Clear();
		Items = null;
		ProjectOpen = false;
	}

	[RelayCommand]
	public void SortBibliographyEntries()
	{
		System.Diagnostics.Debug.Assert(BibtexProject.Instance != null);
		BibtexProject.Instance.SortBibliographyEntries();
	}

	/// <summary>
	/// Check the quality of the text in the text box.
	/// </summary>
	public IEnumerable<TagProcessingData> CheckQuality()
	{
		System.Diagnostics.Debug.Assert(BibtexProject.Instance != null);

		// Cleaning.
		foreach (TagProcessingData tagProcessingData in BibtexProject.Instance.CleanAllEntries())
		{
			yield return tagProcessingData;
		}
	}

	#endregion

} // End class.