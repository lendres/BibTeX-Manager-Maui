using BibTeXLibrary;
using DigitalProduction.Xml.Serialization;
using System.Xml.Serialization;

namespace BibTeXManager;

/// <summary>
/// A class to remap the type and tag names of a bibilography entry.
/// </summary>
[XmlRoot("bibentryremapping")]
public class BibEntryRemapper
{
	#region Fields

	private SerializableDictionary<string, BibEntryMap>		_maps		= new SerializableDictionary<string, BibEntryMap>();

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public BibEntryRemapper()
	{
	}

	#endregion

	#region Properties

	/// <summary>
	/// Bibliography entry maps.
	/// </summary>
	[XmlElement("maps")]
	public SerializableDictionary<string, BibEntryMap> Maps { get => _maps; set => _maps = value; }
	
	#endregion

	#region Methods

	/// <summary>
	/// Remap the type and tag names in a BibEntry.
	/// </summary>
	/// <param name="entry">BibEntry.</param>
	/// <param name="mapName">Name of the map to use.</param>
	public void RemapEntryNames(BibEntry entry)
	{
		if (_maps.ContainsKey(entry.Type.ToLower()))
		{
			BibEntryMap map = _maps[entry.Type.ToLower()];
			entry.Type		= map.ToType;

			// Getting the tag names is a little expensive, so just do it once, outside of the loop.
			List<string> tagNames = entry.TagNames;

			foreach (KeyValuePair<string, string> tagMap in map.TagMaps)
			{
				// Only remap when the key exists.
				if (tagNames.Contains(tagMap.Key))
				{
					entry.RenameTagKey(tagMap.Key, tagMap.Value);
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
	public static BibEntryRemapper? Deserialize(string path)
	{
		return Serialization.DeserializeObject<BibEntryRemapper>(path);
	}

	#endregion

} // End class.