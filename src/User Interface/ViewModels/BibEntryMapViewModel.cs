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

	public BibEntryMapViewModel(BibEntryMap bibEntryMap)
	{
		BibEntryMap = bibEntryMap;
		Title = "Edit BibEntry Name Map";

		foreach (KeyValuePair<string, string> fieldNameMap in BibEntryMap.FieldNameMaps)
		{
			FieldNameMaps.Add(new FieldNameMapViewModel(fieldNameMap.Key, fieldNameMap.Value));
		}
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string								Title { get; set; }

	[ObservableProperty]
	public partial FieldNameMapViewModel?				SelectedFieldNameMap { get; set; }

	public BibEntryMap									BibEntryMap { get; }

	public ObservableCollection<FieldNameMapViewModel>	FieldNameMaps { get; } = [];

	#region Properties

	[ObservableProperty]
	public partial bool									IsSubmittable { get; set; }

	#endregion

	#endregion

	[RelayCommand]
	private void AddFieldNameMap()
	{
		FieldNameMaps.Add(new FieldNameMapViewModel());
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

		foreach (FieldNameMapViewModel fieldNameMap in FieldNameMaps)
		{
			if (!string.IsNullOrWhiteSpace(fieldNameMap.FromName))
			{
				BibEntryMap.FieldNameMaps[fieldNameMap.FromName] = fieldNameMap.ToName;
			}
		}
	}
}