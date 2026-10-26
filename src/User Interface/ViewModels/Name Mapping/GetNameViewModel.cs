using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using DigitalProduction.Maui.Validation;

namespace BibTeXManager.ViewModels;

public partial class GetNameViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public GetNameViewModel(List<string> existingNames)
	{
		Title = "Name";
		Initialize(null, existingNames);
	}

	public GetNameViewModel(string currentName, List<string> existingNames)
	{
		Title = "Rename";
		Initialize(currentName, existingNames);
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string								Title { get; set; }

	public string										Name  { get => NameValidator.Value == null ? throw new Exception("Name is null") : NameValidator.Value; }

	[ObservableProperty]
	public partial ValidatableObject<string>			NameValidator  { get; set; } = new();

	[ObservableProperty]
	public partial bool									IsSubmittable { get; set; }

	#endregion

	#region Initialization and Validation

	private void Initialize(string? currentName, List<string> existingNames)
	{
		InitializeValues(currentName);
		AddValidations(currentName, existingNames);
		ValidateSubmittable();
	}

	private void InitializeValues(string? currentName)
	{
		NameValidator.Value = currentName;
	}

	private void AddValidations(string? currentName, List<string> existingNames)
	{
		NameValidator.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A name is required." });
		NameValidator.Validations.Add(new IsNotDuplicateStringRule
		{
			ValidationMessage	= "The value is already in use.",
			Values				= existingNames,
			ExcludeValue		= currentName
		});
		ValidateName();
	}

	[RelayCommand]
	private void ValidateName()
	{
		NameValidator.Validate();
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = NameValidator.IsValid;

	#endregion
}