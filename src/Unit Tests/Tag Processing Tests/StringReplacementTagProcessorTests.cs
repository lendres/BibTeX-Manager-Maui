using BibTeXLibrary;
using BibTeXManager;

namespace BibTeXManagerUnitTests;

public class StringReplacementTagProcessorTests
{
	/// <summary>
	/// Base line test to replace text.
	/// </summary>
	[Fact]
	public void ReplaceOnlyInSpecifiedTag()
	{
		string solution = @"The quick brown fox \& quicker red squirrel jumped over the fence \& lazy dog.";
		string input	= @"The quick brown fox &amp; quicker red squirrel jumped over the fence &amp; lazy dog.";

		BibEntry entry								= new() { Title = input, Abstract = input };
		StringReplacementTagProcessor processor		= new()
		{
			TagsToProcess = TagsToProcess.OnlySpecified,
			Pattern     = "&amp;",
			Replacement = @"\&"
		};
		processor.TagNames.Add("abstract");

		Utilities.RunProcessor(processor, entry);

		Assert.Equal(input, entry.Title);
		Assert.Equal(solution, entry.Abstract);
	}

	/// <summary>
	/// Base line test to replace text.
	/// </summary>
	[Fact]
	public void ExcludeTheSpecifiedTag()
	{
		string solution = @"The quick brown fox \& quicker red squirrel jumped over the fence \& lazy dog.";
		string input    = @"The quick brown fox &amp; quicker red squirrel jumped over the fence &amp; lazy dog.";

		BibEntry entry                              = new() { Title = input, Abstract = input };
		StringReplacementTagProcessor processor		= new()
		{
			TagsToProcess = TagsToProcess.ExcludeSpecified,
			Pattern       = "&amp;",
			Replacement   = @"\&"
		};
		processor.TagNames.Add("abstract");

		Utilities.RunProcessor(processor, entry);

		Assert.Equal(solution, entry.Title);
		Assert.Equal(input, entry.Abstract);
	}

	/// <summary>
	/// Tests that all tags are processed and that strings at the end of a line are replaced.
	/// </summary>
	[Fact]
	public void ReplaceAllStringsAtEnd()
	{
		string solution	= @"The quick brown fox \& quicker red squirrel jumped over the fence lazy dog.\&";
		string input	= @"The quick brown fox &amp; quicker red squirrel jumped over the fence lazy dog.&amp;";

		BibEntry entry								= new() { Title = input, Abstract = input };
		StringReplacementTagProcessor processor		= new()
		{
			TagsToProcess = TagsToProcess.All,
			Pattern     = "&amp;",
			Replacement = @"\&"
		};

		Utilities.RunProcessor(processor, entry);

		Assert.Equal(solution, entry.Title);
		Assert.Equal(solution, entry.Abstract);
	}

	/// <summary>
	/// Test that a string will not be replaced if the matched string is a substring of the replacement string.
	/// </summary>
	[Fact]
	public void DontReplaceMatchedSubStrings()
	{
		string solution = @"The quick {Red} fox & quicker {Red} squirrel jumped over the fence & lazy dog.";
		string input	= @"The quick {Red} fox & quicker Red squirrel jumped over the fence & lazy dog.";

		BibEntry entry							= new() { Title = input};
		StringReplacementTagProcessor processor	= new()
		{
			TagsToProcess = TagsToProcess.All,
			Pattern     = "Red",
			Replacement = "{Red}"
		};
		processor.TagNames.Add("title");

		// Test lower case tag name.
		Utilities.RunProcessor(processor, entry);
		Assert.Equal(solution, entry.Title);

		// Test upper case tage name.
		processor.TagNames.Clear();
		processor.TagNames.Add("Title");
		Utilities.RunProcessor(processor, entry);
		Assert.Equal(solution, entry.Title);
	}

	/// <summary>
	/// Test that a string replacement at the start and end of strings.
	/// </summary>
	[Fact]
	public void ReplaceAtStartAndEnd()
	{
		string solution1 = @"The quick {Red} fox & quicker {Red} squirrel jumped over the fence & lazy dog.}";
		string solution2 = @"The quick {Red} fox & quicker {Red} squirrel jumped over the fence & lazy dog.";
		string input = @"{The quick {Red} fox & quicker {Red} squirrel jumped over the fence & lazy dog.}";

		BibEntry entry							= new() { Title = input };
		StringReplacementTagProcessor processor = new()
		{
			TagsToProcess = TagsToProcess.All,
			Pattern = "^{",
			Replacement = ""
		};
		processor.TagNames.Add("title");

		// Test lower case tag name.
		Utilities.RunProcessor(processor, entry);
		Assert.Equal(solution1, entry.Title);

		// Test upper case tage name.
		processor.Pattern = "}$";
		processor.Replacement = "";
		Utilities.RunProcessor(processor, entry);
		Assert.Equal(solution2, entry.Title);
	}

	/// <summary>
	/// Test that a string replacement at the start and end of strings.
	/// </summary>
	[Fact]
	public void ReplaceInternationalSymbols()
	{
		string solution = @"The q{\o}ick.";
		string input = @"The qøick.";

		BibEntry entry							= new() { Title = input };
		StringReplacementTagProcessor processor = new()
		{
			TagsToProcess = TagsToProcess.All,
			Pattern = "ø",
			Replacement = @"{\o}"
		};
		processor.TagNames.Add("title");

		// Test lower case tag name.
		Utilities.RunProcessor(processor, entry);
		Assert.Equal(solution, entry.Title);
	}

} // End class.