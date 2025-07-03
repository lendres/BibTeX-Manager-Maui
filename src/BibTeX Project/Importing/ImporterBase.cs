using BibTeXLibrary;

namespace BibTeXManager;

/// <summary>
/// 
/// </summary>
public abstract class ImporterBase
{
	#region Fields

	private bool								_useBibEntryInitialization		= false;
	private BibEntryInitialization				_bibEntryInitialization			= new();

	#endregion

	#region Construction

	/// <summary>	
	/// Default constructor.
	/// </summary>
	public ImporterBase()
	{
	}

	#endregion

	#region Properties

	protected HttpClient HttpClient { get; } = new HttpClient();

	#endregion

	#region Protected Methods

	/// <summary>
	/// Parse a string and return a single BibEntry.
	/// </summary>
	/// <param name="text">Text to process.</param>
	protected BibEntry ParseSingleEntryText(string text)
	{
		StringReader textReader = new(text);
		BibliographyDOM result;

		if (_useBibEntryInitialization)
		{
			result = BibParser.Parse(textReader, _bibEntryInitialization);
		}
		else
		{
			result = BibParser.Parse(textReader);
		}

		return result.Entries[0];
	}

	#endregion

	#region Interface Methods

	public void SetBibliographyInitialization(bool useBibEntryInitialization, BibEntryInitialization bibEntryInitialization)
	{
		_useBibEntryInitialization	= useBibEntryInitialization;
		_bibEntryInitialization     = bibEntryInitialization;
	}

	#endregion

} // End class.