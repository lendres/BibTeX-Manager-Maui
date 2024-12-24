using BibTeXLibrary;
using BibtexManager;

namespace BibtexManagerUnitTests;

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
	/// Run a TagProcessor on a BibEntry.
	/// </summary>
	/// <param name="processor">TagProcessor.</param>
	/// <param name="entry">BibEntry.</param>
	public static void RunProcessor(TagProcessor processor, BibEntry entry)
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
	/// Run a TagProcessor on a BibEntry.
	/// </summary>
	/// <param name="processor">TagProcessor.</param>
	/// <param name="entry">BibEntry.</param>
	public static void RunProcessors(List<TagProcessor> processors, BibEntry entry)
	{
		foreach (TagProcessor processor in processors)
		{
			foreach (Correction correction in processor.Process(entry))
			{
				if (correction.PromptUser)
				{
					correction.ReplaceText = true;
				}
			}
		}
	}

	#endregion

} // End class.