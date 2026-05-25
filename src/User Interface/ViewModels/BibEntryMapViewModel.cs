using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using DigitalProduction.Maui.Validation;

namespace BibTeXManager.ViewModels;

public partial class BibEntryMapViewModel : ObservableObject
{
	#region Fields

	private readonly string					_previousName						= "";
	private readonly List<string>			_existingNames;

	#endregion

	#region Construction

	public BibEntryMapViewModel(List<string> existingNames)
	{
		BibEntryMap = new();
		Title = "Add BibEntry Name Map";
		_existingNames = existingNames;
		Initialize();
	}

	public BibEntryMapViewModel(BibliographyEntryMap bibEntryMap, List<string> existingNames)
	{
		BibEntryMap = bibEntryMap;
		Title = "Edit BibEntry Name Map";

		_previousName = bibEntryMap.Name;
		_existingNames = existingNames;
		Initialize();

	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string								Title { get; set; }

	[ObservableProperty]
	public partial ValidatableObject<string>			Name  { get; set; }				= new();

	[ObservableProperty]
	public partial FieldNameMap?				SelectedFieldNameMap { get; set; }

	public BibliographyEntryMap									BibEntryMap { get; }

	public ObservableCollection<FieldNameMap>	FieldNameMaps { get; } = [];

	[ObservableProperty]
	public partial bool									IsSubmittable { get; set; }

	#endregion

	private void Initialize()
	{
		InitializeValues();
		AddValidations();
		ValidateSubmittable();
	}

	private void InitializeValues()
	{
		Name.Value = BibEntryMap.Name;
	}

	private void AddValidations()
	{
		Name.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A name is required." });
		Name.Validations.Add(new IsNotDuplicateStringRule
		{
			ValidationMessage = "The value is already in use.",
			Values = _existingNames,
			ExcludeValue = _previousName
		});
		ValidateName();
	}

	[RelayCommand]
	private void ValidateName()
	{
		if (Name.Validate())
		{
			BibEntryMap.Name = Name.Value ?? "";
		}
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = Name.IsValid;

}