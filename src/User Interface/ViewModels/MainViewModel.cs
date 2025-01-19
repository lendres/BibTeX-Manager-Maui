using BibTeXLibrary;
using BibtexManager;
using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Projects;

namespace BibTexManager.ViewModels;

public partial class MainViewModel : DataGridBaseViewModel<BibEntry>
{
	#region Fields

	ProjectExtractor?				_projectExtractor		= null;
	BibtexProject?					_project				= null;

	[ObservableProperty]
	private bool					_isSubmittable			= false;

	#endregion

	#region Construction

	public MainViewModel()
    {
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

	private void SaveSettings()
	{
		//Preferences.XmlInputFile		= XmlInputFile.Value!.Trim();
	}

	public void OpenProject(string projectFile)
	{
		BibtexProject.Deserialize(projectFile);
		if (BibtexProject.Instance != null)
		{
			Items = BibtexProject.Instance.Bibliography.Entries;
		}

	}

	#endregion

} // End class.