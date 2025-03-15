using BibTeXLibrary;
using BibtexManager;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Services;
using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Projects;

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
	public partial bool								IsSubmittable { get; set; }					= false;

	#endregion

	#region Validation

	//[RelayCommand]
	//private void ValidatePostprocessor()
	//{
	//	Postprocessor.Validate();
 //       ValidateSubmittable();
	//}

	//public bool ValidateSubmittable() => IsSubmittable =
	//	XmlInputFile.IsValid &&
	//	XsltFile.IsValid;

	#endregion

	#region Events

	private void OnProjectModifiedChanged(object sender, bool modified)
	{
		Modified = modified;
		ValidateCanSave();
	}

	partial void OnProjectOpenChanged(bool value)
	{
		ValidateCanSave();
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

	#endregion

	#region Helper Methods

	private void ValidateCanSave()
	{
		CanSave = Modified && ProjectOpen;
	}

	#endregion

} // End class.