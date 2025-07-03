using BibTeXLibrary;

namespace BibTeXManager;

/// <summary>
/// Processor to handle bibliography constant strings.
/// </summary>
public class StringConstantProcessor
{
	#region Fields

	private readonly Dictionary<string, string>			_map		= [];

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public StringConstantProcessor()
	{
	}

	#endregion

	#region Properties

	#endregion

	#region Methods

	/// <summary>
	/// Clear values.
	/// </summary>
	public void Clear()
	{
		_map.Clear();
	}

	/// <summary>
	/// Extract and save the String Constants from the document object models.
	/// </summary>
	/// <param name="bibliographyDOMs">List of BibliographyDOMs.</param>
	public void AddStringConstantsToMap(List<BibliographyDOM> bibliographyDOMs)
	{
		foreach (BibliographyDOM bibliographyDOM in bibliographyDOMs)
		{
			AddStringConstantsToMap(bibliographyDOM);
		}
	}

	/// <summary>
	/// Extract and save the String Constants from the document object model.
	/// </summary>
	/// <param name="bibliographyDOMs">BibliographyDOM.</param>
	public void AddStringConstantsToMap(BibliographyDOM bibliographyDOM)
	{
		foreach (StringConstantPart entry in bibliographyDOM.StringConstants)
		{
			_map.Add(entry.Name, entry.Value);
		}
	}

	/// <summary>
	/// Replace text with string constants.
	/// </summary>
	/// <param name="entry">Entry to process for text constant replacements.</param>
	public void ApplyStringConstants(BibEntry entry)
	{
		foreach (KeyValuePair<string, string> pair in _map)
		{
			string key = entry.FindTagValue(pair.Value);
			if (key != "")
			{
				entry.SetTagValue(key, pair.Key, TagValueType.StringConstant);
			}
		}
	}

	#endregion

	#region XML

	#endregion

} // End class.