using DigitalProduction.Xml.Serialization;
using System.Xml.Serialization;

namespace BibTeXManager;

/// <summary>
/// A class for mapping BibEntry data.
/// </summary>
public class BibEntryMap
{
	/// <summary>
	/// Default constructor.
	/// </summary>
	public BibEntryMap()
	{
	}

	[XmlAttribute("name")]
	public string Name { get; set; } = "";

	[XmlAttribute("totype")]
	public string ToType { get; set; } = "";
	
	[XmlElement("tagmaps")]
	public SerializableDictionary<string, string> TagMaps { get; set; } = new SerializableDictionary<string, string>();

} // End class.