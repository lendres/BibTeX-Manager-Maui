using BibTeXLibrary;

namespace BibTeXManager;

/// <summary>
/// 
/// </summary>
public class SpeTitleImporter : ImporterBase, ISingleImporter
{
	/// <summary>
	/// Default constructor.
	/// </summary>
	public SpeTitleImporter()
	{
	}

	/// <summary>
	/// Search for and download the Bibtex entry for an SPE paper.
	/// </summary>
	/// <param name="searchTerms">Terms to search the web for the paper.</param>
	public BibEntry? Import(string searchTerms)
	{
		foreach (string? bibTexString in SpeImportUtilities.ArticleSearch(HttpClient, searchTerms))
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

} // End class.