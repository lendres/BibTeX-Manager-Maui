namespace BibTeXManager;

/// <summary>
/// Data used for processing by a single TagProcessor.
/// </summary>
public class TagProcessingData
{
	#region Fields

	private bool			_acceptAll			= false;
	private Correction		_correction			= new();

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public TagProcessingData()
	{
	}

	#endregion

	#region Properties

	/// <summary>
	/// Accept all default replacements/corrections.
	/// </summary>
	public bool AcceptAll { get => _acceptAll; set => _acceptAll = value; }

	/// <summary>
	/// Correction.
	/// </summary>
	public Correction Correction { get => _correction; set => _correction = value; }

	#endregion

} // End class.