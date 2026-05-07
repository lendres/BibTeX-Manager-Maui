using BibTeXLibrary;
using BibTeXManager;

namespace BibTeXManagerUnitTests;

public class StringCaseFieldProcessorTests
{
	/// <summary>
	/// Convert all caps to title case.
	/// </summary>
	[Fact]
	public void ConvertAllCapsToTitle()
	{
		string solution = @"The Quick Brown Fox; Jumped!: Over the Lazy Dog.";
		string input	= @"THE QUICK BROWN FOX; JUMPED!: OVER THE LAZY DOG.";

		StringCaseFieldProcessor processor  = new() { Pattern=@"^[A-Z\s\p{P}]*$",  FieldsToProcess = FieldsToProcess.All };

		BibEntry entry	= new() { Title = input };
		Utilities.RunProcessor(processor, entry);

		Assert.Equal(solution, entry.Title);
	}

} // End class.