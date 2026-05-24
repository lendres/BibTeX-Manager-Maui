using BibTeXLibrary;
using BibTeXManager;

namespace BibTeXManagerUnitTests;

public class StringReplacementXmlTests
{
	[Theory]
	[InlineData(@"Rock &amp; Roll")]
	[InlineData(@"Rock \&amp; Roll")]
	[InlineData(@"Rock \\&amp; Roll")]
	public void ReadFromXmlReplacesHtmlAmpersandInSpecifiedField(string input)
	{
		string fileName = "Ampersand.qlty";
		string solution = @"Rock \& Roll";
		RunTest(fileName, solution, input);
	}

	private static void RunTest(string fileName, string solution, string input)
	{
		QualityProcessor	qualityProcessor	= CreateProcessor(fileName);
		BibEntry			entry				= new() { Title = input };

		foreach (FieldProcessingData fieldProcessingData in qualityProcessor.Process(entry))
		{
			fieldProcessingData.Correction.ReplaceText = true;
		}

		Assert.Equal(solution, entry.Title);
	}

	private static QualityProcessor CreateProcessor(string fileName)
	{
		string processorFilePath = Path.Combine(AppContext.BaseDirectory, "Test Files", fileName);
		return QualityProcessor.Deserialize(processorFilePath)!;
	}
}