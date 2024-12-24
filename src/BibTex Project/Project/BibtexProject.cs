using BibTeXLibrary;
using DigitalProduction.Projects;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BibtexManager;

/// <summary>
/// The model.
/// </summary>
[XmlRoot("bibtexproject")]
public class BibtexProject : DigitalProduction.Projects.Project
{
	#region Fields

	private string								_bibFile						= "";
	private bool								_useRelativePaths				= false;
	private readonly Bibliography               _bibliography                   = new();

	private List<string>						_assessoryFiles					= [];
	private readonly List<BibliographyDOM>		_assessoryFilesDOMs				= [];

	private bool								_useStringConstants				= false;
	private readonly StringConstantProcessor	_stringConstantProcessor		= new();

	private bool								_useBibEntryInitialization		= false;
	private string								_bibEntryInitializationFile		= "";
	private BibEntryInitialization				_bibEntryInitialization			= new();

	private bool								_useTagQualityProcessing		= false;
	private string								_tagQualityProcessingFile		= "";
	private QualityProcessor					_tagQualityProcessor			= new();

	private bool								_useNameRemapping				= false;
	private string								_nameRemappingFile				= "";
	private string								_currentBibEntryMap				= "";
	private BibEntryRemapper					_nameRemapper					= new();

	private WriteSettings						_writeSettings					= new();
	private bool								_autoGenerateKeys				= true;
	private bool								_copyCiteKeyOnEntryAdd			= true;
	private bool                                _sortBibliography				= true;
	private SortBy                              _bibliographySortMethod			= SortBy.Key;

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public BibtexProject()
	{
	}

	#endregion

	#region Properties

	/// <summary>
	/// The path to the bibiography file.
	/// </summary>
	[XmlAttribute("bibfile")]
	public string BibliographyFile
	{
		get => _bibFile;

		set
		{
			if (_bibFile != value)
			{
				_bibFile = value;
				this.Modified = true;
				if (this.Initialized)
				{
					ReadBibliographyFile();
					BuildStringConstantMap();
				}
			}
		}
	}

	/// <summary>
	/// Use paths relative to the bibliography file.
	/// </summary>
	[XmlAttribute("userelativepaths")]
	public bool UsePathsRelativeToBibFile
	{
		get => _useRelativePaths;

		set
		{
			if (_useRelativePaths != value)
			{
				_useRelativePaths = value;
				this.Modified = true;
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
				this.Modified = true;
				if (this.Initialized)
				{
					ReadAccessoryFiles();
					BuildStringConstantMap();
				}
			}
		}
	}

	/// <summary>
	/// Replace tag values with string constants.
	/// </summary>
	public bool UseStringConstants
	{
		get => _useStringConstants;

		set
		{
			if (!_useStringConstants == value)
			{
				_useStringConstants = value;
				this.Modified = true;
			}
		}
	}

	/// <summary>
	/// Determines if the bibiography entry initialization file.
	/// </summary>
	[XmlAttribute("usebibentryinitialization")]
	public bool UseBibEntryInitialization
	{
		get => _useBibEntryInitialization;

		set
		{
			if (_useBibEntryInitialization != value)
			{
				_useBibEntryInitialization = value;
				this.Modified = true;
			}
		}
	}

	/// <summary>
	/// The path to the bibiography entry initialization file.
	/// </summary>
	[XmlAttribute("bibentryinitializationfile")]
	public string BibEntryInitializationFile
	{
		get => _bibEntryInitializationFile;

		set
		{
			if (_bibEntryInitializationFile != value)
			{
				_bibEntryInitializationFile = value;
				this.Modified = true;
				if (this.Initialized)
				{
					ReadBibEntryInitializationFiles();
				}
			}
		}
	}

	/// <summary>
	/// BibEntryInitialization.
	/// </summary>
	[XmlIgnore()]
	public BibEntryInitialization BibEntryInitialization { get => _bibEntryInitialization; }

	/// <summary>
	/// Specifies if the tags should be processed to ensure their quality.
	/// </summary>
	[XmlAttribute("usequalityprocessing")]
	public bool UseQualityProcessing
	{
		get => _useTagQualityProcessing;

		set
		{
			if (_useTagQualityProcessing != value)
			{
				_useTagQualityProcessing = value;
				this.Modified = true;
			}
		}
	}

	/// <summary>
	/// The path to the quality processor file.
	/// </summary>
	[XmlAttribute("qualityprocessorfile")]
	public string TagQualityProcessingFile
	{
		get => _tagQualityProcessingFile;

		set
		{
			if (_tagQualityProcessingFile != value)
			{
				_tagQualityProcessingFile = value;
				this.Modified = true;
				if (this.Initialized)
				{
					ReadTagQualityProcessingFile();
				}
			}
		}
	}

	/// <summary>
	/// Use BibEntry remapping.
	/// </summary>
	[XmlAttribute("usebibentryremapping")]
	public bool UseBibEntryRemapping
	{
		get => _useNameRemapping;

		set
		{
			if (_useNameRemapping != value)
			{
				_useNameRemapping = value;
				this.Modified = true;
			}
		}
	}

