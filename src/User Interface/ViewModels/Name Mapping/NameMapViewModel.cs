using BibTeXLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;

namespace BibTeXManager.ViewModels;

public partial class NameMapViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public NameMapViewModel(List<string> existingNames)
	{
		NameMap	= new();
		Title			= "Add Map";
		Initialize(null, existingNames);
	}

	public NameMapViewModel(NameMap namemap, List<string> existingNames)
    {
		NameMap	= namemap;
		Title			= "Edit Map";
		Initialize(namemap, existingNames);
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

	public NameMap								NameMap { get; set; }

	#endregion

	#region Methods

	private void Initialize(NameMap? namemap, List<string> existingNames)
	{
		InitializeValues();
		AddValidations(namemap, existingNames);
		ValidateSubmittable();
	}

	private void InitializeValues()
	{
		FromName.Value	= NameMap.From;
		ToName.Value	= NameMap.To;
	}

	private void AddValidations(NameMap? namemap, List<string> existingNames)
	{
		FromName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A name is required." });
		FromName.Validations.Add(new IsNotDuplicateStringRule
		{
			ValidationMessage = "The value is already in use.",
			Values = existingNames,
			ExcludeValue = namemap?.From
		});
		ValidateFromName();

		ToName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "An value is required." });
		ValidateToName();
	}

	[RelayCommand]
	protected void ValidateFromName()
	{
		if (FromName.Validate() && NameMap.From != FromName.Value)
		{
			NameMap.From = FromName.Value ?? "";
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	protected virtual void ValidateToName()
	{
		if (ToName.Validate() && NameMap.To != ToName.Value)
		{
			NameMap.To = ToName.Value ?? "";
		}
		ValidateSubmittable();
	}

	public virtual bool ValidateSubmittable() => IsSubmittable = NameMap.Modified && FromName.IsValid && ToName.IsValid;

	#endregion
}