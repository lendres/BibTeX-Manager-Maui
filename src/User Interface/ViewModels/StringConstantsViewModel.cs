using BibTeXLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.ViewModels;

namespace BibTeXManager.ViewModels;

public partial class StringConstantsViewModel : DataGridBaseViewModel<StringEntry>
{
	#region Fields

	#endregion

	#region Construction

	public StringConstantsViewModel()
    {
		Items = Project.Bibliography.StringConstants;
	}

	#endregion

	#region Properties

	public BibTeXProject							Project { get => BibTeXProject.Instance ?? throw new NullReferenceException("Project is null."); }

	[ObservableProperty]
	public partial bool								IsSubmittable { get; set; }					= false;

	#endregion

	#region Validation

	#endregion

	#region Events

	private void OnProjectModifiedChanged(object sender, bool modified)
	{
		Modified = modified;
	}

	#endregion

	#region Methods and Commands

	#region Edit Menu

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

	#endregion

} // End class.