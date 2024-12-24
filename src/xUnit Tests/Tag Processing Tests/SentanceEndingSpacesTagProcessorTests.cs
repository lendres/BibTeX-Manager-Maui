using BibTeXLibrary;
using BibtexManager;

namespace BibtexManagerUnitTests;

public class SentanceEndingSpacesTagProcessorTests
{
	#region Fields

	private static string			_pattern			= @"\.+[a-zA-Z]+";
	private static List<string>		_excludePatterns	= new() { "D.C.", "I.e.", "i.e.", "E.g.", "e.g.", "U.S.", "U.S.A.", "A.K.A." };
//{~{U\.S|D\.C|i\.e|I\.e|e\.g|E\.g}}";

	#endregion

	/// <summary>
	/// Base line test to replace text.
	/// </summary>
	[Fact]
	public void BasicEndOfSentance()
	{
		string solution = @"The quick brown fox jumped.  It was over the lazy dog.";
		string input	= @"The quick brown fox jumped.It was over the lazy dog.";

		BibEntry entry			= new BibEntry() { Abstract = input };
		TagProcessor processor	= new SentanceEndingSpacesTagProcessor() { TagsToProcess = TagsToProcess.All };
		processor.Pattern		= _pattern;

		Utilities.RunProcessor(processor, entry);
		Assert.Equal(solution, entry.Abstract);
	}

	[Fact]
	public void ExcludeSpecialPhrases()
	{
		string solution	= @"The fox went to D.C.  I.e., or e.g. the capital.  Of the U.S., A.K.A., the U.S.A.";
		string input	= @"The fox went to D.C.I.e., or e.g. the capital.Of the U.S., A.K.A., the U.S.A.";
		//string solution = @"The fox went to D.C.  I.e., the capital of the U.S.";
		//string input = @"The fox went to D.C.I.e., the capital of the U.S.";

		BibEntry entry								= new BibEntry() { Abstract = input };
		SentanceEndingSpacesTagProcessor processor	= new SentanceEndingSpacesTagProcessor() { TagsToProcess = TagsToProcess.All };
		processor.Pattern							= _pattern;
		processor.ExcludePatterns					= _excludePatterns;

		Utilities.RunProcessor(processor, entry);
		Assert.Equal(solution, entry.Abstract);
	}

} // End class.