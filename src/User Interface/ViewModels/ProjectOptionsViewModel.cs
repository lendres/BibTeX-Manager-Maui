using BibTeXLibrary;
using BibTeXManager.Validation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;

namespace BibTeXManager.ViewModels;

public partial class ProjectOptionsViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public ProjectOptionsViewModel(ProjectSettings projectSettings)
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
	public partial bool							UseFieldOrder { get; set; }

	[ObservableProperty]
	public partial ValidatableObject<string>	FieldOrderFile { get; set; }					= new();

	[ObservableProperty]
	public partial bool							UseFieldQuality { get; set; }

	[ObservableProperty]
	public partial ValidatableObject<string>	FieldQualityFile { get; set; }				= new();

	[ObservableProperty]
	public partial bool							UseNameRemapping { get; set; }

	[ObservableProperty]
	public partial ValidatableObject<string>	NameRemappingFile { get; set; }				= new();

	[ObservableProperty]
	public partial WhiteSpace					WhiteSpace { get; set; }					= WhiteSpace.Tab;

	[ObservableProperty]
	public partial bool							AlignFieldValues { get; set; }

	[ObservableProperty]
	public partial bool							SortBibliographyEntries { get; set; }

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }

	public IReadOnlyList<string>				SorByItems { get; set; }					= DigitalProduction.Reflection.Enumerations.GetAllDescriptionAttributesForType<SortBibliographyBy>();

	#endregion

	#region Initialization

	private void Initialize()
	{
		UseRelativePaths		= Settings.UsePathsRelativeToBibFile;
		UseAuxiliaryFile		= Settings.UseAuxiliaryFile;
		AuxiliaryFile.Value		= Settings.AuxiliaryFile;
		UseFieldOrder			= Settings.UseBibEntryInitialization;
		FieldOrderFile.Value	= Settings.BibEntryInitializationFile;
		UseFieldQuality			= Settings.UseFieldQualityProcessing;
		FieldQualityFile.Value	= Settings.FieldQualityProcessingFile;
		UseNameRemapping        = Settings.UseBibEntryRemapping;
		NameRemappingFile.Value	= Settings.BibEntryRemappingFile;
		WhiteSpace				= Settings.WriteSettings.WhiteSpace;
		AlignFieldValues		= Settings.WriteSettings.AlignFieldValues;
		SortBibliographyEntries	= Settings.SortBibliography;
	}

	public void Save()
	{
		Preferences.ProjectSettings = Settings;
	}

	#endregion

	#region Validation

	private void AddValidations()
	{
		AuxiliaryFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		AuxiliaryFile.Validations.Add(new RelativePathExistsRule { ValidationMessage = "The file does not exist." });
		ValidateAuxiliaryFile();

		FieldOrderFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		FieldOrderFile.Validations.Add(new RelativePathExistsRule { ValidationMessage = "The file does not exist." });
		ValidateFieldOrderFile();

		FieldQualityFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		FieldQualityFile.Validations.Add(new RelativePathExistsRule { ValidationMessage = "The file does not exist." });
		ValidateFieldQualityFile();

		NameRemappingFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		NameRemappingFile.Validations.Add(new RelativePathExistsRule { ValidationMessage = "The file does not exist." });
		ValidateNameRemappingFile();
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
	private void ValidateFieldOrderFile()
	{
		SetValidationData(FieldOrderFile);
		if (FieldOrderFile.Validate())
		{
			Settings.BibEntryInitializationFile = FieldOrderFile.Value!;
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateFieldQualityFile()
	{
		SetValidationData(FieldQualityFile);
		if (FieldQualityFile.Validate())
		{
			Settings.FieldQualityProcessingFile = FieldQualityFile.Value!;
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
		(!UseFieldOrder || FieldOrderFile.IsValid) &&
		(!UseFieldQuality || FieldQualityFile.IsValid) &&
		(!UseNameRemapping || NameRemappingFile.IsValid);

	#endregion

	#region Events
	private void OnSettingsModifiedChanged(object sender, bool modified) => ValidateSubmittable();

	private void OnSettingsPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) => ValidateSubmittable();

	partial void OnUseAuxiliaryFileChanged(bool value) => Settings.UseAuxiliaryFile = value;

	partial void OnUseFieldOrderChanged(bool value) => Settings.UseBibEntryInitialization = value;

	partial void OnUseFieldQualityChanged(bool value) => Settings.UseFieldQualityProcessing = value;

	partial void OnUseNameRemappingChanged(bool value) => Settings.UseBibEntryRemapping = value;

	partial void OnWhiteSpaceChanged(WhiteSpace value) => Settings.WriteSettings.WhiteSpace = value;

	partial void OnAlignFieldValuesChanged(bool value) => Settings.WriteSettings.AlignFieldValues = value;

	partial void OnSortBibliographyEntriesChanged(bool value) => Settings.SortBibliography = value;

	#endregion
}