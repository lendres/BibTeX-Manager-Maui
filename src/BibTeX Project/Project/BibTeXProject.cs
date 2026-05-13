using BibTeXLibrary;
using DigitalProduction.Projects;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace BibTeXManager;

/// <summary>
/// The model.
/// </summary>
[XmlRoot("bibtexproject")]
public class BibTeXProject : DigitalProduction.Projects.Project
{
	#region Static Interface

	private static BibTeXProject? _instance;

	[XmlIgnore()]
	public static BibTeXProject? Instance
	{
		get => _instance;
		set => _instance = value;
	}

	public static void New() => Instance = new BibTeXProject();

	public static void New(ProjectSettings settings) => Instance = new BibTeXProject(settings);

	#endregion

	#region Fields

	private ProjectSettings                     _settings                       = new();

	private Bibliography						_bibliography					= new();

	private readonly List<BibliographyDOM>		_accessoryFilesDOMs				= [];

	private readonly StringConstantProcessor	_stringConstantProcessor		= new();

	private BibEntryInitialization				_bibEntryInitialization			= new();

	private QualityProcessor					_fieldQualityProcessor			= new();

	private BibEntryRemapper					_nameRemapper					= new();

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public BibTeXProject() :
		base(CompressionType.Uncompressed)
	{
		ModifiedChanged += OnMyModifiedChanged;
		NewBibliographyFile();
	}

	/// <summary>
	/// Default constructor.
	/// </summary>
	protected BibTeXProject(ProjectSettings settings) :
		this()
	{
		Settings = settings;
	}

	#endregion

	#region Properties

	[XmlElement("settings")]
	public ProjectSettings Settings
	{
		get => _settings;

		set
		{
			if (_settings != value)
			{
				_settings = value;
				ReadAccessoaryFiles();
			}
		}
	}

	/// <summary>
	/// BibEntryInitialization.
	/// </summary>
	[XmlIgnore()]
	public BibEntryInitialization BibEntryInitialization { get => _bibEntryInitialization; }

	/// <summary>
	/// Bibliography.
	/// </summary>
	[XmlIgnore()]
	public Bibliography Bibliography { get => _bibliography; }

	[XmlIgnore()]
	public bool BibliographytOpen { get; set; } = false;

	#endregion

	#region File Reading Methods

	/// <summary>
	/// Read the bibliography file.
	/// </summary>
	public void NewBibliographyFile(string file)
	{
		Path = file;
		NewBibliographyFile();
	}

	public void NewBibliographyFile()
	{
		if (_bibliography != null)
		{
			_bibliography.ModifiedChanged	-= OnChildModifiedChanged;
			_bibliography.PropertyChanged	-= OnPropertyChanged;
		}
		_bibliography					=  new();
		_bibliography.ModifiedChanged	+= OnChildModifiedChanged;
		_bibliography.PropertyChanged	+= OnPropertyChanged;
		Path                            =  "";
		base.Open();
	}

	/// <summary>
	/// Read the bibliography file.
	/// </summary>
	public void ReadBibliographyFile(string file)
	{
		Path = file;
		ReadBibliographyFile();
		base.Open();
	}

	/// <summary>
	/// Read the bibliography file.
	/// </summary>
	public void ReadBibliographyFile()
	{
		System.Diagnostics.Debug.Assert(_bibliography != null, "An instance of Bibliography is required. Call \"NewBibliographyFile\" before calling this method.");

		if (!File.Exists(Path))
		{
			return;
		}

		string bibEntryInitializaitonFile = ConvertToAbsolutePath(_settings.BibEntryInitializationFile);
		if (_settings.UseBibEntryInitialization && File.Exists(bibEntryInitializaitonFile))
		{
			_bibliography.Read(Path, bibEntryInitializaitonFile);
		}
		else
		{
			_bibliography.Read(Path);
		}

		BuildStringConstantMap();
		Modified	= false;
		IsOpen		= true;
	}

	/// <summary>
	/// Writes the bibliography file from memory.  The bibliography file must be set and represent a valid path
	/// or this method will throw an exception.
	/// </summary>
	public void WriteBibliographyFile(string path)
	{
		Path = path;
		WriteBibliographyFile();
	}

	/// <summary>
	/// Writes the bibliography file from memory.  The bibliography file must be set and represent a valid path
	/// or this method will throw an exception.
	/// </summary>
	public void WriteBibliographyFile()
	{
		if (!string.IsNullOrEmpty(Path))
		{
			_bibliography.Write(Path, _settings.WriteSettings);
		}
		Modified = false;
	}

