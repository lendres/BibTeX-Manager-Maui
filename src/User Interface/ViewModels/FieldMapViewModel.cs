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

	public FieldMapViewModel(List<string> existingNames)
	{
		FieldNameMap	= new();
		Title			= "Add Field Map";
		Initialize(null, existingNames);
	}

	public FieldMapViewModel(FieldNameMap fieldMap, List<string> existingNames)
    {
		FieldNameMap	= fieldMap;
		Title			= "Edit Field Map";
		Initialize(fieldMap, existingNames);
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

	private void Initialize(FieldNameMap? fieldMap, List<string> existingNames)
	{
		InitializeValues();
		AddValidations(fieldMap, existingNames);
		ValidateSubmittable();
	}

	private void InitializeValues()
	{
		FromName.Value	= FieldNameMap.From;
		ToName.Value	= FieldNameMap.To;
	}

	private void AddValidations(FieldNameMap? fieldMap, List<string> existingNames)
	{
		FromName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A name is required." });
		FromName.Validations.Add(new IsNotDuplicateStringRule
		{
			ValidationMessage = "The value is already in use.",
			Values = existingNames,
			ExcludeValue = fieldMap?.From
		});
		ValidateFromName();

		ToName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "An value is required." });
		ValidateToName();
	}

	[RelayCommand]
	private void ValidateFromName()
	{
		if (FromName.Validate() && FieldNameMap.From != FromName.Value)
		{
			FieldNameMap.From = FromName.Value ?? "";
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateToName()
	{
		if (ToName.Validate() && FieldNameMap.To != ToName.Value)
		{
			FieldNameMap.To = ToName.Value ?? "";
		}
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = FieldNameMap.Modified && FromName.IsValid && ToName.IsValid;

	#endregion
}