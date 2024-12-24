using BibTeXLibrary;
using BibtexManager.Importing;
using BibtexManager.Project;

namespace BibtexManager;

public interface IBulkImporter
{
	ImportErrorHandlingType Continue { get; set; }

	void SetBibliographyInitialization(bool useBibEntryInitialization, BibEntryInitialization bibEntryInitialization);

	/// <summary>
	/// Bulk SPE paper search and import.
	/// </summary>
	IEnumerable<ImportResult> BulkImport();
}