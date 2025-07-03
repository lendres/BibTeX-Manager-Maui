using BibTeXLibrary;
using BibTeXManager.Project;

namespace BibTeXManager;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// Default constructor.
/// </remarks>
public class SpeBulkTitleImporter(string importPath) : BulkImporter, IBulkImporter
{
	#region Fields

	private readonly string _importPath = importPath;

	#endregion

	#region Interface Methods

	/// <summary>
	/// Search for and download the Bibtex entry for an SPE paper.
	/// </summary>
	/// <param name="searchTerms">Terms to search the web for the paper.</param>
	protected override BibEntry? Import(string searchTerms)
	{
		foreach (string? bibTexString in SpeImportUtilities.ArticleSearch(this.HttpClient, searchTerms))
		{
			if (!String.IsNullOrEmpty(bibTexString))
			{
				BibEntry bibEntry = ParseSingleEntryText(bibTexString);

				// Check to see if we found the right bibliography entry by comparing the search terms to the title.
				if (DigitalProduction.Strings.Format.Similarity(bibEntry.Title, searchTerms) > 0.9)
				{
					return bibEntry;
				}
			}
		}
		return null;
	}

	/// <summary>
	/// Bulk SPE paper search and import.
	/// </summary>
	/// <param name="path">The path to a file that contains a list of search strings.</param>
	public override IEnumerable<ImportResult> BulkImport()
	{
		string[] lines = File.ReadAllLines(_importPath);

		foreach (ImportResult importResult in BulkImport(lines))
		{
			yield return importResult;
		}

		// Write the results.
		string outputPath = DigitalProduction.IO.Path.GetFullPathWithoutExtension(_importPath) + "-output.xlsx";
		WriteBulkImportResults(outputPath);
	}

	#endregion

} // End class.