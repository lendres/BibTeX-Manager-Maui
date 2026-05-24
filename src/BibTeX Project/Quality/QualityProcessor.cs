using BibTeXLibrary;
using BibTeXManager.Quality;
using DigitalProduction.Xml.Serialization;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BibTeXManager;

/// <summary>
/// Represents a quality processor for cleaning BibTeX entries.
/// </summary>
[XmlRoot("qualityprocessor")]
public class QualityProcessor
{
	#region Fields

	private BindingList<FieldProcessorGroup>			_fieldProcessorGroups			= [];

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

	[XmlArray("fieldprocessorgroups"), XmlArrayItem("fieldprocessorgroup")]
	public BindingList<FieldProcessorGroup> FieldProcessorGroups { get => _fieldProcessorGroups; set => _fieldProcessorGroups = value; }

	#endregion

	#region Methods

	/// <summary>
	/// Process a BibEntry and correct errors.
	/// </summary>
	/// <param name="entry">BibEntry to process and clean.</param>
	public IEnumerable<FieldProcessingData> Process(BibEntry entry)
	{
		FieldProcessingData fieldProcessingData = new();
		foreach (FieldProcessorGroup fieldProcessorGroup in _fieldProcessorGroups)
		{
			foreach (FieldProcessor processor in fieldProcessorGroup.FieldProcessors)
			{
				foreach (Correction correction in processor.Process(entry))
				{
					fieldProcessingData.Correction = correction;
					if (fieldProcessingData.AcceptAll)
					{
						correction.ReplaceText = true;
					}
					else
					{
						yield return fieldProcessingData;
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
			throw new InvalidOperationException("The file cannot be saved. A valid path must be specified.");
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