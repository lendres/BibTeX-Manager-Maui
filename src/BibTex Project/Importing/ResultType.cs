using System.ComponentModel;

namespace BibtexManager.Project;

/// <summary>
/// Add summary here.
/// 
/// The "Description" attribute can be accessed using Reflection to get a string representing the enumeration type.
/// 
/// The "Length" enumeration can be used in loops as a convenient way of terminating a loop that does not have to be changed if
/// the number of items in the enumeration changes.  The "Length" enumeration must be the last item.
/// for (int i = 0; i &lt; (int)EnumType.Length; i++) {...}
/// </summary>
public enum ResultType
{
	/// <summary>An error occured.</summary>
	[Description("Error")]
	Error,

	/// <summary>The import has not been attempted/started.</summary>
	[Description("Not Attempted")]
	NotAttempted,

	/// <summary>The import attempt did not find the bibliography data.</summary>
	[Description("Not Found")]
	NotFound,

	/// <summary>The import attempt was successful.</summary>
	[Description("Successful")]
	Successful,

	/// <summary>The number of types/items in the enumeration.</summary>
	[Description("Length")]
	Length

} // End enum.