using BibTeXLibrary;
using BibTeXManager;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Enums;
using DigitalProduction.Maui.Validation;
using DigitalProduction.Maui.ViewModels;
using System.Collections.ObjectModel;

namespace BibTeXManager.ViewModels;

public partial class NameMappingViewModel : DataGridBaseViewModel<FieldNameMap>
{
	#region Construction

	public NameMappingViewModel()
	{
		NameMapper = BibTeXProject.Instance!.NameRemapper;
				
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
	public partial bool							IsSubmittable { get; set; }

	#endregion

	#region Commands

	[RelayCommand]
	private void SelectedMappingChanged()
	{
		SelectedBibliographyEntryMap = NameMapper.Maps.TryGetValue(SelectedType!, out BibliographyEntryMap? map) ? map : null;
		if (SelectedBibliographyEntryMap != null)
		{
			Items = NameMapper.Maps[SelectedType!].FieldNameMaps;
		}
		else
		{
			SelectedItem = null;
		}

	}

	#endregion













	/// <summary>
	/// Searches.
	/// </summary>
	/// <param name="search">Search term.</param>
	/// <returns>SearchResult that indicates the outcome of the search.</returns>
	public override SearchResult Find(string search)
	{
		return SearchResult.NoItemsFound;
	}

	public void Save()
	{
		// TODO: Save to file.
	}
}