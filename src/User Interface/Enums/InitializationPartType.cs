
using System.ComponentModel;

/// <summary>
/// Main parts of the Bibliography.
/// 
/// These need to be in the same order as they are in the file, so that they can be used to index the vaiable in
/// the MainView.
/// 
/// The "Description" attribute can be accessed using Reflection to get a string representing the enumeration type.
/// </summary>
namespace BibTeXManager.Enums;

public enum InitializationPartType
{
	/// <summary>Type to teomplate maps.</summary>
	[Description("Type Maps")]
	TypeMaps,

	/// <summary>Templates.</summary>
	[Description("Templates")]
	Templates
}