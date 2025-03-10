using BibTeXLibrary;
using DigitalProduction.ComponentModel;
using System.Xml.Serialization;

namespace BibtexManager;

public class ProjectSettings : NotifyPropertyModifiedChanged
{
	#region Fields

	private List<string>				_assessoryFiles					= [];
	private WriteSettings				_writeSettings					= new();

	#endregion

	#region Constructions

	public ProjectSettings()
	{
		ModifiedChanged += OnMyModifiedChanged;
		_writeSettings.ModifiedChanged += OnChildModifiedChanged;
	}

	#endregion

	#region Properties

	/// <summary>
	/// Use paths relative to the bibliography file.
	/// </summary>
	[XmlAttribute("userelativepaths")]
	public bool UsePathsRelativeToBibFile
	{
		get => GetValueOrDefault<bool>(false);

		set
		{
			if (SetValue(value))
			{
				Modified = true;
			}
		}
	}

	/// <summary>
	/// Determines if the bibiography entry initialization file.
	/// </summary>
	[XmlAttribute("usebibentryinitialization")]
	public bool UseBibEntryInitialization
	{
		get => GetValueOrDefault<bool>(false);

		set
		{
			if (SetValue(value))
			{
				Modified = true;
			}
		}
	}

	/// <summary>
	/// The path to the bibiography entry initialization file.
	/// </summary>
	[XmlAttribute("bibentryinitializationfile")]
	public string BibEntryInitializationFile
	{
		get => GetValueOrDefault<string>(string.Empty);

		set
		{
			if (SetValue(value))
			{
				Modified = true;
			}
		}
	}

	/// <summary>
	/// The path to the bibiography file.
	/// </summary>
	[XmlAttribute("bibfile")]
	public string BibliographyFile
	{
		get => GetValueOrDefault<string>(string.Empty);
		set
		{
			if (SetValue(value))
			{
				Modified = true;
			}
		}
	}

	/// <summary>
	/// Assessory files that contain things like strings.
	/// </summary>
	[XmlArray("assessoryfiles"), XmlArrayItem("file")]
	public List<string> AssessoryFiles
	{
		get => _assessoryFiles;

		set
		{
			if (!_assessoryFiles.SequenceEqual(value))
			{
				_assessoryFiles = value;
				Modified = true;
				OnPropertyChanged(nameof(AssessoryFiles));
			}
		}
	}

	/// <summary>
	/// Replace tag values with string constants.
	/// </summary>
	[XmlAttribute("usestringconstants")]
	public bool UseStringConstants
	{
		get => GetValueOrDefault<bool>(false);

		set
		{
			if (SetValue(value))
			{
				Modified = true;
			}
		}
	}

	/// <summary>
	/// Specifies if the tags should be processed to ensure their quality.
	/// </summary>
	[XmlAttribute("usetagqualityprocessing")]
	public bool UseTagQualityProcessing
	{
		get => GetValueOrDefault<bool>(false);

		set
		{
			if (SetValue(value))
			{
				Modified = true;
			}
		}
	}

	/// <summary>
	/// The path to the quality processor file.
	/// </summary>
	[XmlAttribute("qualityprocessorfile")]
	public string TagQualityProcessingFile
	{
		get => GetValueOrDefault<string>(string.Empty);

		set
		{
			if (SetValue(value))
			{
				Modified = true;
			}
		}
	}

	/// <summary>
	/// Use BibEntry remapping.
	/// </summary>
	[XmlAttribute("usebibentryremapping")]
	public bool UseBibEntryRemapping
	{
		get => GetValueOrDefault<bool>(false);

		set
		{
			if (SetValue(value))
			{
				Modified = true;
			}
		}
	}

	/// <summary>
	/// The path to the bibliography remapping file.
	/// </summary>
	[XmlAttribute("nameremappingfile")]
	public string BibEntryRemappingFile
	{
		get => GetValueOrDefault<string>(string.Empty);

		set
		{
			if (SetValue(value))
			{
				Modified = true;
			}
		}
	}

	/// <summary>
	/// The settings for writing the bibliography file.
	/// </summary>
	[XmlElement("writesettings")]
	public WriteSettings WriteSettings
	{
		get => _writeSettings;

		set
		{
			// WriteSettings needs to be able to override != ==.
			if (_writeSettings != value)
			{
				_writeSettings = value;
				_writeSettings.ModifiedChanged += OnChildModifiedChanged;
				Modified = true;
			}
		}
	}

	/// <summary>
	/// The settings for writing the bibliography file.
	/// </summary>
	[XmlAttribute("autogenerateekeys")]
	public bool AutoGenerateKeys
	{
		get => GetValueOrDefault<bool>(true);

		set
		{
			if (SetValue(value))
			{
				Modified = true;
			}
		}
	}

	/// <summary>
	/// Copy the bibliography entry's cite key when the entry is added.
	/// </summary>
	[XmlAttribute("copycitekeyonadd")]
	public bool CopyCiteKeyOnEntryAdd
	{
		get => GetValueOrDefault<bool>(true);

		set
		{
			if (SetValue(value))
			{
				Modified = true;
			}
		}
	}

	/// <summary>
	/// Sort the bibliography.
	/// </summary>
	[XmlAttribute("sortbibliography")]
	public bool SortBibliography
	{
		get => GetValueOrDefault<bool>(true);

		set
		{
			if (SetValue(value))
			{
				Modified = true;
			}
		}
	}

	/// <summary>
	/// Method to sort the bibliography by.
	/// </summary>
	[XmlAttribute("bibliographysortmethod")]
	public SortBy BibliographySortMethod
	{
		get => GetValueOrDefault<SortBy>(SortBy.Key);

		set
		{
			if (SetValue(value))
			{
				Modified = true;
			}
		}
	}

	#endregion

	#region Events

	private void OnMyModifiedChanged(object sender, bool modified)
	{
		// If this instance is saved, then consider the write settings saved, too.
		if (!modified)
		{
			_writeSettings.MarkSaved();
		}
	}

	private void OnChildModifiedChanged(object sender, bool modified)
	{
		// If a child was modified, then I am consider modified as well (propagate the change up).
		if (modified)
		{
			Modified = modified;
		}
	}

	#endregion

	#region Methods

	/// <summary>
	/// The WriteSettings do not save/serialize themselves.  Therefore, we provide a method for to indicate the object was saved.
	/// </summary>
	public void MarkSaved()
	{
		Modified = false;
	}

	#endregion
}