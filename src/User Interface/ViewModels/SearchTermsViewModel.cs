using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;

namespace BibTexManager.ViewModels;

public partial class SearchTermsViewModel : ObservableObject
{
	#region Construction

	public SearchTermsViewModel()
	{
		Initialize();
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial ValidatableObject<string>		SearchTerms { get; set; } = new();

	public string									SearchTermsString { get => SearchTerms.Value ?? throw new Exception("Search terms is null"); }

	[ObservableProperty]
	public partial bool								IsSubmittable { get; set; }

	#endregion

	#region Methods

	private void Initialize()
	{
		AddValidations();
		ValidateSubmittable();
	}

	private void AddValidations()
	{
		SearchTerms.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "At least one search term is required." });
		ValidateSearchTerms();
	}

	[RelayCommand]
	private void ValidateSearchTerms()
	{
		SearchTerms.Validate();
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = SearchTerms.IsValid;

	#endregion
}