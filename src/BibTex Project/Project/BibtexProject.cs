using BibTeXLibrary;
using DigitalProduction.Projects;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace BibtexManager;

/// <summary>
/// The model.
/// </summary>
[XmlRoot("bibtexproject")]
public class BibtexProject : DigitalProduction.Projects.Project
{
	#region Static Interface

	private static BibtexProject? _instance;

	[XmlIgnore()]
	public static BibtexProject? Instance
	{
		get => _instance;
		set
		{
			_instance = value;
			if (_instance != null)
			{
				_instance.Closed += OnClose;
			}
		}
	}

	public static void New() => Instance = new BibtexProject();
	public static void New(string bibliographyFile) => Instance = new BibtexProject(bibliographyFile);

	private static void OnClose() { _instance = null; }

	#endregion

	#region Fields

	private ProjectSettings                     _settings                       = new();

	private readonly Bibliography               _bibliography                   = new();

	private readonly List<BibliographyDOM>		_assessoryFilesDOMs				= [];

	private readonly StringConstantProcessor	_stringConstantProcessor		= new();

	private BibEntryInitialization				_bibEntryInitialization			= new();

	private QualityProcessor					_tagQualityProcessor			= new();

	private string								_currentBibEntryMap				= "";
	private BibEntryRemapper					_nameRemapper					= new();

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	protected BibtexProject() :
		base(CompressionType.Uncompressed)
	{
		ModifiedChanged += OnMyModifiedChanged;

		_settings.ModifiedChanged += OnChildModifiedChanged;
		_settings.PropertyChanged += OnSettingsPropertyChanged;

		_bibliography.ModifiedChanged += OnChildModifiedChanged;
		_bibliography.PropertyChanged += OnPropertyChanged;
	}

