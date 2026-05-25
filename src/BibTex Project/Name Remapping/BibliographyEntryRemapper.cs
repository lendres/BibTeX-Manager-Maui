using BibTeXLibrary;
using DigitalProduction.Xml.Serialization;
using System.Xml.Serialization;

namespace BibTeXManager;

/// <summary>
/// A class to remap the type and field names of a bibilography entry.
/// </summary>
[XmlRoot("bibliographyentryremapping")]
public class BibliographyEntryRemapper
{
	#region Fields

	private SerializableDictionary<string, BibliographyEntryMap>		_maps		= new SerializableDictionary<string, BibliographyEntryMap>();

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public BibliographyEntryRemapper()
	{
	}

	#endregion

	#region Properties

	/// <summary>
	/// Bibliography entry maps.
	/// </summary>
	[XmlElement("maps")]
	public SerializableDictionary<string, BibliographyEntryMap> Maps { get => _maps; set => _maps = value; }
	
	#endregion

	#region Methods

	/// <summary>
	/// Remap the type and field names in a BibEntry.
	/// </summary>
	/// <param name="entry">BibEntry.</param>
	/// <param name="mapName">Name of the map to use.</param>
	public void RemapEntryNames(BibEntry entry)
	{
		if (_maps.TryGetValue(entry.Type.ToLower(), out BibliographyEntryMap? map))
		{
			entry.Type = map.ToType;

			// Getting the field names is a little expensive, so just do it once, outside of the loop.
			List<string> fieldNames = entry.FieldNames;

			foreach (KeyValuePair<string, string> nameMap in map.FieldNameMaps)
			{
				// Only remap when the key exists.
				if (fieldNames.Contains(nameMap.Key))
				{
					entry.RenameField(nameMap.Key, nameMap.Value);
				}
			}
		}
	}

	#endregion

	#region XML

	/// <summary>
	/// Write this object to a file to the provided path.
	/// </summary>
	/// <param name="path">Path (full path and filename) to write to.</param>
	/// <exception cref="InvalidOperationException">Thrown when the projects path is not valid.</exception>
	public void Serialize(string path)
	{
		if (!DigitalProduction.IO.Path.PathIsWritable(path))
		{
			throw new InvalidOperationException("The file cannot be saved.  A valid path must be specified.");
		}
		SerializationSettings settings = new SerializationSettings(this, path);
		settings.XmlSettings.NewLineOnAttributes = false;
		Serialization.SerializeObject(settings);
	}

	/// <summary>
	/// Create an instance from a file.
	/// </summary>
	/// <param name="path">The file to read from.</param>
	public static BibliographyEntryRemapper? Deserialize(string path)
	{
		return Serialization.DeserializeObject<BibliographyEntryRemapper>(path);
	}

	#endregion

} // End class.