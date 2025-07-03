namespace BibTeXManager.Quality;

/// <summary>
/// 
/// </summary>
public class RemoveEnclosingBracesTagProcessor : TagProcessor
{
	#region Fields

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public RemoveEnclosingBracesTagProcessor()
	{
		// Provide a default pattern.  It can be overridden in the input file.
		_pattern = @"^{[\s\S]*}$";
	}

	#endregion

	#region Properties

	#endregion

	#region Methods

	/// <summary>
	/// Gets the replacement string for the input (original) string.
	/// </summary>
	/// <param name="correction">Correction information.</param>
	protected override void ProcessPatternMatch(Correction correction)
	{
		// We need to make sure we don't replace a string with the intended output.  I.e., we have to make sure we
		// don't replace "XXX" with "{XXX}" when the brackets already exist.  The matching (searching) part will find
		// the "XXX" inside of "{XXX}" so we could end up with "{{XXX}}" if we don't check.

		// The replacement string already exists so don't prompt the user and don't replace the text.
		correction.PromptUser   = false;
		correction.ReplaceText  = false;

		// Need to handle cases like:
		// {The quick brown fox.}
		// {[ABC} The quick brown fox.}
		// {The {ABC} quick brown fox.}
		// And ignore cases like:
		// {ABC} The quick brown fox.
		// {ABC} The quick brown {FOX}.

		string fullText = correction.FullText;

		if (fullText[..1] == "{" && fullText.Substring(fullText.Length-1, 1) == "}")
		{
			int braceCount = 1;
			for (int i = 1; i < fullText.Length-1; i++)
			{
				if (fullText[i] == '}')
				{
					braceCount--;
					if (braceCount == 0)
					{ 
						break;
					}
				}

				if (fullText[i] == '{')
				{
					braceCount++;
				}
			}

			if (braceCount > 0)
			{
				// The loop will stop 1 character short of the end.  If we will have an unmatched brace, the starting
				// and ending braces are a matched pair.
				correction.PromptUser		= true;
				correction.ReplaceText		= true;
				correction.ReplacementText	= fullText[1..^1];
			}
		}
	}

	#endregion

} // End class.