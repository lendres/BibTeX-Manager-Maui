using DigitalProduction.Xml.Serialization;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BibTeXManager.Quality;

/// <summary>
/// 
/// </summary>
[XmlRoot("tagprocessorgroup")]
public class TagProcessorGroup
{
	#region Fields

	private string                                  _name					= string.Empty;
	private BindingList<TagProcessor>               _tagProcessors          = [];

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public TagProcessorGroup()
	{
	}

	#endregion

	#region Properties

	/// <summary>
	/// Name of the group.
	/// </summary>
	[XmlAttribute("name")]
	public string Name { get => _name; set => _name = value; }

	/// <summary>
	/// Tag processing groups.
	/// </summary>
	[XmlArray("tagprocessors"), XmlArrayItem("tagprocessor")]
	public BindingList<TagProcessor> TagProcessors { get => _tagProcessors; set => _tagProcessors = value; }

	#endregion

	#region Methods

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

		Serialization.SerializeObject(this, path);
	}

	/// <summary>
	/// Create an instance from a file.
	/// </summary>
	/// <param name="path">The file to read from.</param>
	public static TagProcessorGroup? Deserialize(string path)
	{
		return Serialization.DeserializeObject<TagProcessorGroup>(path);
	}

	#endregion

} // End class.