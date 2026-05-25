using System.Xml.Serialization;

namespace BibTeXManager;

public partial class FieldNameMap
{
	#region Construction

	public FieldNameMap()
	{
	}

	public FieldNameMap(string fromName, string toName)
	{
		From	= fromName;
		To		= toName;
	}

	public FieldNameMap(FieldNameMap other)
	{
		From	= other.From;
		To		= other.To;
	}

	#endregion

	#region Properties

	[XmlAttribute("from")]
	public string From { get; set; } = "";

	[XmlAttribute("to")]
	public string To { get; set; } = "";

	#endregion
}