using BibTeXLibrary;
using DigitalProduction.ComponentModel;
using System.Xml.Serialization;

namespace BibTeXManager;

public class ProjectSettings : NotifyPropertyModifiedChanged
{
	#region Fields

	private WriteSettings				_writeSettings					= new();

	#endregion

	#region Constructions

	public ProjectSettings()
	{
		ModifiedChanged					+= OnMyModifiedChanged;
		_writeSettings.ModifiedChanged	+= OnChildModifiedChanged;
	}

	public ProjectSettings(ProjectSettings projectSettings)
	{
		WriteSettings				= new WriteSettings(projectSettings.WriteSettings);
		UsePathsRelativeToBibFile	= projectSettings.UsePathsRelativeToBibFile;
		UseBibEntryInitialization	= projectSettings.UseBibEntryInitialization;
		BibEntryInitializationFile	= projectSettings.BibEntryInitializationFile;
		UseAuxiliaryFile			= projectSettings.UseAuxiliaryFile;
		AuxiliaryFile				= projectSettings.AuxiliaryFile;
		UseStringConstants			= projectSettings.UseStringConstants;
		UseFieldQualityProcessing	= projectSettings.UseFieldQualityProcessing;
		FieldQualityProcessingFile	= projectSettings.FieldQualityProcessingFile;
		UseBibEntryRemapping		= projectSettings.UseBibEntryRemapping;
		BibEntryRemappingFile		= projectSettings.BibEntryRemappingFile;
		AutoGenerateKeys			= projectSettings.AutoGenerateKeys;
		CopyCiteKeyOnEntryAdd		= projectSettings.CopyCiteKeyOnEntryAdd;
		SortBibliography			= projectSettings.SortBibliography;
		BibliographySortMethod		= projectSettings.BibliographySortMethod;
		Modified					= false;
	}

	#endregion

	#region Properties

	/// <summary>
	/// Use paths relative to the bibliography file.
	/// </summary>
	[XmlAttribute("userelativepaths")]
	public bool UsePathsRelativeToBibFile
	{
		get => GetValueOrDefault(false);
		set => SetValue(value);
	}

	/// <summary>
	/// Determines if the bibiography entry initialization file.
	/// </summary>
	[XmlAttribute("usebibentryinitialization")]
	public bool UseBibEntryInitialization
	{
		get => GetValueOrDefault(false);
		set => SetValue(value);
	}

	/// <summary>
	/// The path to the bibiography entry initialization file.
	/// </summary>
	[XmlAttribute("bibentryinitializationfile")]
	public string BibEntryInitializationFile
	{
		get => GetValueOrDefault(string.Empty);
		set => SetValue(value);
	}

	/// <summary>
	/// Specifies if an accessory file should be used to provide additional information for the bibliography, such as strings.
	/// </summary>
	[XmlAttribute("useauxiliaryfile")]
	public bool UseAuxiliaryFile
	{
		get => GetValueOrDefault(false);
		set => SetValue(value);
	}

	/// <summary>
	/// Assessory files that contain things like strings.
	/// </summary>
	[XmlAttribute("auxiliaryfile")]
	public string AuxiliaryFile
	{
		get => GetValueOrDefault(string.Empty);
		set => SetValue(value);
	}

	/// <summary>
	/// Replace field values with string constants.
	/// </summary>
	[XmlAttribute("usestringconstants")]
	public bool UseStringConstants
	{
		get => GetValueOrDefault(false);
		set => SetValue(value);
	}

	/// <summary>
	/// Specifies if the fields should be processed to ensure their quality.
	/// </summary>
	[XmlAttribute("usefieldqualityprocessing")]
	public bool UseFieldQualityProcessing
	{
		get => GetValueOrDefault(false);
		set => SetValue(value);
	}

	/// <summary>
	/// The path to the quality processor file.
	/// </summary>
	[XmlAttribute("qualityprocessorfile")]
	public string FieldQualityProcessingFile
	{
		get => GetValueOrDefault(string.Empty);
		set => SetValue(value);
	}

	/// <summary>
	/// Use BibEntry remapping.
	/// </summary>
	[XmlAttribute("usebibentryremapping")]
	public bool UseBibEntryRemapping
	{
		get => GetValueOrDefault(false);
		set => SetValue(value);
	}

	/// <summary>
	/// The path to the bibliography remapping file.
	/// </summary>
	[XmlAttribute("nameremappingfile")]
	public string BibEntryRemappingFile
	{
		get => GetValueOrDefault(string.Empty);
		set => SetValue(value);
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
				OnPropertyChanged();
			}
		}
	}

	/// <summary>
	/// The settings for writing the bibliography file.
	/// </summary>
	[XmlAttribute("autogenerateekeys")]
	public bool AutoGenerateKeys
	{
		get => GetValueOrDefault(true);
		set => SetValue(value);
	}

	/// <summary>
	/// Copy the bibliography entry's cite key when the entry is added.
	/// </summary>
	[XmlAttribute("copycitekeyonadd")]
	public bool CopyCiteKeyOnEntryAdd
	{
		get => GetValueOrDefault(true);
		set => SetValue(value);
	}

	/// <summary>
	/// Sort the bibliography.
	/// </summary>
	[XmlAttribute("sortbibliography")]
	public bool SortBibliography
	{
		get => GetValueOrDefault(true);
		set => SetValue(value);
	}

	/// <summary>
	/// Method to sort the bibliography by.
	/// </summary>
	[XmlAttribute("bibliographysortmethod")]
	public SortBibliographyBy BibliographySortMethod
	{
		get => GetValueOrDefault(SortBibliographyBy.Key);
		set => SetValue(value);
	}

	#endregion

	#region Events

	private void OnMyModifiedChanged(object sender, bool modified)
	{
		// If this instance is saved, then consider the write settings saved, too.
		if (!modified)
		{
			_writeSettings.Save();
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