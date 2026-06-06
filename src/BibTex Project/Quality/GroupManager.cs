using DigitalProduction.Xml.Serialization;
using System.Xml.Serialization;

namespace BibtexManager;


[XmlRoot("qualityprocessor")]
public class GroupManager
{

	#region Fields

	public const string		FieldQualityManagerExtension			= ".qlty";
	public const string		FieldQualityProcessingGroupExtension	= ".fqpg";
	private string			_path									= string.Empty;

	#endregion

	#region Properties

	public string Directory => Path.GetDirectoryName(_path) ?? throw new Exception("The path must be set before accessing the directory.");

	[XmlArray("fieldprocessorgroups"), XmlArrayItem("include", Namespace = "http://www.w3.org/2001/XInclude")]
	public List<XInclude> Includes { get; set; } = new();

	[XmlIgnore]
	public List<string> IncludeNames
	{
		get
		{
			return Includes
				.Where(include => !string.IsNullOrWhiteSpace(include.Href))
				.Select(include => include.Href)
				.ToList();
		}

		set
		{
			Includes = value?
				.Where(name => !string.IsNullOrWhiteSpace(name))
				.Select(name => new XInclude { Href = name })
				.ToList()
				?? [];
		}
	}

	#endregion

	#region Methods

	public List<string> GetAvailableQualityFiles()
	{
		if (string.IsNullOrWhiteSpace(Directory) || !System.IO.Directory.Exists(Directory))
		{
			return [];
		}

		return System.IO.Directory
			.GetFiles(Directory, $"*{FieldQualityProcessingGroupExtension}")
			.Select(Path.GetFileName)
			.Where(fileName => !string.IsNullOrWhiteSpace(fileName))
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