	/// <summary>
	/// Read the bibliography entry initialization file.
	/// </summary>
	private void ReadBibEntryInitializationFiles()
	{
		string absolutePath = ConvertToAbsolutePath(_settings.BibEntryInitializationFile);
		if (System.IO.File.Exists(absolutePath))
		{
			_bibEntryInitialization = BibEntryInitialization.Deserialize(absolutePath) ??
				throw new Exception("Bibliography entry initialization failed.");
		}
	}

	/// <summary>
	/// Read field quality processing file.
	/// </summary>
	private void ReadFieldQualityProcessingFile()
	{
		string absolutePath = ConvertToAbsolutePath(_settings.FieldQualityProcessingFile);
		if (System.IO.File.Exists(absolutePath))
		{
			_fieldQualityProcessor = QualityProcessor.Deserialize(absolutePath) ??
				throw new Exception("Field quality initialization failed.");
		}
	}

	/// <summary>
	/// Read name mapping file.
	/// </summary>
	private void ReadNameMappingFile()
	{
		string absolutePath = ConvertToAbsolutePath(_settings.BibEntryRemappingFile);
		if (System.IO.File.Exists(absolutePath))
		{
			_nameRemapper = BibEntryRemapper.Deserialize(absolutePath) ??
				throw new Exception("Name remapping initialization failed.");
		}
	}

	/// <summary>
	/// Read assessory files.
	/// </summary>
	private void ReadAccessoryFiles()
	{
		_accessoryFilesDOMs.Clear();

		string absolutePath = ConvertToAbsolutePath(_settings.AuxiliaryFile);
		if (System.IO.File.Exists(absolutePath))
		{
			_accessoryFilesDOMs.Add(BibParser.Parse(absolutePath));
		}
	}

	/// <summary>
	/// Convert a path to absolute path if the relative path option is in use.
	/// </summary>
	/// <param name="path">Path to convert.</param>
	private string ConvertToAbsolutePath(string path)
	{
		if (_settings.UsePathsRelativeToBibFile && !string.IsNullOrEmpty(Path))
		{
			path = DigitalProduction.IO.Path.ConvertToAbsolutePath(path, System.IO.Path.GetDirectoryName(Path)!);
		}
		return path;
	}

	/// <summary>
	/// Initialize references.
	/// </summary>
	public void ReadAccessoaryFiles()
	{
		ReadBibEntryInitializationFiles();
		ReadFieldQualityProcessingFile();
		ReadNameMappingFile();
		ReadAccessoryFiles();
		BuildStringConstantMap();
	}

	/// <summary>
	/// Build the string constants map.
	/// </summary>
	private void BuildStringConstantMap()
	{
		_stringConstantProcessor.Clear();
		_stringConstantProcessor.AddStringConstantsToMap(_bibliography);
		_stringConstantProcessor.AddStringConstantsToMap(_accessoryFilesDOMs);
	}

	#endregion

	#region Events

	private void OnMyModifiedChanged(object sender, bool modified)
	{
		// If this instance is saved, then consider the write settings saved, too.
		if (!modified)
		{
			_settings.MarkSaved();
		}
	}

