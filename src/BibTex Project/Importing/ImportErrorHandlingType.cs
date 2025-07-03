using System.ComponentModel;

namespace BibTeXManager.Importing;

/// <summary>
/// Add summary here.
/// 
/// The "Description" attribute can be accessed using Reflection to get a string representing the enumeration type.
/// 
/// The "Length" enumeration can be used in loops as a convenient way of terminating a loop that does not have to be changed if
/// the number of items in the enumeration changes.  The "Length" enumeration must be the last item.
/// for (int i = 0; i &lt; (int)EnumType.Length; i++) {...}
/// </summary>
public enum ImportErrorHandlingType
{
	/// <summary>Cancel import.</summary>
	[Description("Cancel")]
	Cancel,

	/// <summary>Skip and continue.</summary>
	[Description("Skip")]
	Skip,

	/// <summary>Try the import again.</summary>
	[Description("Try Again")]
	TryAgain,

	/// <summary>The number of types/items in the enumeration.</summary>
	[Description("Length")]
	Length

} // End enum.