	/// <summary>
	/// Default constructor.
	/// </summary>
	protected BibtexProject(string bibliographyFile) :
		this()
	{
		_settings.BibliographyFile = bibliographyFile;
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
				Modified = true;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>
	/// BibEntryInitialization.
	/// </summary>
	[XmlIgnore()]
	public BibEntryInitialization BibEntryInitialization { get => _bibEntryInitialization; }

	/// <summary>
	/// The BibEntryMap to use for remapping.
	/// </summary>
	[XmlAttribute("bibentrymap")]
	public string BibEntryMap
	{
		get => _currentBibEntryMap;

		set
		{
			if (_currentBibEntryMap != value)
			{
				_currentBibEntryMap = value;
				Modified = true;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>
	/// Bibliography.
	/// </summary>
	[XmlIgnore()]
	public Bibliography Bibliography { get => _bibliography; }

	#endregion

	#region File Reading Methods

	/// <summary>
	/// Read the bibliography file.
	/// </summary>
	private void ReadBibliographyFile()
	{
		string bibFile = ConvertToAbsolutePath(_settings.BibliographyFile);
		if (!File.Exists(bibFile))
		{
			return;
		}

		string bibEntryInitializaitonFile = ConvertToAbsolutePath(_settings.BibEntryInitializationFile);
		if (_settings.UseBibEntryInitialization && File.Exists(bibEntryInitializaitonFile))
		{
			_bibliography.Read(bibFile, bibEntryInitializaitonFile);
		}
		else
		{
			_bibliography.Read(bibFile);
		}
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
	/// Read tag quality processing file.
	/// </summary>
	private void ReadTagQualityProcessingFile()
	{
		string absolutePath = ConvertToAbsolutePath(_settings.TagQualityProcessingFile);
		if (System.IO.File.Exists(absolutePath))
		{
			_tagQualityProcessor = QualityProcessor.Deserialize(absolutePath) ??
				throw new Exception("Tag quality initialization failed.");
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
		_assessoryFilesDOMs.Clear();

		foreach (string file in _settings.AssessoryFiles)
		{
			string absolutePath = ConvertToAbsolutePath(file);
			if (System.IO.File.Exists(absolutePath))
			{
				_assessoryFilesDOMs.Add(BibParser.Parse(absolutePath));
			}
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
	/// Build the string constants map.
	/// </summary>
	private void BuildStringConstantMap()
	{
		_stringConstantProcessor.Clear();
		_stringConstantProcessor.AddStringConstantsToMap(_bibliography);
		_stringConstantProcessor.AddStringConstantsToMap(_assessoryFilesDOMs);
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

			case nameof(Settings.BibliographyFile):
				ReadBibliographyFile();
				BuildStringConstantMap();
				break;

			case nameof(Settings.AssessoryFiles):
				ReadAccessoryFiles();
				BuildStringConstantMap();
				break;

			case nameof(Settings.TagQualityProcessingFile):
				ReadTagQualityProcessingFile();
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

	/// <summary>
	/// Clean up.
	/// </summary>
	public override void Close()
	{
		// Must call base first.  This calls the OnClose event which should clear all forms (unbind) and
		// make it safe to close the Bibliography.
		base.Close();
		Instance = null;
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
	public IEnumerable<TagProcessingData> CleanEntry(BibEntry entry)
	{
		if (_settings.UseTagQualityProcessing)
		{
			foreach (TagProcessingData tagProcessingData in _tagQualityProcessor.Process(entry))
			{
				yield return tagProcessingData;
			}
		}
	}

	/// <summary>
	/// Clean a single entry.  Used to automatically accept each change.
	/// </summary>
	/// <param name="entry">BibEntry.</param>
	public void AutoCleanEntry(BibEntry entry)
	{
		if (_settings.UseTagQualityProcessing)
		{
			foreach (TagProcessingData tagProcessingData in CleanEntry(entry))
			{
				tagProcessingData.Correction.ReplaceText    = true;
				tagProcessingData.AcceptAll                 = true;
			}
		}
	}

	/// <summary>
	/// Remaps the Key and Tag Keys to new names.
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
	/// </summary>
	public void SortBibliographyEntries()
	{
		if (_settings.SortBibliography)
		{
			_bibliography.SortBibEntries(_settings.BibliographySortMethod);
		}
	}

	/// <summary>
	/// Sort the bibliography entries.
	/// </summary>
	public IEnumerable<TagProcessingData> CleanAllEntries()
	{
		if (_settings.UseTagQualityProcessing)
		{
			bool modified = false;

			foreach (BibEntry entry in _bibliography.Entries)
			{
				foreach (TagProcessingData tagProcessingData in _tagQualityProcessor.Process(entry))
				{
					if (tagProcessingData.Correction.ReplaceText)
					{
						modified = true;
					}
					yield return tagProcessingData;
				}
			}

			if (modified)
			{
				Modified = true;
			}
		}
	}

	#endregion

	#endregion

	#region XML

	/// <summary>
	/// Writes a Project file (compressed file containing all the project's files).  Uses a ProjectCompressor to zip all files.  An
	/// event of RaiseOnSavingEvent fires allowing other files to be added to the project.
	///
	/// The Path must be set and represent a valid path or this method will throw an exception.
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when the projects path is not set or not valid.</exception>
	public override void Serialize()
	{
		// Save the bibliography file from memory.
		if (!string.IsNullOrEmpty(_settings.BibliographyFile))
		{
			string file = DigitalProduction.IO.Path.ConvertToAbsolutePath(_settings.BibliographyFile, System.IO.Path.GetDirectoryName(Path)!);
			//file += ".output.bib";
			_bibliography.Write(file, _settings.WriteSettings);
		}
		base.Serialize();
	}

	public static void Deserialize(string path)
	{
		Instance = Deserialize<BibtexProject>(path, CompressionType.Uncompressed);
		Instance.ReadAccessoaryFiles();
		Instance.Modified = false;
	}

	/// <summary>
	/// Initialize references.
	/// </summary>
	protected override void DeserializationInitialization()
	{
	}

	/// <summary>
	/// Initialize references.
	/// </summary>
	public void ReadAccessoaryFiles()
	{
		ReadBibEntryInitializationFiles();
		ReadTagQualityProcessingFile();
		ReadNameMappingFile();
		ReadBibliographyFile();
		ReadAccessoryFiles();
		BuildStringConstantMap();
	}

	#endregion

} // End class.