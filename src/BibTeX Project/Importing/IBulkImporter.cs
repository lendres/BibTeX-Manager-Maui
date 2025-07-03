using BibTeXLibrary;
using BibTeXManager.Importing;
using BibTeXManager.Project;

namespace BibTeXManager;

public interface IBulkImporter
{
	ImportErrorHandlingType Continue { get; set; }

	void SetBibliographyInitialization(bool useBibEntryInitialization, BibEntryInitialization bibEntryInitialization);

	/// <summary>
	/// Bulk SPE paper search and import.
	/// </summary>
	IEnumerable<ImportResult> BulkImport();
}