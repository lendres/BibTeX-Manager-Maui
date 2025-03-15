using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Services;
using System.Collections.ObjectModel;
using DigitalProduction.Maui.Validation;
using CommunityToolkit.Mvvm.Input;
using BibTeXLibrary;
using BibtexManager;

namespace BibTexManager.ViewModels;

public partial class ProjectOptionsViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public ProjectOptionsViewModel(ProjectSettings projectSettings)
	{
		Settings = projectSettings;
		Initialize();
		AddValidations();
		ValidateSubmittable();
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial ProjectSettings						Settings { get; set; }


	[ObservableProperty]
	public partial ValidatableObject<string>			BibliographyFile { get; set; }				= new();

	[ObservableProperty]
	public partial WhiteSpace							WhiteSpace { get; set; }					= WhiteSpace.Tab;

	//[ObservableProperty]
	//public partial int							NumberOfItemsShown { get; set; }

	//[ObservableProperty]
	//public partial int							NumberOfItemsToStore { get; set; }

	//[ObservableProperty]
	//public partial bool							IsSubmittable { get; set; }						= true;

	[ObservableProperty]
	public partial bool								IsSubmittable { get; set; }

	#endregion

	private void Initialize()
	{
		BibliographyFile.Value  = Settings.BibliographyFile;
	}


	#region Validation

	private void AddValidations()
	{
		BibliographyFile.Validations.Add(new IsNotNullOrEmptyRule	{ ValidationMessage = "A file name is required." });
		BibliographyFile.Validations.Add(new FileExistsRule		{ ValidationMessage = "The file does not exist." });
		ValidateBibliographyFile();
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

	public bool ValidateSubmittable() => IsSubmittable = BibliographyFile.IsValid; //&& OutputDirectory.IsValid && OutputFileName.IsValid;

	#endregion

	public void Save()
	{
	}
}