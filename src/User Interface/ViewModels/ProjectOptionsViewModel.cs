using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Services;
using System.Collections.ObjectModel;
using DigitalProduction.Maui.Validation;
using CommunityToolkit.Mvvm.Input;
using BibTeXLibrary;
using BibtexManager;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BibTexManager.ViewModels;

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
		ValidateSubmittable();
		Settings.ModifiedChanged += OnSettingsModifiedChanged;
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial ProjectSettings				Settings { get; set; }

	[ObservableProperty]
	public partial ValidatableObject<string>	BibliographyFile { get; set; }				= new();

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
		BibliographyFile.Value	= Settings.BibliographyFile;
		UseTagOrder				= Settings.UseBibEntryInitialization;
		TagOrderFile.Value		= Settings.BibEntryInitializationFile;
		WhiteSpace				= Settings.WriteSettings.WhiteSpace;
		AlignTagValues			= Settings.WriteSettings.AlignTagValues;
		SortBibliographyEntries	= Settings.SortBibliography;
	}

	#endregion

	#region Validation

	private void AddValidations()
	{
		BibliographyFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		BibliographyFile.Validations.Add(new FileExistsRule { ValidationMessage = "The file does not exist." });
		ValidateBibliographyFile();

		TagOrderFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		TagOrderFile.Validations.Add(new FileExistsRule { ValidationMessage = "The file does not exist." });
		ValidateTagOrderFile();

		TagQualityFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		TagQualityFile.Validations.Add(new FileExistsRule { ValidationMessage = "The file does not exist." });
		ValidateTagQualityFile();

		NameRemappingFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		NameRemappingFile.Validations.Add(new FileExistsRule { ValidationMessage = "The file does not exist." });
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
	private void ValidateTagOrderFile()
	{
		if (TagOrderFile.Validate())
		{
			Settings.BibEntryInitializationFile = TagOrderFile.Value!;
		}
		ValidateSubmittable();
	}


	[RelayCommand]
	private void ValidateTagQualityFile()
	{
		if (TagQualityFile.Validate())
		{
			Settings.TagQualityProcessingFile = TagQualityFile.Value!;
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
		Settings.Modified &&
		BibliographyFile.IsValid &&
		(!UseTagOrder || TagOrderFile.IsValid) &&
		(!UseTagQuality || TagQualityFile.IsValid) &&
		(!UseNameRemapping || NameRemappingFile.IsValid);

	#endregion

	#region Events

	private void OnSettingsModifiedChanged(object sender, bool modified)
	{
		ValidateSubmittable();
	}

	partial void OnUseTagOrderChanged(bool value)
	{
		Settings.UseBibEntryInitialization = value;
		ValidateSubmittable();
	}

	partial void OnUseTagQualityChanged(bool value)
	{
		Settings.UseTagQualityProcessing = value;
		ValidateSubmittable();
	}

	partial void OnUseNameRemappingChanged(bool value)
	{
		Settings.UseBibEntryRemapping = value;
		ValidateSubmittable();
	}

	partial void OnWhiteSpaceChanged(WhiteSpace value)
	{
		Settings.WriteSettings.WhiteSpace = value;
		ValidateSubmittable();
	}

	partial void OnAlignTagValuesChanged(bool value)
	{
		Settings.WriteSettings.AlignTagValues = value;
		ValidateSubmittable();
	}

	partial void OnSortBibliographyEntriesChanged(bool value)
	{
		Settings.SortBibliography = value;
		ValidateSubmittable();
	}

	#endregion
}