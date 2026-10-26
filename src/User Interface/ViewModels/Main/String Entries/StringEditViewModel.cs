using BibTeXLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;

namespace BibTeXManager.ViewModels;

public partial class StringEditViewModel : ObservableObject
{
	#region Fields

	#endregion

	#region Construction

	public StringEditViewModel()
	{
		StringEntry				= new();
		Title					= "Add String";
		Initialize(null);
	}

	public StringEditViewModel(StringEntry stringEntry)
    {
		StringEntry					= stringEntry;
		Title						= "Edit String";
		Initialize(stringEntry);
	}

	#endregion

	#region Properties

	public BibTeXProject						Project { get => BibTeXProject.Instance ?? throw new NullReferenceException("Project is null."); }

	[ObservableProperty]
	public partial string						Title { get; set; }

	[ObservableProperty]
	public partial ValidatableObject<string>	EnteredName { get; set; }				= new();

	[ObservableProperty]
	public partial ValidatableObject<string>	EnteredValue { get; set; }				= new();

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }

	public StringEntry							StringEntry { get; set; }

	#endregion

	#region Methods

	private void Initialize(StringEntry? excludeStringConstant)
	{
		InitializeValues();
		AddValidations(excludeStringConstant);
		ValidateSubmittable();
	}

	private void InitializeValues()
	{
		EnteredName.Value	= StringEntry.Name;
		EnteredValue.Value	= StringEntry.Value;
	}

	private void AddValidations(StringEntry? excludeStringConstant)
	{
		EnteredName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A name is required." });
		EnteredName.Validations.Add(new IsNotDuplicateStringRule
		{
			ValidationMessage		= "The value is already in use.",
			Values					= Project.Bibliography.GetStringNames(),
			ExcludeValue			= excludeStringConstant?.Name
		});
		ValidateName();

		EnteredValue.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "An value is required." });
		EnteredValue.Validations.Add(new IsNotDuplicateStringRule
		{
			ValidationMessage		= "The value is already in use.",
			Values					= Project.Bibliography.GetStringValues(),
			ExcludeValue			= excludeStringConstant?.Value
		});
		ValidateValue();
	}

	[RelayCommand]
	private void ValidateName()
	{
		if (EnteredName.Validate())
		{
			StringEntry.Name = EnteredName.Value ?? "";
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateValue()
	{
		if (EnteredValue.Validate())
		{
			StringEntry.Value = EnteredValue.Value ?? "";
		}
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = EnteredName.IsValid && EnteredValue.IsValid;

	#endregion
}