using BibTeXLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;

namespace BibTeXManager.ViewModels;

public partial class StringConstantViewModel : ObservableObject
{
	#region Fields

	#endregion

	#region Construction

	public StringConstantViewModel()
	{
		StringConstant				= new();
		Title						= "Add String";
		Initialize(null);
	}

	public StringConstantViewModel(StringConstant stringConstant)
    {
		StringConstant				= stringConstant;
		Title						= "Edit String";
		Initialize(stringConstant);
	}

	#endregion

	#region Properties

	public BibTeXProject						Project { get => BibTeXProject.Instance ?? throw new NullReferenceException("Project is null."); }

	[ObservableProperty]
	public partial string						Title { get; set; }

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>	EnteredName { get; set; }				= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>	EnteredValue { get; set; }				= new();

	[ObservableProperty]
	public partial string						Description { get; set; }

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }

	public StringConstant						StringConstant { get; set; }

	#endregion

	#region Methods

	private void Initialize(StringConstant? excludeStringConstant)
	{
		InitializeValues();
		AddValidations(excludeStringConstant);
		ValidateSubmittable();
	}

	private void InitializeValues()
	{
		EnteredName.Value	= StringConstant.Name;
		EnteredValue.Value	= StringConstant.Value;
	}

	private void AddValidations(StringConstant? excludeStringConstant)
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
			StringConstant.Name = EnteredName.Value ?? "";
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateValue()
	{
		if (EnteredValue.Validate())
		{
			StringConstant.Value = EnteredValue.Value ?? "";
		}
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = EnteredName.IsValid && EnteredValue.IsValid;

	#endregion
}