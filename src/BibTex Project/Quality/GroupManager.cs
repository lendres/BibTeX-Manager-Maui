using DigitalProduction.Xml.Serialization;
using System.Xml.Serialization;

namespace BibtexManager;


[XmlRoot("qualityprocessor")]
public class GroupManager
{

	#region Fields

	private const string	FieldQualityManagerExtension			= ".qlty";
	private const string	FieldQualityProcessingGroupExtension	= ".fqpg";
	private string			_path									= string.Empty;

	#endregion

	#region Properties

	[XmlArray("fieldprocessorgroups"), XmlArrayItem("include", Namespace = "http://www.w3.org/2001/XInclude")]
	public List<XInclude> Includes { get; set; } = new();

	[XmlIgnore]
	public List<string> IncludeNames
	{
		get
		{
			return Includes
				.Where(include => !string.IsNullOrWhiteSpace(include.Href))
				.Select(include => Path.GetFileNameWithoutExtension(include.Href))
				.ToList();
		}

		set
		{
			Includes = value?
				.Where(name => !string.IsNullOrWhiteSpace(name))
				.Select(name => new XInclude
				{
					Href = Path.ChangeExtension(name, FieldQualityProcessingGroupExtension)
				})
				.ToList()
				?? [];
		}
	}

	#endregion

	#region Methods

	public List<string> GetAvailableQualityFiles()
	{
		string? directory			= Path.GetDirectoryName(_path);
		string inputFileNameRoot	= Path.GetFileNameWithoutExtension(_path);
		

		if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
		{
			return [];
		}

		return Directory
			.GetFiles(directory, $"*{FieldQualityProcessingGroupExtension}")
			.Select(Path.GetFileNameWithoutExtension)
			.Where(fileName => !string.IsNullOrWhiteSpace(fileName))
			.Where(fileName => !fileName!.Equals(inputFileNameRoot, StringComparison.OrdinalIgnoreCase))
			.OrderBy(fileName => fileName)
			.ToList()!;
	}

	#endregion

	#region XML

	/// <summary>
	/// Write this object to a file to the provided path.
	/// </summary>
	/// <param name="path">Path (full path and filename) to write to.</param>
	/// <exception cref="InvalidOperationException">Thrown when the projects path is not valid.</exception>
	public void Serialize()
	{
		System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(_path), "The path must be set before serialization.");
		Serialize(_path);
	}

	/// <summary>
	/// Write this object to a file to the provided path.
	/// </summary>
	/// <param name="path">Path (full path and filename) to write to.</param>
	/// <exception cref="InvalidOperationException">Thrown when the projects path is not valid.</exception>
	public void Serialize(string path)
	{
		_path = path;
		if (!DigitalProduction.IO.Path.PathIsWritable(path))
		{
			throw new InvalidOperationException("The file cannot be saved. A valid path must be specified.");
		}
		Serialization.SerializeObject(this, path);
	}

	/// <summary>
	/// Deserialize an object from a file.
	/// </summary>
	/// <typeparam name="T">Type of object to deserialize.</typeparam>
	/// <param name="file">File to deserialize from.</param>
	public static GroupManager? Deserialize(string file)
	{
		GroupManager? groupManager = Serialization.DeserializeWithoutIncluding<GroupManager>(file);
		if (groupManager != null)
		{
			groupManager._path = file;
		}
		return groupManager;
	}



	#endregion
}