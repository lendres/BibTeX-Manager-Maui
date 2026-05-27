using DigitalProduction.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

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
		FieldNameMaps.CollectionChanged += OnCollectionModifiedChanged;
	}

	/// <summary>
	/// Copy constructor.
	/// </summary>
	public BibliographyEntryMap(BibliographyEntryMap other)
	{
		Name			= other.Name;
		ToType			= other.ToType;
		FieldNameMaps	= new ObservableCollection<NameMap>(other.FieldNameMaps);

		FieldNameMaps.CollectionChanged += OnCollectionModifiedChanged;
		Modified = false;
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

	[XmlArray("fieldmaps"), XmlArrayItem("namemap")]
	public ObservableCollection<NameMap> FieldNameMaps { get; set; } = new();

	public List<string> InUseTypes { get => FieldNameMaps.Select(fieldNameMap => fieldNameMap.From).ToList(); }

	#endregion

	#region Methods

	public override void Save()
	{
		foreach (NameMap fieldNameMap in FieldNameMaps)
		{
			fieldNameMap.Save();
		}
		base.Save();
	}

	private void OnCollectionModifiedChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs eventArgs)
	{
		Modified = true;
	}

	#endregion

} // End class.