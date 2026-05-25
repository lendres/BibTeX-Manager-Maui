using DigitalProduction.Xml.Serialization;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace BibTeXManager;

/// <summary>
/// A class for mapping BibEntry data.
/// </summary>
[XmlRoot("bibliographyentrymap")]
public class BibliographyEntryMap
{
	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public BibliographyEntryMap()
	{
	}

	/// <summary>
	/// Copy constructor.
	/// </summary>
	public BibliographyEntryMap(BibliographyEntryMap other)
	{
		Name			= other.Name;
		ToType			= other.ToType;
		FieldNameMaps	= new ObservableCollection<FieldNameMap>(other.FieldNameMaps);
	}

	#endregion

	#region Properties

	[XmlAttribute("name")]
	public string Name { get; set; } = "";

	[XmlAttribute("totype")]
	public string ToType { get; set; } = "";
	
	[XmlArray("fieldmaps"), XmlArrayItem("fieldmap")]
	public ObservableCollection<FieldNameMap> FieldNameMaps { get; set; } = new();

	#endregion

} // End class.