	/// <summary>
	/// For certain properties, we need to do some work when they change.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="eventArgs">Event arguments.</param>
	private void OnSettingsPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs eventArgs)
	{
		switch (eventArgs.PropertyName)
		{
			case nameof(Settings.BibEntryInitializationFile):
				ReadBibEntryInitializationFiles();
				ReadBibliographyFile();
				BuildStringConstantMap();
				break;

			case nameof(Settings.AuxiliaryFile):
				ReadAccessoryFiles();
				BuildStringConstantMap();
				break;

			case nameof(Settings.FieldQualityProcessingFile):
				ReadFieldQualityProcessingFile();
				break;

			case nameof(Settings.BibEntryRemappingFile):
				ReadNameMappingFile();
				break;
		}
	}

	#endregion

	#region Methods

	/// <summary>
	/// Get an array of all the names of the maps.
	/// </summary>
	public string[] GetBibEntryMapNames()
	{
		return _nameRemapper.Maps.Keys.ToArray();
	}

	/// <summary>
	/// Parse a string and return a single BibEntry.
	/// </summary>
	/// <param name="text">Text to process.</param>

	public BibEntry ParseSingleEntryText(string text)
	{
		ObservableCollection<BibEntry> entries = ParseText(text);
		return entries[0];
	}

	/// <summary>
	/// Parse a string and return BibEntrys.
	/// </summary>
	/// <param name="text">Text to process.</param>
	public ObservableCollection<BibEntry> ParseText(string text)
	{
		StringReader textReader = new(text);
		BibliographyDOM result;

		if (_settings.UseBibEntryInitialization)
		{
			result = BibParser.Parse(textReader, _bibEntryInitialization);
		}
		else
		{
			result = BibParser.Parse(textReader);
		}

		return result.Entries;
	}

	#endregion

	#region Quality and Automation Methods

	#region Entry Automation and Quality

	/// <summary>
	/// If the option for automatically generating keys is on, a key is generated for the entry.
	/// </summary>
	/// <param name="entry">BibEntry.</param>
	public void GenerateNewKey(BibEntry entry)
	{
		if (_settings.AutoGenerateKeys)
		{
			_bibliography.GenerateUniqueCiteKey(entry);
		}
	}

	/// <summary>
	/// If the option for automatically generating keys is on, a key is generated for the entry.
	/// </summary>
	/// <param name="entry">BibEntry.</param>
	public void ValidateKey(BibEntry entry)
	{
		if (_settings.AutoGenerateKeys)
		{
			if (!_bibliography.HasValidAutoCiteKey(entry))
			{
				_bibliography.GenerateUniqueCiteKey(entry);
			}
		}
	}

	/// <summary>
	/// Clean a single entry.  Used to prompt a user if the issues should be changed or not.
	/// </summary>
	/// <param name="entry">BibEntry.</param>
	public IEnumerable<FieldProcessingData> CleanEntry(BibEntry entry)
	{
		if (_settings.UseFieldQualityProcessing)
		{
			foreach (FieldProcessingData fieldProcessingData in _fieldQualityProcessor.Process(entry))
			{
				yield return fieldProcessingData;
			}
		}
	}

	/// <summary>
	/// Clean a single entry.  Used to automatically accept each change.
	/// </summary>
	/// <param name="entry">BibEntry.</param>
	public void AutoCleanEntry(BibEntry entry)
	{
		if (_settings.UseFieldQualityProcessing)
		{
			foreach (FieldProcessingData fieldProcessingData in CleanEntry(entry))
			{
				fieldProcessingData.Correction.ReplaceText    = true;
				fieldProcessingData.AcceptAll                 = true;
			}
		}
	}

	/// <summary>
	/// Remaps the Key and Field Keys to new names.
	/// </summary>
	/// <param name="entry">BibEntry.</param>
	public void RemapEntryNames(BibEntry entry)
	{
		if (_settings.UseBibEntryRemapping)
		{
			_nameRemapper.RemapEntryNames(entry);
		}
	}

	/// <summary>
	/// Search for text that can be replaced with string constants.
	/// </summary>
	/// <param name="entry"></param>
	public void ApplyStringConstants(BibEntry entry)
	{
		if (_settings.UseStringConstants)
		{
			_stringConstantProcessor.ApplyStringConstants(entry);
		}
	}

	/// <summary>
	/// Get the location to re-insert and editted entry.
	/// </summary>
	/// <param name="entry">BibEntry.</param>
	/// <param name="proposedIndex">The current index of the BibEntry.</param>
	public int GetEntryInsertIndex(BibEntry entry, int proposedIndex)
	{
		if (_settings.SortBibliography)
		{
			return _bibliography.FindInsertIndex(entry, _settings.BibliographySortMethod);
		}
		else
		{
			return proposedIndex;
		}
	}

	/// <summary>
	/// Apply all cleaning to an entry.  Automatically accepts suggested changes.
	/// </summary>
	/// <param name="entry">BibEntry to clean.</param>
	public void ApplyAllCleaning(BibEntry entry)
	{
		// Mapping.
		RemapEntryNames(entry);

		// Cleaning.
		AutoCleanEntry(entry);

		// String constants replacement.
		ApplyStringConstants(entry);

		// Key.
		GenerateNewKey(entry);
	}

	#endregion

	#region Entire Bibliography

	/// <summary>
	/// Sort the bibliography entries.
	/// Note, it is assumed this method is called deliberately.  It does not check to see if sorting is enabled in the settings.
	/// </summary>
	public void SortBibliographyEntries()
	{
		_bibliography.SortBibEntries(_settings.BibliographySortMethod);
	}

	/// <summary>
	/// Clean the entries.
	/// Note, it is assumed this method is called deliberately.  It does not check to see if quality processing is enabled in the settings.
	/// </summary>
	public IEnumerable<FieldProcessingData> CleanAllEntries()
	{
		foreach (BibEntry entry in _bibliography.Entries)
		{
			foreach (FieldProcessingData fieldProcessingData in _fieldQualityProcessor.Process(entry))
			{
				yield return fieldProcessingData;
			}
		}
	}

	#endregion

	#endregion

} // End class.