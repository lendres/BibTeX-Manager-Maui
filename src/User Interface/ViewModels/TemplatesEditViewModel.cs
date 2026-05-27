using BibTeXLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;

namespace BibTeXManager.ViewModels;

public partial class TemplatesEditViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public TemplatesEditViewModel()
	{
		Initialize();
		AddValidations();
		SetModified(false);
	 }

	#endregion

	#region Properties

	private bool								Modified  { get; set; }

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }

	#endregion

	#region Initialization

	private void Initialize()
	{
	}

	#endregion

	#region Validation

	private void AddValidations()
	{
		//AuxiliaryFile.Validations.Add(new ConditionalIsNotNullOrEmptyRule { IsRequired = () => Settings.UseAuxiliaryFile, ValidationMessage = "A file name is required." });
		//AuxiliaryFile.Validations.Add(new ConditionalFileExistsRule { IsRequired = () => Settings.UseAuxiliaryFile, ValidationMessage = "The file does not exist." });
		//ValidateAuxiliaryFile();
	}

	[RelayCommand]
	private void ValidateAuxiliaryFile()
	{
		//if (AuxiliaryFile.Validate())
		//{
		//	Settings.AuxiliaryFile = AuxiliaryFile.Value!;
		//}
		ValidateSubmittable();
	}
	public bool ValidateSubmittable() => IsSubmittable =
		Modified;


	#endregion

	#region Events


	#endregion

	#region Methods

	private void SetModified(bool modified)
	{
		Modified = modified;
		ValidateSubmittable();
	}

	public void Save()
	{
	}

	#endregion
}