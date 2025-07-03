using BibTeXLibrary;
using BibTeXManager;

namespace BibTeXManagerUnitTests;

public class QuoteProcessorTests
{
	/// <summary>
	/// Test replacing "text" with ``text''.
	/// </summary>
	[Fact]
	public void ReplaceQuotes()
	{
		string solution = @"The ``quick'' brown fox jumped over the lazy dog.";
		string input = "The \"quick\" brown fox jumped over the lazy dog.";

		BibEntry entry = new() { Title = input };
		QuoteTagProcessor processor = new()
		{
			TagsToProcess = TagsToProcess.All,
			Pattern = "\".*\""
		};

		// Test lower case tag name.
		Utilities.RunProcessor(processor, entry);
		Assert.Equal(solution, entry.Title);
	}

} // End class.