using BibTeXLibrary;

namespace BibTeXManager;

/// <summary>
/// 
/// </summary>
public abstract class SingleImporter : ImporterBase, ISingleImporter
{
	#region Fields

	#endregion

	#region Construction

	/// <summary>	
	/// Default constructor.
	/// </summary>
	public SingleImporter()
	{
	}

	#endregion

	#region Properties

	#endregion

	#region Protected Methods

	#endregion

	#region Interface Methods

	/// <summary>
	/// Import a single entry from a search string.
	/// </summary>
	/// <param name="searchString">String containing search terms.</param>
	public abstract BibEntry Import(string searchString);

	#endregion

} // End class.