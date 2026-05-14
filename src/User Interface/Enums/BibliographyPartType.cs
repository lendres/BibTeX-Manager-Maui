using System.ComponentModel;

/// <summary>
/// Main parts of the Bibliography.
/// 
/// The "Description" attribute can be accessed using Reflection to get a string representing the enumeration type.
/// </summary>
namespace BibTeXManager.Enums
{
	public enum BibliographyPartType
	{
		/// <summary>Bibliography entries.</summary>
		[Description("Bibliography Entries")]
		BibliographyEntries,

		/// <summary>Header of the bibliography.</summary>
		[Description("Header")]
		Header,

		/// <summary>String entries.</summary>
		[Description("String Entries")]
		StringEntries
	}
}