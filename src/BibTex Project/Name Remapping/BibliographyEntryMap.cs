using DigitalProduction.ComponentModel;
using DigitalProduction.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace BibTeXManager;

/// <summary>
/// A class for mapping BibEntry data.
/// </summary>
[XmlRoot("bibliographyentrymap")]
public class BibliographyEntryMap : NotifyPropertyModifiedChanged
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
	public string Name
	{
		get => GetValueOrDefault(string.Empty);
		set => SetValue(value);
	}

	[XmlAttribute("totype")]
	public string ToType
	{
		get => GetValueOrDefault(string.Empty);
		set => SetValue(value);
	}

	[XmlArray("fieldmaps"), XmlArrayItem("fieldmap")]
	public ObservableCollection<FieldNameMap> FieldNameMaps { get; set; } = new();

	#endregion

} // End class.