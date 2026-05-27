using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Enums;
using DigitalProduction.Maui.Validation;
using DigitalProduction.Maui.ViewModels;

namespace BibTeXManager.ViewModels;

public partial class NameMappingViewModel : DataGridBaseViewModel<FieldNameMap>
{
	#region Construction

	public NameMappingViewModel()
	{
		NameMapper = BibTeXProject.Instance!.NameRemapper;
		Initialize();
	}

	#endregion

	#region Properties

	private BibliographyEntryRemapper			NameMapper { get; set; }

	[ObservableProperty]
	public partial List<string>?				BibliographyEntryTypes { get; set; } = BibTeXProject.Instance!.NameRemapper.Maps.Keys.ToList();

	[ObservableProperty]
	public partial string?						SelectedType { get; set; }

	[ObservableProperty]
	public partial BibliographyEntryMap?		SelectedBibliographyEntryMap { get; set; }

	[ObservableProperty]
	public partial ValidatableObject<string>	ToType  { get; set; } = new();

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }

	#endregion

	#region Initialization and Validation

	private void Initialize()
	{
		AddValidations();
		ValidateSubmittable();
	}

	private void AddValidations()
	{
		ToType.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A name is required." });
		ValidateToType();
	}

	[RelayCommand]
	private void ValidateToType()
	{
		if (ToType.Validate())
		{
			if (SelectedBibliographyEntryMap != null)
			{
				SelectedBibliographyEntryMap.ToType = ToType.Value!;
			}
		}
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = Modified && ToType.IsValid;

	#endregion

	#region Events

	partial void OnSelectedBibliographyEntryMapChanged(BibliographyEntryMap? oldValue, BibliographyEntryMap? newValue)
	{
		oldValue?.ModifiedChanged -= OnMapModifiedChanged;
		newValue?.ModifiedChanged += OnMapModifiedChanged;
	}

	private void OnMapModifiedChanged(object sender, bool modified)
	{
		Modified = true;
		ValidateSubmittable();
	}

	#endregion

	#region Commands

	[RelayCommand]
	private void SelectedMappingChanged()
	{
		if (SelectedType == null)
		{
			SelectedBibliographyEntryMap	= null;
			Items							= null;
			SelectedItem					= null;
			return;
		}

		SelectedBibliographyEntryMap = NameMapper.Maps.TryGetValue(SelectedType, out BibliographyEntryMap? map) ? map : null;
		if (SelectedBibliographyEntryMap != null)
		{
			Items = NameMapper.Maps[SelectedType!].FieldNameMaps;
			ToType.Value = SelectedBibliographyEntryMap.ToType;
		}
		else
		{
			// DataGrid selected item.
			SelectedItem = null;
		}

	}

	#endregion

	#region Methods

	public void NewBibliographyEntryMap(string bibliographyEntryType)
	{
		BibliographyEntryMap newMap = new();
		NameMapper.Maps[bibliographyEntryType.ToLower()] = newMap;
		BibliographyEntryTypes = NameMapper.Maps.Keys.ToList();
		SelectedType = bibliographyEntryType;
		SetModified(true);
	}

	public void RenameBibliographyEntryMap(string oldBibliographyEntryType, string newBibliographyEntryType)
	{
		if (NameMapper.Maps.TryGetValue(oldBibliographyEntryType.ToLower(), out BibliographyEntryMap? map))
		{
			NameMapper.Maps.Remove(oldBibliographyEntryType.ToLower());
			NameMapper.Maps[newBibliographyEntryType.ToLower()] = map;
		}
		BibliographyEntryTypes = NameMapper.Maps.Keys.ToList();
		SelectedType = newBibliographyEntryType;
		SetModified(true);
	}

	public void DeleteBibliographyEntryMap(string bibliographyEntryType)
	{
		NameMapper.Maps.Remove(bibliographyEntryType.ToLower());
		BibliographyEntryTypes = NameMapper.Maps.Keys.ToList();
		SelectedType = BibliographyEntryTypes.FirstOrDefault();
		SetModified(true);
	}

	public void Save()
	{
		NameMapper.Serialize();
		SetModified(false);
	}

	/// <summary>
	/// Searches.
	/// </summary>
	/// <param name="search">Search term.</param>
	/// <returns>SearchResult that indicates the outcome of the search.</returns>
	public override SearchResult Find(string search)
	{
		return SearchResult.NoItemsFound;
	}

	private void SetModified(bool modified)
	{
		Modified = modified;
		ValidateSubmittable();
	}

	#endregion
}