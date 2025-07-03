using BibTeXLibrary;

namespace BibTeXManager;

public interface ISingleImporter
{
	void SetBibliographyInitialization(bool useBibEntryInitialization, BibEntryInitialization bibEntryInitialization);

	/// <summary>
	/// Import a single entry from a search string.
	/// </summary>
	/// <param name="searchString">String containing search terms.</param>
	BibEntry? Import(string searchString);
}