	/// <summary>
	/// The path to the bibliography remapping file.
	/// </summary>
	[XmlAttribute("remappingfile")]
	public string RemappingFile
	{
		get => _nameRemappingFile;

		set
		{
			if (_nameRemappingFile != value)
			{
				_nameRemappingFile = value;
				this.Modified = true;
				if (this.Initialized)
				{
					ReadNameMappingFile();
				}
			}
		}
	}

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
				this.Modified = true;
			}
		}
	}

	/// <summary>
	/// Bibliography.
	/// </summary>
	[XmlIgnore()]
	public Bibliography Bibliography { get => _bibliography; }

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
				_writeSettings.OnModifiedChanged += SetModified;
				this.Modified = true;
			}
		}
	}

	/// <summary>
	/// The settings for writing the bibliography file.
	/// </summary>
	[XmlAttribute("autogenerateekeys")]
	public bool AutoGenerateKeys
	{
		get => _autoGenerateKeys;

		set
		{
			if (_autoGenerateKeys != value)
			{
				_autoGenerateKeys = value;
				this.Modified = true;
			}
		}
	}

	/// <summary>
	/// Copy the bibliography entry's cite key when the entry is added.
	/// </summary>
	[XmlAttribute("copycitekeyonadd")]
	public bool CopyCiteKeyOnEntryAdd
	{
		get => _copyCiteKeyOnEntryAdd;

		set
		{
			if (_copyCiteKeyOnEntryAdd != value)
			{
				_copyCiteKeyOnEntryAdd = value;
				this.Modified = true;
			}
		}
	}

	/// <summary>
	/// Sort the bibliography.
	/// </summary>
	[XmlAttribute("sortbibliography")]
	public bool SortBibliography
	{
		get => _sortBibliography;

		set
		{
			if (_sortBibliography != value)
			{
				_sortBibliography = value;
				this.Modified = true;
			}
		}
	}

	/// <summary>
	/// Method to sort the bibliography by.
	/// </summary>
	[XmlAttribute("bibliographysortmethod")]
	public SortBy BibliographySortMethod
	{
		get => _bibliographySortMethod;

		set
		{
			if (_bibliographySortMethod != value)
			{
				_bibliographySortMethod = value;
				this.Modified = true;
			}
		}
	}

	#endregion

	#region File Reading Methods

	/// <summary>
	/// Read the bibliography file.
	/// </summary>
	private void ReadBibliographyFile()
	{
		string bibFile = ConvertToAbsolutePath(_bibFile);
		if (!File.Exists(bibFile))
		{
			return;
		}

		string bibEntryInitializaitonFile = ConvertToAbsolutePath(_bibEntryInitializationFile);
		if (_useBibEntryInitialization && File.Exists(bibEntryInitializaitonFile))
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
		string absolutePath = ConvertToAbsolutePath(_bibEntryInitializationFile);
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
		string absolutePath = ConvertToAbsolutePath(_tagQualityProcessingFile);
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
		string absolutePath = ConvertToAbsolutePath(_nameRemappingFile);
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

		foreach (string file in _assessoryFiles)
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
		if (this.UsePathsRelativeToBibFile && !string.IsNullOrEmpty(Path))
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
		_stringConstantProcessor.AddStringConstantsToMap(_bibliography.DocumentObjectModel);
		_stringConstantProcessor.AddStringConstantsToMap(_assessoryFilesDOMs);
	}

	#endregion

	#region Methods

	protected void SetModified(bool modified)
	{
		Modified = modified;		
	}

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
		BindingList<BibEntry> entries = ParseText(text);
		return entries[0];
	}

	/// <summary>
	/// Parse a string and return BibEntrys.
	/// </summary>
	/// <param name="text">Text to process.</param>
	public BindingList<BibEntry> ParseText(string text)
	{
		StringReader textReader = new(text);
		BibliographyDOM result;

		if (_useBibEntryInitialization)
		{
			result = BibParser.Parse(textReader, _bibEntryInitialization);
		}
		else
		{
			result = BibParser.Parse(textReader);
		}

		return result.BibliographyEntries;
	}

	/// <summary>
	/// Clean up.
	/// </summary>
	public override void Close()
	{
		// Must call base first.  This calls the OnClose event which should clear all forms (unbind) and
		// make it safe to close the Bibliography.
		base.Close();
		_bibliography.Close();
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
		if (_autoGenerateKeys)
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
		if (_autoGenerateKeys)
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
		if (_useTagQualityProcessing)
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
		if (_useTagQualityProcessing)
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
		if (_useNameRemapping)
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
		if (_useStringConstants)
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
		if (_sortBibliography)
		{
			return _bibliography.DocumentObjectModel.FindInsertIndex(entry, _bibliographySortMethod);
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
		if (_sortBibliography)
		{
			_bibliography.DocumentObjectModel.SortBibEntries(_bibliographySortMethod);
		}
	}

	/// <summary>
	/// Sort the bibliography entries.
	/// </summary>
	public IEnumerable<TagProcessingData> CleanAllEntries()
	{
		if (_useTagQualityProcessing)
		{
			bool modified = false;

			foreach (BibEntry entry in _bibliography.DocumentObjectModel.BibliographyEntries)
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
				this.Modified = true;
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
	/// The this.Path must be set and represent a valid path or this method will throw an exception.
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when the projects path is not set or not valid.</exception>
	public override void Serialize()
	{
		// Save the bibliography file from memory.
		if (!string.IsNullOrEmpty(_bibFile))
		{
			string file = DigitalProduction.IO.Path.ConvertToAbsolutePath(_bibFile, System.IO.Path.GetDirectoryName(Path)!);
			//file += ".output.bib";
			_bibliography.Write(file, _writeSettings);
		}
		base.Serialize();
	}

	public static ProjectExtractor Deserialize(string path)
	{
		ProjectExtractor projectExtractor	= ProjectExtractor.ExtractFiles(path);
		BibtexProject project				= Deserialize<BibtexProject>(projectExtractor);
		project.ReadAccessoaryFiles();
		return projectExtractor;
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