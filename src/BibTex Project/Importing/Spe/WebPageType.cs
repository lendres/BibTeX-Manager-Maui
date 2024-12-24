using System.ComponentModel;

namespace BibtexManager
{
	/// <summary>
	/// Used to determine what type of webpage a search returned.
	/// 
	/// The "Description" attribute can be accessed using Reflection to get a string representing the enumeration type.
	/// 
	/// The "Length" enumeration can be used in loops as a convenient way of terminating a loop that does not have to be changed if
	/// the number of items in the enumeration changes.  The "Length" enumeration must be the last item.
	/// for (int i = 0; i &lt; (int)EnumType.Length; i++) {...}
	/// </summary>
	public enum WebPageType
	{
		/// <summary>The main webpage for an article.</summary>
		[Description("Article Web Page")]
		ArticlePage,

		/// <summary>Webpage that lists all the presentations/papers in a session.</summary>
		[Description("Conference Webpage")]
		ConferencePage,

		/// <summary>Webpage is none of the know types.</summary>
		[Description("Unknown Type of Webpage")]
		Unknown,

		/// <summary>The number of types/items in the enumeration.</summary>
		[Description("Length")]
		Length

	} // End enum.
}