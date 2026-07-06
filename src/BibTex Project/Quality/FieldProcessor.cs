using BibTeXLibrary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace BibTeXManager;

/// <summary>
/// Base class for field processors.
/// </summary>
[XmlInclude(typeof(QuoteFieldProcessor))]
[XmlInclude(typeof(RemoveEnclosingBracesFieldProcessor))]
[XmlInclude(typeof(SentenceEndingSpacesFieldProcessor))]
[XmlInclude(typeof(StringCaseFieldProcessor))]
[XmlInclude(typeof(StringReplacementFieldProcessor))]
public abstract class FieldProcessor
{
	#region Fields

	private FieldsToProcess					_fieldsToProcess	= FieldsToProcess.All;
	private	readonly List<string>			_fieldNames			= [];
	protected string						_pattern			= string.Empty;

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public FieldProcessor()
	{
	}

	#endregion

	#region Properties

	[XmlAttribute("type", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
	public string XsiType { get; set; } = string.Empty;

	/// <summary>
	/// Process any field or just those specified.
	/// </summary>
	[XmlAttribute("fieldstoprocess")]
	public FieldsToProcess FieldsToProcess { get => _fieldsToProcess; set => _fieldsToProcess = value; }

	/// <summary>
	/// Field names to process.
	/// </summary>
	[XmlArray("fields"), XmlArrayItem("field")]
	public List<string> FieldNames
	{
		get
		{
			return _fieldNames;
		}

		set
		{
			_fieldNames.Clear();
			foreach (string fieldName in value)
			{
				_fieldNames.Add(fieldName.ToLower());
			}
		}
	}

	[XmlAttribute("pattern")]
	public string Pattern { get => _pattern; set => _pattern = value; }

	#endregion

	#region Methods

	/// <summary>
	/// Processes all the corrections for a single BibEntry.
	/// </summary>
	/// <param name="entry">BibEntry to process.</param>
	public IEnumerable<Correction> Process(BibEntry entry)
	{
		foreach (string fieldName in entry.FieldNames)
		{
			bool processFields = _fieldsToProcess switch
			{
				FieldsToProcess.All					=> true,
				FieldsToProcess.ExcludeSpecified	=> !_fieldNames.Contains(fieldName.ToLower()),
				FieldsToProcess.OnlySpecified		=> _fieldNames.Contains(fieldName.ToLower()),
				_									=> throw new System.Exception("The value for FieldsToProcess is out of range."),
			};

			// If we are processing all fields or if the current field name was specified as one to process.
			// We do a case insensitive comparison of field names. See FieldNames.set for where this objects
			// field names are set to lower case.
			if (processFields)
			{
				foreach (Correction correction in ProcessField(entry, fieldName))
				{
					correction.FieldName = fieldName;
					yield return correction;
				}
			}
		}
	}

	/// <summary>
	/// Process a single field.
	/// </summary>
	/// <param name="entry">BibEntry.</param>
	/// <param name="fieldName">Name of the field to process.</param>
	private IEnumerable<Correction> ProcessField(BibEntry entry, string fieldName)
	{
		StringBuilder output	= new();
		string fieldValue		= entry[fieldName];
		int lastIndex			= 0;

		foreach (Match match in Regex.Matches(fieldValue, _pattern))
		{
			if (match.Success && match.Groups.Count > 0)
			{
				Correction correction = new() { FullText = fieldValue, MatchedText = match.Value, MatchStartIndex = match.Index };

				// When processing a matched pattern, the FieldProcessor can reject the match (Replace=false) and/or specify that the user
				// does not need to be prompted for this particular match.
				ProcessPatternMatch(correction);

				if (correction.PromptUser)
				{
					yield return correction;
				}

				lastIndex = ProcessCorrectionResult(correction, output, lastIndex);
			}
		}

		// Add the remaining part of the string.
		if (lastIndex < fieldValue.Length)
		{
			output.Append(fieldValue.AsSpan(lastIndex));
		}

		entry[fieldName] = output.ToString();
	}

	/// <summary>
	/// Uses the information provided to update the output string.
	/// </summary>
	/// <param name="correction">Correction data.</param>
	/// <param name="output">Output string that is being built.</param>
	/// <param name="lastIndex">The index position in the string that was last processed.</param>
	protected int ProcessCorrectionResult(Correction correction, StringBuilder output, int lastIndex)
	{
		if (correction.ReplaceText)
		{
			output.Append(correction.FullText[lastIndex..correction.MatchStartIndex]);
			output.Append(correction.ReplacementText);

			lastIndex = correction.MatchStartIndex + correction.MatchedText.Length;
		}

		return lastIndex;
	}

	/// <summary>
	/// Gets the replacement string for the input (original) string.
	/// </summary>
	/// <param name="correction">Correction information.</param>
	protected abstract void ProcessPatternMatch(Correction correction);

	#endregion

} // End class.