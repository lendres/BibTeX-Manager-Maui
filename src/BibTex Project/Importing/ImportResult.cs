using BibTeXLibrary;
using DigitalProduction.Xml.Serialization;
using System.Xml.Serialization;

namespace BibTeXManager.Project;

/// <summary>
/// 
/// </summary>
[XmlRoot("ImportResult")]
public class ImportResult
{
	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public ImportResult(ResultType resultType, BibEntry? bibEntry, string message)
	{
		Result			= resultType;
		BibEntry		= bibEntry;
		Message			= message;
	}

	#endregion

	#region Properties

	/// <summary>
	/// The result type.
	/// </summary>
	public ResultType Result { get; set; } = ResultType.NotAttempted;

	/// <summary>
	/// The BibEntry.
	/// </summary>
	public BibEntry? BibEntry { get; set; } = null;

	/// <summary>
	/// A message, if one is supplied.
	/// </summary>
	public string Message { get; set; } = string.Empty;

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
	public static ImportResult? Deserialize(string path)
	{
		return Serialization.DeserializeObject<ImportResult>(path);
	}

	#endregion

} // End class.