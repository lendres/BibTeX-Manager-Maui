using BibTeXLibrary;
using BibtexManager;
using BibtexManager.Project;

namespace BibTexManagerUnitTests;

/// <summary>
/// 
/// </summary>
public class TestBulkImporter : BulkImporter, IBulkImporter
{
	/// <summary>
	/// Default constructor.
	/// </summary>
	public TestBulkImporter()
	{
		this.BibEntryStrings = new string[] { "" };
	}

	public string[] BibEntryStrings { get; set; }

	/// <summary>
	/// Import a single entry from a search string.
	/// </summary>
	/// <param name="searchString">String containing search terms.</param>
	protected override BibEntry? Import(string searchString)
	{
		return ParseSingleEntryText(this.BibEntryStrings[0]);
	}

	/// <summary>
	/// Bulk SPE paper search and import.
	/// </summary>
	public override IEnumerable<ImportResult> BulkImport()
	{
		foreach (string bibString in this.BibEntryStrings)
		{
			yield return new ImportResult(ResultType.Successful, ParseSingleEntryText(bibString), "");
		}
	}

} // End class.