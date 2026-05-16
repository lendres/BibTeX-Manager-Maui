using BibTeXLibrary;
using BibTeXManager;

namespace BibTeXManagerUnitTests;

/// <summary>
/// 
/// </summary>
public static class Utilities
{
	#region Fields

	#endregion

	#region Properties

	#endregion

	#region Methods
	
	/// <summary>
	/// Run a FieldProcessor on a BibEntry.
	/// </summary>
	/// <param name="processor">FieldProcessor.</param>
	/// <param name="entry">BibEntry.</param>
	public static void RunProcessor(FieldProcessor processor, BibEntry entry)
	{
		foreach (Correction correction in processor.Process(entry))
		{
			if (correction.PromptUser)
			{
				correction.ReplaceText = true;
			}
		}
	}

	/// <summary>
	/// Run a FieldProcessor on a BibEntry.
	/// </summary>
	/// <param name="processor">FieldProcessor.</param>
	/// <param name="entry">BibEntry.</param>
	public static void RunProcessors(List<FieldProcessor> processors, BibEntry entry)
	{
		foreach (FieldProcessor processor in processors)
		{
			RunProcessor(processor, entry);
		}
	}

	#endregion

} // End class.