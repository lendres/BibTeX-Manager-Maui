using DigitalProduction.Xml.Serialization;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BibTeXManager;

/// <summary>
/// 
/// </summary>
[XmlRoot("fieldprocessorgroup")]
public class FieldProcessorGroup
{
	#region Fields

	private string								_name					= string.Empty;
	private BindingList<FieldProcessor>			_fieldProcessors		= [];

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public FieldProcessorGroup()
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
	/// Field processing groups.
	/// </summary>
	[XmlArray("fieldprocessors"), XmlArrayItem("fieldprocessor")]
	public BindingList<FieldProcessor> FieldProcessors { get => _fieldProcessors; set => _fieldProcessors = value; }

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
	public static FieldProcessorGroup? Deserialize(string path)
	{
		return Serialization.DeserializeObject<FieldProcessorGroup>(path);
	}

	#endregion

} // End class.