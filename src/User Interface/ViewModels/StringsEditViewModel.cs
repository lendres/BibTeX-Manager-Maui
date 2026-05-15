using BibTeXLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.ViewModels;

namespace BibTeXManager.ViewModels;

public partial class StringsEditViewModel : BibiographyPartDataGridBaseViewModel<StringEntry>
{
	#region Construction

	public StringsEditViewModel()
    {
		Items = Project.Bibliography.StringConstants;
	}

	#endregion

	#region Methods

	protected override void AddItems() => Items = Project.Bibliography.StringConstants;

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