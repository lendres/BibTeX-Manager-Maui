using BibTeXLibrary;
using BibtexManager.Quality;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace BibtexManager;

/// <summary>
/// Base class for tag processors.
/// </summary>
[XmlInclude(typeof(QuoteTagProcessor))]
[XmlInclude(typeof(RemoveEnclosingBracesTagProcessor))]
[XmlInclude(typeof(SentanceEndingSpacesTagProcessor))]
[XmlInclude(typeof(StringCaseTagProcessor))]
[XmlInclude(typeof(StringReplacementTagProcessor))]
public abstract class TagProcessor
{
	#region Fields

	private TagsToProcess					_tagsToProcess		= TagsToProcess.All;
	private	readonly BindingList<string>	_tagNames			= [];
	protected string						_pattern			= string.Empty;

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public TagProcessor()
	{
	}

	#endregion

	#region Properties

	/// <summary>
	/// Process any tag or just those specified.
	/// </summary>
	[XmlAttribute("tagstoprocess")]
	public TagsToProcess TagsToProcess { get => _tagsToProcess; set => _tagsToProcess = value; }

	/// <summary>
	/// Tag names to process.
	/// </summary>
	[XmlArray("tags"), XmlArrayItem("tag")]
	public BindingList<string> TagNames
	{
		get
		{
			return _tagNames;
		}

		set
		{
			_tagNames.Clear();
			foreach (string tagName in value)
			{
				_tagNames.Add(tagName.ToLower());
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
		foreach (string tagName in entry.TagNames)
		{
			bool processTags = _tagsToProcess switch
			{
				TagsToProcess.All => true,
				TagsToProcess.ExcludeSpecified => !_tagNames.Contains(tagName.ToLower()),
				TagsToProcess.OnlySpecified => _tagNames.Contains(tagName.ToLower()),
				_ => throw new System.Exception("The value for TagsToProcess is out of range."),
			};

			// If we are processing all tags or if the current tag name was specified as one to process.
			// We do a case insensitive comparison of tag names.  See this.TagNames set for where this objects
			// tag names are set to lower case.
			if (processTags)
			{
				foreach (Correction correction in ProcessTag(entry, tagName))
				{
					correction.TagName = tagName;
					yield return correction;
				}
			}
		}
	}

	/// <summary>
	/// Process a single tag.
	/// </summary>
	/// <param name="entry">BibEntry.</param>
	/// <param name="tagName">Name of the tag to process.</param>
	private IEnumerable<Correction> ProcessTag(BibEntry entry, string tagName)
	{
		StringBuilder output	= new();
		string tagValue			= entry[tagName];
		int lastIndex			= 0;

		foreach (Match match in Regex.Matches(tagValue, _pattern))
		{
			if (match.Success && match.Groups.Count > 0)
			{
				Correction correction = new() { FullText = tagValue, MatchedText = match.Value, MatchStartIndex = match.Index };

				// When processing a matched pattern, the TagProcessor can reject the match (Replace=false) and/or specify that the user
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
		if (lastIndex < tagValue.Length)
		{
			output.Append(tagValue.AsSpan(lastIndex));
		}

		entry[tagName] = output.ToString();
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