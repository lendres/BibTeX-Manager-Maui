using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;

namespace BibTexManager.ViewModels;

public partial class WebSettingsViewModel : ObservableObject
{
	#region Construction

	public WebSettingsViewModel()
	{
		Initialize();
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial ValidatableObject<string>		Identifier { get; set; }		= new();

	[ObservableProperty]
	public partial  ValidatableObject<string>		ApiKey  { get; set; }			= new();

	[ObservableProperty]
	public partial bool								IsSubmittable { get; set; }

	#endregion

	#region Events

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
		Identifier.Value    = Preferences.CustomSearchEngineIdentifier;
		ApiKey.Value		= Preferences.SearchEngineApiKey;
	}

	private void AddValidations()
	{
		Identifier.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "An identified (cx) is required." });
		ValidateIdentifier();

		ApiKey.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "An API key is required." });
		ValidateApiKey();
	}

	[RelayCommand]
	private void ValidateIdentifier()
	{
		if (Identifier.Validate())
		{
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateApiKey()
	{
		if (ApiKey.Validate())
		{
		}
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = Identifier.IsValid && ApiKey.IsValid;

	public void Save()
	{
		Preferences.CustomSearchEngineIdentifier	= Identifier.Value!;
		Preferences.SearchEngineApiKey				= ApiKey.Value!;
	}

	#endregion
}