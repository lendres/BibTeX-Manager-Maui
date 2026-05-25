using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BibTeXManager.ViewModels;

public partial class BibEntryMapViewModel : ObservableObject
{
	#region Construction

	public BibEntryMapViewModel()
	{
		BibEntryMap = new();
		Title = "Add BibEntry Name Map";
	}

	public BibEntryMapViewModel(BibliographyEntryMap bibEntryMap)
	{
		BibEntryMap = bibEntryMap;
		Title = "Edit BibEntry Name Map";

		foreach (KeyValuePair<string, string> fieldNameMap in BibEntryMap.FieldNameMaps)
		{
			FieldNameMaps.Add(new FieldNameMap(fieldNameMap.Key, fieldNameMap.Value));
		}
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string								Title { get; set; }

	[ObservableProperty]
	public partial FieldNameMap?				SelectedFieldNameMap { get; set; }

	public BibliographyEntryMap									BibEntryMap { get; }

	public ObservableCollection<FieldNameMap>	FieldNameMaps { get; } = [];

	#region Properties

	[ObservableProperty]
	public partial bool									IsSubmittable { get; set; }

	#endregion

	#endregion

	[RelayCommand]
	private void AddFieldNameMap()
	{
		FieldNameMaps.Add(new FieldNameMap());
	}

	[RelayCommand]
	private void DeleteFieldNameMap()
	{
		if (SelectedFieldNameMap != null)
		{
			FieldNameMaps.Remove(SelectedFieldNameMap);
		}
	}

	public void Save()
	{
		BibEntryMap.FieldNameMaps.Clear();

		foreach (FieldNameMap fieldNameMap in FieldNameMaps)
		{
			if (!string.IsNullOrWhiteSpace(fieldNameMap.FromName))
			{
				BibEntryMap.FieldNameMaps[fieldNameMap.FromName] = fieldNameMap.ToName;
			}
		}
	}
}