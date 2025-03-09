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
	ProjectExtractor?					_projectExtractor		= null;

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

	public BibtexProject							Project { get => BibtexProject.Instance ?? throw new NullReferenceException("Project is null.");  }

	[ObservableProperty]
	public partial IRecentPathsManagerService		RecentPathsManagerService { get; set; }

	[ObservableProperty]
	public partial bool								IsSubmittable { get; set; }						= false;

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

	#region Methods and Commands

	public void NewProject(string bibliographyFile)
	{
		BibtexProject.New(bibliographyFile);
		if (BibtexProject.Instance != null)
		{
			Items = BibtexProject.Instance.Bibliography.Entries;
		}
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
	}

	[RelayCommand]
	void ShowRemovedMessage(string path)
	{
		_dialogService.ShowMessage("File Not Found", $"The path \"{path}\" was was not found.", "OK");
	}

	public void CloseProject()
	{
		System.Diagnostics.Debug.Assert(BibtexProject.Instance != null);
		BibtexProject.Instance.Close();
		Items?.Clear();
		Items = null;
	}

	#endregion

} // End class.