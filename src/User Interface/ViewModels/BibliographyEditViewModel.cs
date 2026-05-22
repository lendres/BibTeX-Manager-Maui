using BibTeXLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Enums;

namespace BibTeXManager.ViewModels;

public partial class BibliographyEditViewModel : BibiographyPartDataGridBaseViewModel<BibEntry>
{
	#region Construction

	public BibliographyEditViewModel()
	{
		ProjectInitialization();
	}

	void ProjectInitialization()
	{
		Project.ModifiedChanged	+= OnProjectModifiedChanged;
		Project.PropertyChanged	+= OnProjectPropertyChanged;
		Project.Opened			+= OnProjectOpenChanged;
		Project.Closed			+= OnProjectOpenChanged;
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial bool		HasTemplates	{ get; set; } = false;

	#endregion

	#region Validation

	private void ValidateHasTemplates()
	{
		HasTemplates = Project.IsOpen && BibTeXProject.Instance?.BibEntryInitialization.TemplateNames.Count > 0;
	}

	#endregion

	#region Events

	private void OnProjectModifiedChanged(object sender, bool modified)
	{
		ValidateHasTemplates();
	}

	private void OnProjectPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs eventArgs)
	{
		ValidateHasTemplates();
	}

	private void OnProjectOpenChanged()
	{
		ValidateHasTemplates();
	}

	#endregion

	#region File Menu

	protected override void AddItems() => Items = Project.Bibliography.Entries;

	#endregion

	#region Commands

	[RelayCommand]
	private void CopyCiteKeyToClipboard()
	{
		System.Diagnostics.Debug.Assert(SelectedItem != null);
		Clipboard.Default.SetTextAsync(SelectedItem.Key);
	}

	#endregion

	#region DataGridBaseViewModel Overrides

	public override void Insert(BibEntry item, int position = 0, bool select = true)
	{
		if (Project.Settings.SortBibliography)
		{
			// If sorting, ignore the position and add based on the sort method.
			Project.Bibliography.Insert(item, Project.Settings.BibliographySortMethod);
		}
		else
		{

			if (position == 0)
			{
				// If we are adding new (position == 0) and not sorting, add to the end of the list.
				Project.Bibliography.Add(item);
			}
			else
			{
				// If we are not sorting, then add at the specified position.
				Project.Bibliography.Insert(item, position);
			}
		}

		FinalizeInsert(item, select);
	}

	/// <summary>
	/// Searches the bibliography for the specified search string in the author and title fields.
	/// </summary>
	/// <param name="search">Search term.</param>
	/// <returns>True if at least one BibEntry is found, false if no entries are found.</returns>
	public override SearchResult Find(string search)
	{
		List<string> tagNames		= ["author", "title"];
		List<BibEntry> findResults	= Project.Bibliography.SearchBibEntries(tagNames, true, search);
		return SetSearchResults(search, findResults);
	}

	#endregion
}