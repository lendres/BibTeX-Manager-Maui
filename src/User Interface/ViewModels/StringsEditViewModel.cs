using BibTeXLibrary;

namespace BibTeXManager.ViewModels;

public partial class StringsEditViewModel : BibiographyPartDataGridBaseViewModel<StringEntry>
{
	#region Construction

	public StringsEditViewModel()
    {
	}

	#endregion

	#region Methods

	protected override void AddItems() => Items = Project.Bibliography.StringConstants;

	public override void Insert(StringEntry item, int position = 0, bool select = true)
	{
		if (Project.Settings.SortBibliography)
		{
			// If sorting, ignore the position and add based on the sort method.
			Project.Bibliography.Insert(item, Project.Settings.StringsSortMethod);
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
	public override bool Find(string search)
	{
		List<StringEntry> findResults = Project.Bibliography.SearchStringConstants(true, search);
		return SetSearchResults(search, findResults);
	}

	#endregion

} // End class.