using BibTeXLibrary;
using BibtexManager.Importing;
using BibtexManager.Project;
using ClosedXML.Excel;

namespace BibtexManager;

/// <summary>
/// 
/// </summary>
public abstract class BulkImporter : ImporterBase
{
	#region Fields

	private ImportErrorHandlingType				_importErrorHandling			= ImportErrorHandlingType.TryAgain;
	private readonly List<string[]>				_bulkImportResults				= new List<string[]>();

	#endregion

	#region Construction

	/// <summary>	
	/// Default constructor.
	/// </summary>
	public BulkImporter()
	{
	}

	#endregion

	#region Properties

	public ImportErrorHandlingType Continue { get => _importErrorHandling; set => _importErrorHandling=value; }

	#endregion

	#region Interface Methods

	/// <summary>
	/// Import a single entry from a search string.
	/// </summary>
	/// <param name="searchString">String containing search terms.</param>
	protected abstract BibEntry? Import(string searchString);

	/// <summary>
	/// Bulk SPE paper search and import.
	/// </summary>
	public abstract IEnumerable<ImportResult> BulkImport();

	#endregion

	#region Protected Methods

	/// <summary>
	/// Bulk SPE paper search and import.
	/// </summary>
	/// <param name="path">The path to a file that contains a list of search strings.</param>
	protected IEnumerable<ImportResult> BulkImport(string[] lines)
	{
		foreach (string searchString in lines)
		{
			_importErrorHandling = ImportErrorHandlingType.TryAgain;

			ImportResult importResult;
			do
			{
				importResult = ImportTry(searchString);

				if (importResult.Result == ResultType.Error)
				{
					yield return importResult;
				}
			}
			while (importResult.Result==ResultType.Error & _importErrorHandling==ImportErrorHandlingType.TryAgain);

			if (_importErrorHandling == ImportErrorHandlingType.Cancel)
			{
				break;
			}

			yield return importResult;

			FormatAndSaveResult(searchString, importResult);
		}
	}

	/// <summary>
	/// Default saving of an Import result.  This can be overridden if a subclass needs to provide a different save format.
	/// </summary>
	/// <param name="searchString">String provided to the Import method.</param>
	/// <param name="importResult">Results of the import.</param>
	protected virtual void FormatAndSaveResult(string searchString, ImportResult importResult)
	{
		if (importResult.BibEntry is null)
		{
			// A bibliography entry was not found.
			SaveResult(new string[] { "", "", searchString });
		}
		else
		{
			// A bibliography entry was found and returned.
			SaveResult(
				new string[]
				{ 
					importResult.BibEntry.Key,
					importResult.BibEntry.Title,
					searchString
				}
			);
		}
	}

	/// <summary>
	/// Adds the result to the list of saved results.
	/// </summary>
	/// <param name="resultArray">An array of strings to save as a single line item.</param>
	protected void SaveResult(string[] resultArray)
	{
		_bulkImportResults.Add(resultArray);
	}

	#endregion

	#region Methods

	/// <summary>
	/// Trys to do an import and catches an errors that occur.
	/// </summary>
	/// <param name="searchString">Search or other information to provide to the "Import" method.</param>
	protected ImportResult ImportTry(string searchString)
	{
		try
		{
			BibEntry bibEntry       = Import(searchString);
			ResultType resultType   = bibEntry==null ? ResultType.NotFound : ResultType.Successful;
			return new ImportResult(resultType, bibEntry, "");
		}
		catch (Exception exception)
		{
			string message = "Error: " + Environment.NewLine + exception.Message + Environment.NewLine + Environment.NewLine + "Search:" + Environment.NewLine + searchString;
			return new ImportResult(ResultType.Error, null, message);
		}
	}

	/// <summary>
	/// Writes the names of the references and titles to a file.
	/// </summary>
	/// <param name="filePath">The path and file name to write to.</param>
	protected void WriteBulkImportResults(string filePath, string[]? headers = null)
	{
		int rowOffset = 1;
		using (XLWorkbook workbook = new XLWorkbook())
		{
			var worksheet = workbook.Worksheets.Add("Sheet 1");

			if (headers != null)
			{
				for (int j = 0; j < headers.Length; j++)
				{
					worksheet.Cell(1, j+1).Value = headers[j];
					worksheet.Cell(1, j+1).Style.Font.Bold = true;
					worksheet.Cell(1, j+1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
				}
				rowOffset++;
			}

			// Write each row of data to the file.
			for (int i = 0; i < _bulkImportResults.Count; i++)
			{
				for (int j = 0; j < _bulkImportResults[i].Length; j++)
				{
					worksheet.Cell(i+rowOffset, j+1).Value = _bulkImportResults[i][j];
				}
			}

			worksheet.Columns().AdjustToContents();

			workbook.SaveAs(filePath);
		}
	}

	#endregion

} // End class.