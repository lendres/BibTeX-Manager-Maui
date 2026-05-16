using BibTeXLibrary;
using BibTeXManager;

namespace BibTeXManagerUnitTests;

public class QuoteProcessorTests
{
	/// <summary>
	/// Test replacing "text" with ``text''.
	/// </summary>
	[Fact]
	public void ReplaceSingleSetOfQuotes()
	{
		string solution	= @"The ``quick'' brown fox jumped over the lazy dog.";
		string input	= "The \"quick\" brown fox jumped over the lazy dog.";

		BibEntry entry = new() { Title = input };
		QuoteFieldProcessor processor = new()
		{
			FieldsToProcess = FieldsToProcess.All,
			Pattern = "\"[^\"]*\""
		};

		// Test lower case field name.
		Utilities.RunProcessor(processor, entry);
		Assert.Equal(solution, entry.Title);
	}

	/// <summary>
	/// Test replacing "text" with ``text''.
	/// </summary>
	[Fact]
	public void ReplaceTwoSetsOfQuotes()
	{
		string solution	= @"The ``quick'' brown fox ``jumped'' over the lazy dog.";
		string input	= "The \"quick\" brown fox \"jumped\" over the lazy dog.";

		BibEntry entry = new() { Title = input };
		QuoteFieldProcessor processor = new()
		{
			FieldsToProcess = FieldsToProcess.All,
			Pattern = "\"[^\"]*\""
		};

		// Test lower case field name.
		Utilities.RunProcessor(processor, entry);
		Assert.Equal(solution, entry.Title);
	}

	/// <summary>
	/// Test replacing "text" with ``text''.
	/// </summary>
	[Fact]
	public void ReplaceNoSpaceTwoSetsOfQuotes()
	{
		string solution	= @"The ``quick''``brown'' fox jumped over the lazy dog.";
		string input	= "The \"quick\"\"brown\" fox jumped over the lazy dog.";

		BibEntry entry = new() { Title = input };
		QuoteFieldProcessor processor = new()
		{
			FieldsToProcess = FieldsToProcess.All,
			Pattern = "\"[^\"]*\""
		};

		// Test lower case field name.
		Utilities.RunProcessor(processor, entry);
		Assert.Equal(solution, entry.Title);
	}

} // End class.