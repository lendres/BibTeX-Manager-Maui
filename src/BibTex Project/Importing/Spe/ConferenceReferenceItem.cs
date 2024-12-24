namespace BibtexManager;

/// <summary>
/// Class for holding information related to an article.
/// </summary>
public class ConferenceReferenceItem
{
	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public ConferenceReferenceItem()
	{
	}

	#endregion

	#region Properties

	/// <summary>Bibtex reference ID.</summary>
	public string Reference { get; set; } = string.Empty;

	/// <summary>Paper title.</summary>
	public string Title { get; set; } = string.Empty;

	/// <summary>Session name extracted from the website.</summary>
	public string Session {  get; set; } = string.Empty;

	/// <summary>Page searched to find the title (from the list of input URLs).</summary>
	public int Number { get; set; } = 0;

	/// <summary>URL of page searched.</summary>
	public string ConferencePage { get; set; } = string.Empty;

	/// <summary>Paper URL.</summary>
	public string Url { get; set; } = string.Empty;

	#endregion

	#region Methods

	/// <summary>
	/// Generate a list of headers to use for the output file.
	/// </summary>
	public static string[] Headers
	{
		get
		{
			return new string[]
			{
				"Reference",
				"Title",
				"Session",
				"Day",
				"Conference Page",
				"Link"
			};
		}
	}

	/// <summary>
	/// Convert this instaled to an array of strings for saving to the output.
	/// </summary>
	public string[] ToStringArray()
	{
		return new string[]
		{
			Reference,
			Title,
			Session,
			Number.ToString(),
			ConferencePage,
			Url
		};
	}

	#endregion

} // End class.