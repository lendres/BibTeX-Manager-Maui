using BibTeXLibrary;
using BibTeXManager;

namespace BibTeXManagerUnitTests;

/// <summary>
/// 
/// </summary>
public class TestImporter : ImporterBase, ISingleImporter
{
	/// <summary>
	/// Default constructor.
	/// </summary>
	public TestImporter()
	{
		this.BibEntryStrings = new string[] { "" };
	}

	public string[] BibEntryStrings { get; set; }


	/// <summary>
	/// Import a single entry from a search string.
	/// </summary>
	/// <param name="searchString">String containing search terms.</param>
	public BibEntry Import(string searchString)
	{
		return ParseSingleEntryText(this.BibEntryStrings[0]);
	}

} // End class.