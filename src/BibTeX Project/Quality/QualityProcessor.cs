using BibTeXLibrary;
using BibTeXManager.Quality;
using DigitalProduction.Xml.Serialization;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BibTeXManager;

/// <summary>
/// 
/// </summary>
[XmlRoot("qualityprocessor")]
public class QualityProcessor
{
	#region Fields

	private BindingList<TagProcessorGroup>			_tagProcessorGroups			= [];

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public QualityProcessor()
	{
	}

	#endregion

	#region Properties

	[XmlArray("tagprocessorgroups"), XmlArrayItem("tagprocessorgroup")]
	public BindingList<TagProcessorGroup> TagProcessorGroups { get => _tagProcessorGroups; set => _tagProcessorGroups = value; }

	#endregion

	#region Methods

	/// <summary>
	/// Process a BibEntry and correct errors.
	/// </summary>
	/// <param name="entry">BibEntry to process and clean.</param>
	public IEnumerable<TagProcessingData> Process(BibEntry entry)
	{
		TagProcessingData tagProcessingData = new();
		foreach (TagProcessorGroup tagProcessorGroup in _tagProcessorGroups)
		{
			foreach (TagProcessor processor in tagProcessorGroup.TagProcessors)
			{
				foreach (Correction correction in processor.Process(entry))
				{
					tagProcessingData.Correction = correction;
					if (tagProcessingData.AcceptAll)
					{
						correction.ReplaceText = true;
					}
					else
					{
						yield return tagProcessingData;
					}
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
		Serialization.SerializeObject(this, path);
	}

	/// <summary>
	/// Create an instance from a file.
	/// </summary>
	/// <param name="path">The file to read from.</param>
	public static QualityProcessor? Deserialize(string path)
	{
		return Serialization.DeserializeObject<QualityProcessor>(path);
	}

	#endregion

} // End class.