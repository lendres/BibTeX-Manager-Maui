using BibTeXLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;

namespace BibTeXManager.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public SettingsViewModel()
	{
		Settings = new ProjectSettings(BibTeXProject.Instance!.Settings);
		Initialize();
		AddValidations();
		Settings.ModifiedChanged += OnSettingsModifiedChanged;
		Settings.PropertyChanged += OnSettingsPropertyChanged;
		SetModified(false);
	 }

	#endregion

	#region Properties

	#region General and Common

	[ObservableProperty]
	public partial ProjectSettings				Settings { get; set; }

	private bool								Modified  { get; set; }

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }

	public IReadOnlyList<string>				BracketType { get; set; }					= DigitalProduction.Reflection.Enumerations.GetAllDescriptionAttributesForType<EntryBracketType>();

	#endregion

	#region Program

	[ObservableProperty]
	public partial bool OpenLastProjectAtStartUp { get; set; } = false;

	[ObservableProperty]
	public partial bool RemoveNotFoundPaths { get; set; }

	[ObservableProperty]
	public partial int NumberOfItemsShown { get; set; }

	[ObservableProperty]
	public partial int NumberOfItemsToStore { get; set; }

	#endregion

	#region String Entries

	public IReadOnlyList<string>				SortStringsByItems { get; set; }			= DigitalProduction.Reflection.Enumerations.GetAllDescriptionAttributesForType<SortStringsBy>();

	#endregion

	#region Bibliography Entries

	[ObservableProperty]
	public partial ValidatableObject<string>	AuxiliaryFile { get; set; }					= new();

	[ObservableProperty]
	public partial ValidatableObject<string>	FieldOrderFile { get; set; }				= new();

	[ObservableProperty]
	public partial ValidatableObject<string>	FieldQualityFile { get; set; }				= new();

	[ObservableProperty]
	public partial ValidatableObject<string>	NameRemappingFile { get; set; }				= new();

	public IReadOnlyList<string>				SorBibliographyByItems { get; set; }		= DigitalProduction.Reflection.Enumerations.GetAllDescriptionAttributesForType<SortBibliographyBy>();

	#endregion

	#endregion

	#region Initialization

	private void Initialize()
	{
		OpenLastProjectAtStartUp	= Preferences.LoadLastProjectAtStartUp;
		RemoveNotFoundPaths			= Preferences.RecentPathsManagerService.RemoveNotFoundPaths;
		NumberOfItemsShown			= (int)Preferences.RecentPathsManagerService.NumberOfItemsShown;
		NumberOfItemsToStore		= (int)Preferences.RecentPathsManagerService.MaxSize;

		AuxiliaryFile.Value			= Settings.AuxiliaryFile;
		FieldOrderFile.Value		= Settings.BibEntryInitializationFile;
		FieldQualityFile.Value		= Settings.FieldQualityProcessingFile;
		NameRemappingFile.Value		= Settings.BibEntryRemappingFile;
	}

	#endregion

	#region Validation

	private void AddValidations()
	{
		AuxiliaryFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		AuxiliaryFile.Validations.Add(new FileExistsRule { ValidationMessage = "The file does not exist." });
		ValidateAuxiliaryFile();

		FieldOrderFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		FieldOrderFile.Validations.Add(new FileExistsRule { ValidationMessage = "The file does not exist." });
		ValidateFieldOrderFile();

		FieldQualityFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		FieldQualityFile.Validations.Add(new FileExistsRule { ValidationMessage = "The file does not exist." });
		ValidateFieldQualityFile();

		NameRemappingFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		NameRemappingFile.Validations.Add(new FileExistsRule { ValidationMessage = "The file does not exist." });
		ValidateNameRemappingFile();
	}

	[RelayCommand]
	private void ValidateAuxiliaryFile()
	{
		if (AuxiliaryFile.Validate())
		{
			Settings.AuxiliaryFile = AuxiliaryFile.Value!;
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateFieldOrderFile()
	{
		if (FieldOrderFile.Validate())
		{
			Settings.BibEntryInitializationFile = FieldOrderFile.Value!;
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateFieldQualityFile()
	{
		if (FieldQualityFile.Validate())
		{
			Settings.FieldQualityProcessingFile = FieldQualityFile.Value!;
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateNameRemappingFile()
	{
		if (NameRemappingFile.Validate())
		{
			Settings.BibEntryRemappingFile = NameRemappingFile.Value!;
		}
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable =
		Modified &&
		(!Settings.UseAuxiliaryFile || AuxiliaryFile.IsValid) &&
		(!Settings.UseFieldQualityProcessing || FieldOrderFile.IsValid) &&
		(!Settings.UseFieldQualityProcessing || FieldQualityFile.IsValid) &&
		(!Settings.UseBibEntryRemapping || NameRemappingFile.IsValid);

	#endregion

	#region Events

	private void OnSettingsModifiedChanged(object sender, bool modified) => SetModified(true);

	private void OnSettingsPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) => ValidateSubmittable();

	partial void OnOpenLastProjectAtStartUpChanged(bool value) => SetModified(true);

	partial void OnRemoveNotFoundPathsChanged(bool value) => SetModified(true);

	partial void OnNumberOfItemsShownChanged(int value) => SetModified(true);

	partial void OnNumberOfItemsToStoreChanged(int value) => SetModified(true);

	#endregion

	#region Methods

	private void SetModified(bool modified)
	{
		Modified = modified;
		ValidateSubmittable();
	}

	public void Save()
	{
		// Save the program settings to the preferences. These settings will be applied immediately and also
		// remembered for the next time the program is started.
		Preferences.LoadLastProjectAtStartUp						= OpenLastProjectAtStartUp;
		Preferences.RecentPathsManagerService.RemoveNotFoundPaths	= RemoveNotFoundPaths;
		Preferences.RecentPathsManagerService.NumberOfItemsShown	= (uint)NumberOfItemsShown;
		Preferences.RecentPathsManagerService.MaxSize				= (uint)NumberOfItemsToStore;

		// Save to the preferences which will remember the settings for the next time the program is started.
		// Also save to the project so that the changes are applied immediately.
		Preferences.ProjectSettings			= Settings;
		BibTeXProject.Instance!.Settings	= Settings;
	}

	#endregion
}