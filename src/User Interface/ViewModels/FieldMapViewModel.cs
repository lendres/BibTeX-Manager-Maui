using BibTeXLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;

namespace BibTeXManager.ViewModels;

public partial class FieldMapViewModel : ObservableObject
{
	#region Fields

	#endregion

	#region Construction

	public FieldMapViewModel()
	{
		FieldNameMap			= new();
		Title					= "Add Field Map";
		Initialize(null);
	}

	public FieldMapViewModel(FieldNameMap fieldMap)
    {
		FieldNameMap			= fieldMap;
		Title					= "Edit Field Map";
		Initialize(fieldMap);
	}

	#endregion

	#region Properties

	public BibTeXProject						Project { get => BibTeXProject.Instance ?? throw new NullReferenceException("Project is null."); }

	[ObservableProperty]
	public partial string						Title { get; set; }

	[ObservableProperty]
	public partial ValidatableObject<string>	FromName { get; set; }				= new();

	[ObservableProperty]
	public partial ValidatableObject<string>	ToName { get; set; }				= new();

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }

	public FieldNameMap							FieldNameMap { get; set; }

	#endregion

	#region Methods

	private void Initialize()
	{
		InitializeValues();
		AddValidations();
		ValidateSubmittable();
	}

	private void InitializeValues()
	{
		FromName.Value	= FieldNameMap.From;
		ToName.Value	= FieldNameMap.To;
	}

	private void AddValidations()
	{
		FromName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A name is required." });
		ValidateName();

		ToName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "An value is required." });
		ValidateValue();
	}

	[RelayCommand]
	private void ValidateName()
	{
		if (FromName.Validate())
		{
			FieldNameMap.From = FromName.Value ?? "";
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateValue()
	{
		if (ToName.Validate())
		{
			FieldNameMap.To = ToName.Value ?? "";
		}
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = FieldNameMap.Modified && FromName.IsValid && ToName.IsValid;

	#endregion
}