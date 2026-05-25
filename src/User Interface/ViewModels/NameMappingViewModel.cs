using BibTeXLibrary;
using BibTeXManager;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Enums;
using DigitalProduction.Maui.ViewModels;
using System.Collections.ObjectModel;

namespace BibTeXManager.ViewModels;

public partial class NameMappingViewModel : DataGridBaseViewModel<BibliographyEntryMap>
{
	#region Construction

	public NameMappingViewModel()
	{
		Items = new ObservableCollection<BibliographyEntryMap>(BibTeXProject.Instance!.NameRemapper.Maps.Values);
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial List<string>? TranslationMatrices { get; set; }

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }

	#endregion

	public void Insert(BibliographyEntryMap item)
	{
		BibTeXProject.Instance!.NameRemapper.Maps[item.Name.ToLower()] = item;
		Items.Add(item);
		SelectedItem = item;
	}

	public void ReplaceSelected(BibliographyEntryMap item)
	{
		if (SelectedItem == null)
		{
			return;
		}

		int index = Items.IndexOf(SelectedItem);

		BibTeXProject.Instance!.NameRemapper.Maps.Remove(SelectedItem.Name.ToLower());
		BibTeXProject.Instance!.NameRemapper.Maps[item.Name.ToLower()] = item;

		Items[index] = item;
		SelectedItem = item;
	}

	[RelayCommand]
	public void Delete()
	{
		if (SelectedItem == null)
		{
			return;
		}

		BibTeXProject.Instance!.NameRemapper.Maps.Remove(SelectedItem.Name.ToLower());
		Items.Remove(SelectedItem);
		SelectedItem = null;
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

	public void Save()
	{
		// TODO: Save to file.
	}
}