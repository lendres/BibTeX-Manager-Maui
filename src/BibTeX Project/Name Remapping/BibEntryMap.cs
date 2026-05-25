using DigitalProduction.Xml.Serialization;
using System.Xml.Serialization;

namespace BibTeXManager;

/// <summary>
/// A class for mapping BibEntry data.
/// </summary>
[XmlRoot("bibliographyentrymap")]
public class BibEntryMap
{
	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public BibEntryMap()
	{
	}

	/// <summary>
	/// Copy constructor.
	/// </summary>
	public BibEntryMap(BibEntryMap other)
	{
		Name			= other.Name;
		ToType			= other.ToType;
		FieldNameMaps	= new SerializableDictionary<string, string>(other.FieldNameMaps);
	}

	#endregion

	#region Properties

	[XmlAttribute("name")]
	public string Name { get; set; } = "";

	[XmlAttribute("totype")]
	public string ToType { get; set; } = "";
	
	[XmlElement("fieldmaps")]
	public SerializableDictionary<string, string> FieldNameMaps { get; set; } = new SerializableDictionary<string, string>();

	#endregion

} // End class.