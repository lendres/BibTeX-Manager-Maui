using BibTeXLibrary;
using BibTeXManager.Validation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BibTeXManager.ViewModels;

public partial class ProgramOptionsViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public ProgramOptionsViewModel(ProjectSettings projectSettings)
	{
		Settings = new ProjectSettings(projectSettings);
		Initialize();
		AddValidations();
		Settings.ModifiedChanged += OnSettingsModifiedChanged;
		Settings.PropertyChanged += OnSettingsPropertyChanged;
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial bool							OpenLastProjectAtStartUp { get; set; }		= false;

	[ObservableProperty]
	public partial bool							RemoveNotFoundPaths { get; set; }

	[ObservableProperty]
	public partial int							NumberOfItemsShown { get; set; }

	[ObservableProperty]
	public partial int							NumberOfItemsToStore { get; set; }



	[ObservableProperty]
	public partial ProjectSettings				Settings { get; set; }

	[ObservableProperty]
	public partial bool							UseRelativePaths { get; set; }

	[ObservableProperty]
	public partial ValidatableObject<string>	BibliographyFile { get; set; }				= new();

	[ObservableProperty]
	public partial bool							UseAuxiliaryFile { get; set; }

	[ObservableProperty]
	public partial ValidatableObject<string>	AuxiliaryFile { get; set; }					= new();

	[ObservableProperty]
	public partial bool							UseTagOrder { get; set; }

	[ObservableProperty]
	public partial ValidatableObject<string>	TagOrderFile { get; set; }					= new();

	[ObservableProperty]
	public partial bool							UseTagQuality { get; set; }

	[ObservableProperty]
	public partial ValidatableObject<string>	TagQualityFile { get; set; }				= new();

	[ObservableProperty]
	public partial bool							UseNameRemapping { get; set; }

	[ObservableProperty]
	public partial ValidatableObject<string>	NameRemappingFile { get; set; }				= new();

	[ObservableProperty]
	public partial WhiteSpace					WhiteSpace { get; set; }					= WhiteSpace.Tab;

	[ObservableProperty]
	public partial bool							AlignTagValues { get; set; }

	[ObservableProperty]
	public partial bool							SortBibliographyEntries { get; set; }

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }

	public IReadOnlyList<string>				SorByItems { get; set; }					= DigitalProduction.Reflection.Enumerations.GetAllDescriptionAttributesForType<SortBy>();

	#endregion

	#region Initialization

	private void Initialize()
	{
		OpenLastProjectAtStartUp	= Preferences.LoadLastProjectAtStartUp;
		RemoveNotFoundPaths			= Preferences.RecentPathsManagerService.RemoveNotFoundPaths;
		NumberOfItemsShown			= (int)Preferences.RecentPathsManagerService.NumberOfItemsShown;
		NumberOfItemsToStore		= (int)Preferences.RecentPathsManagerService.MaxSize;

		UseRelativePaths			= Settings.UsePathsRelativeToBibFile;
		BibliographyFile.Value		= Settings.BibliographyFile;
		UseAuxiliaryFile			= Settings.UseAuxiliaryFile;
		AuxiliaryFile.Value			= Settings.AuxiliaryFile;
		UseTagOrder					= Settings.UseBibEntryInitialization;
		TagOrderFile.Value			= Settings.BibEntryInitializationFile;
		UseTagQuality				= Settings.UseBibEntryInitialization;
		TagQualityFile.Value		= Settings.TagQualityProcessingFile;
		UseNameRemapping			= Settings.UseBibEntryRemapping;
		NameRemappingFile.Value		= Settings.BibEntryRemappingFile;
		WhiteSpace					= Settings.WriteSettings.WhiteSpace;
		AlignTagValues				= Settings.WriteSettings.AlignTagValues;
		SortBibliographyEntries		= Settings.SortBibliography;
	}

	#endregion


	#region Validation

	private void AddValidations()
	{
		BibliographyFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		BibliographyFile.Validations.Add(new RelativePathExistsRule { ValidationMessage = "The file does not exist." });
		ValidateBibliographyFile();

		AuxiliaryFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		AuxiliaryFile.Validations.Add(new RelativePathExistsRule { ValidationMessage = "The file does not exist." });
		ValidateBibliographyFile();

		TagOrderFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		TagOrderFile.Validations.Add(new RelativePathExistsRule { ValidationMessage = "The file does not exist." });
		ValidateTagOrderFile();

		TagQualityFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		TagQualityFile.Validations.Add(new RelativePathExistsRule { ValidationMessage = "The file does not exist." });
		ValidateTagQualityFile();

		NameRemappingFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		NameRemappingFile.Validations.Add(new RelativePathExistsRule { ValidationMessage = "The file does not exist." });
		ValidateNameRemappingFile();
	}

	[RelayCommand]
	private void ValidateBibliographyFile()
	{
		if (BibliographyFile.Validate())
		{
			Settings.BibliographyFile = BibliographyFile.Value!;
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateAuxiliaryFile()
	{
		SetValidationData(AuxiliaryFile);
		if (AuxiliaryFile.Validate())
		{
			Settings.AuxiliaryFile = AuxiliaryFile.Value!;
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateTagOrderFile()
	{
		SetValidationData(TagOrderFile);
		if (TagOrderFile.Validate())
		{
			Settings.BibEntryInitializationFile = TagOrderFile.Value!;
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateTagQualityFile()
	{
		SetValidationData(TagQualityFile);
		if (TagQualityFile.Validate())
		{
			Settings.TagQualityProcessingFile = TagQualityFile.Value!;
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateNameRemappingFile()
	{
		SetValidationData(NameRemappingFile);
		if (NameRemappingFile.Validate())
		{
			Settings.BibEntryRemappingFile = NameRemappingFile.Value!;
		}
		ValidateSubmittable();
	}

	private void SetValidationData(ValidatableObject<string> validationObject)
	{
		RelativePathExistsRule rule = (RelativePathExistsRule)validationObject.Validations[1];
		rule.UsingRelativePaths	= UseRelativePaths;
		rule.MainPath           = BibliographyFile.Value!;
	}

	public bool ValidateSubmittable() => IsSubmittable =
		Settings.Modified &&
		BibliographyFile.IsValid &&
		(!UseAuxiliaryFile || AuxiliaryFile.IsValid) &&
		(!UseTagOrder || TagOrderFile.IsValid) &&
		(!UseTagQuality || TagQualityFile.IsValid) &&
		(!UseNameRemapping || NameRemappingFile.IsValid);

	#endregion

	#region Events

	private void OnSettingsModifiedChanged(object sender, bool modified) => ValidateSubmittable();

	private void OnSettingsPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) => ValidateSubmittable();

	partial void OnUseAuxiliaryFileChanged(bool value) => Settings.UseAuxiliaryFile = value;

	partial void OnUseTagOrderChanged(bool value) => Settings.UseBibEntryInitialization = value;

	partial void OnUseTagQualityChanged(bool value) => Settings.UseTagQualityProcessing = value;

	partial void OnUseNameRemappingChanged(bool value) => Settings.UseBibEntryRemapping = value;

	partial void OnWhiteSpaceChanged(WhiteSpace value) => Settings.WriteSettings.WhiteSpace = value;

	partial void OnAlignTagValuesChanged(bool value) => Settings.WriteSettings.AlignTagValues = value;

	partial void OnSortBibliographyEntriesChanged(bool value) => Settings.SortBibliography = value;

	#endregion

	#region Methods

	public void Save()
	{
		Preferences.LoadLastProjectAtStartUp						= OpenLastProjectAtStartUp;
		Preferences.RecentPathsManagerService.RemoveNotFoundPaths	= RemoveNotFoundPaths;
		Preferences.RecentPathsManagerService.NumberOfItemsShown	= (uint)NumberOfItemsShown;
		Preferences.RecentPathsManagerService.MaxSize				= (uint)NumberOfItemsToStore;
	}

	#endregion

}