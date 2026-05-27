using BibTeXManager.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Services;
using DigitalProduction.ViewModels;

namespace BibTeXManager.ViewModels;

public partial class MainViewModel : ProjectViewModel<BibTeXProject>
{
	#region Fields

	private readonly IDialogService		_dialogService;

	#endregion

	#region Construction

	public MainViewModel(IRecentPathsManagerService recentPathsManagerService, IDialogService dialogService, ISaveService saveBeforeExitService) :
		base(BibTeXProject.Instance ?? throw new NullReferenceException("Project is null."))
    {
		RecentPathsManagerService	= recentPathsManagerService;
		_dialogService				= dialogService;

		saveBeforeExitService.IsModifiedFunction	= IsModified;
		saveBeforeExitService.SaveFunction			= SaveAsync;

		ProjectInitialization();
	}

	void ProjectInitialization()
	{
		Project.ModifiedChanged += OnProjectModifiedChanged;
		Project.PropertyChanged += OnProjectPropertyChanged;
		Project.Opened += OnProjectOpenChanged;
		Project.Closed += OnProjectOpenChanged;
	}

	#endregion

	#region Properties

	public IRecentPathsManagerService			RecentPathsManagerService	{ get; set; }

	public Page?								MenuHostingPage				{ get => _dialogService.HostingPage; set => _dialogService.HostingPage = value; }

	public bool									SavePathRequired			{ get => !Project.HasSavePath; }

	public string?								SearchString				{ get; set; } = null;

	public bool									RequireSearchString			{ get => SearchString == null; }

	[ObservableProperty]
	public partial BibliographyPartType			ActiveBibliographyPart		{  get; set; }	= BibliographyPartType.BibliographyEntries;

	[ObservableProperty]
	public partial object?						SelectedStringItem			{ get; set; }

	[ObservableProperty]
	public partial object?						SelectedBibliographyItem	{ get; set; }

	[ObservableProperty]
	public partial bool							CanAdd						{ get; set; } = false;

	[ObservableProperty]
	public partial bool							CanAddFromTemplates			{ get; set; } = false;

	[ObservableProperty]
	public partial bool							IsItemSelected				{ get; set; } = false;

	#endregion

	#region Validation

	private void ValidateHasTemplates()
	{
		CanAddFromTemplates =
			Project.IsOpen &&
			BibTeXProject.Instance?.BibEntryInitialization.TemplateNames.Count > 0 &&
			ActiveBibliographyPart == BibliographyPartType.BibliographyEntries;
	}

	public void ValidateCanAdd()
	{
		CanAdd = Project.IsOpen && ActiveBibliographyPart != BibliographyPartType.Header;
	}

	public void ValidateIsItemSelected()
	{
		IsItemSelected = ActiveBibliographyPart switch
		{
			BibliographyPartType.Header					=> false,
			BibliographyPartType.StringEntries			=> SelectedStringItem != null,
			BibliographyPartType.BibliographyEntries    => SelectedBibliographyItem != null,
			_ => throw new InvalidOperationException("Invalid bibliography part type.")
		};
	}

	#endregion

	#region Events

	private void OnProjectModifiedChanged(object sender, bool modified)
	{
		ValidateHasTemplates();
	}

	private void OnProjectPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs eventArgs)
	{
		ValidateHasTemplates();
	}

	private void OnProjectOpenChanged()
	{
		ValidateHasTemplates();
	}

	partial void OnSelectedStringItemChanged(object? value)
	{
		ValidateIsItemSelected();
	}

	partial void OnSelectedBibliographyItemChanged(object? value)
	{
		ValidateIsItemSelected();
	}

	partial void OnActiveBibliographyPartChanged(BibliographyPartType value)
	{
		ValidateHasTemplates();
		ValidateCanAdd();
		ValidateIsItemSelected();
	}

	#endregion

	#region Methods and Commands

	#region File Menu

	public void New()
	{
		Project.NewBibliographyFile();
	}

	public async Task OpenWithPathSave(string projectFile)
	{
		RecentPathsManagerService.PushTop(projectFile);
		await Open(projectFile);
	}

	public async Task Open(string file)
	{
		Project.NewBibliographyFile();
		Project.ReadBibliographyFile(file);
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

	public void Close()
	{
		Project.Close();
		IsOpen		= false;
		Modified	= false;
	}

	#endregion

	#region Edit Menu
	#endregion

	#region Tools Menu

	[RelayCommand]
	public void SortStringEntries()
	{
		Project.SortStringEntries();
	}

	[RelayCommand]
	public void SortBibliographyEntries()
	{
		Project.SortBibliographyEntries();
	}

	/// <summary>
	/// Check the quality of the text in the text box.
	/// </summary>
	public IEnumerable<FieldProcessingData> CheckQuality()
	{
		// Cleaning.
		foreach (FieldProcessingData tagProcessingData in Project.CleanAllEntries())
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
	public bool IsModified() => RequiresSave;

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