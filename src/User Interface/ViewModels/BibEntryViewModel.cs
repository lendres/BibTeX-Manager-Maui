using BibTeXLibrary;
using BibtexManager;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;
using Microsoft.Maui.Controls;

namespace BibTexManager.ViewModels;

[QueryProperty(nameof(AddMode), "AddMode")]
[QueryProperty(nameof(BibEntry), "BibEntry")]
public partial class BibEntryViewModel : ObservableObject
{
	#region Fields

	private bool						_addMode				= true;
	private BibEntry?					_bibEntry				= new();
	private string                      _originalKey            = string.Empty;
	private WriteSettings				_writeSettings			= new();
	private bool                        _modified               = false;

	// Clipboard timer.  It is require to periodically check if there is valid data, there is no automated way of knowing what is in the clipboard.
	private readonly Timer              _timer;

	#endregion

	#region Construction

	public BibEntryViewModel()
	{
		_timer = new Timer((obj) => CheckClipboard(), null, 200, Timeout.Infinite);
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string						Title { get; set; }					= "Add Bibtex Entry";
	
	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial string						RawBibEntry { get; set; }			= "";

	//[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	//public partial ValidatableObject<string>	Key { get; set; }					= new();

	//[ObservableProperty]
	//public partial bool							Modified  { get; set; }				= false;

	[ObservableProperty]
	public partial bool							CanCopyKey  { get; set; }			= false;

	[ObservableProperty]
	public partial bool							IsKeyValid  { get; set; }			= false;

	[ObservableProperty]
	public partial bool							CanPaste  { get; set; }				= false;

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }			= false;

	public string								SaveCommand { get; set; }			= "Save";

	public bool AddMode
	{
		get => _addMode;
		set
		{
			_addMode = value;
			if (_addMode)
			{
				Title		= "Add Bibtex Entry";
				SaveCommand	= "Save";
			}
			else
			{
				Title		= "Edit Bibtex Entry";
				SaveCommand = "Replace";
			}
		}
	}

	public BibEntry? BibEntry
	{
		get => _bibEntry;
		set
		{
			_bibEntry = value;
			if (_bibEntry != null)
			{
				RawBibEntry = _bibEntry.ToString(new WriteSettings());
				_originalKey = _bibEntry.Key;
			}
		}
	}

	public WriteSettings WriteSettings
	{
		get => _writeSettings;
		set
		{
			if (value != _writeSettings)
			{
				_writeSettings   = value;

				if (_bibEntry != null)
				{
					RawBibEntry = _bibEntry.ToString(new WriteSettings());
				}
			}
		}
	}

	#endregion

	#region Initialize and Validation

	private void ValidateKey()
	{
		System.Diagnostics.Debug.Assert(BibtexProject.Instance != null);

		// Set our value for if we can copy the key to the clipboard.
		CanCopyKey = _bibEntry != null && _bibEntry.Key != string.Empty;

		// To be a valid key, we first need to be able to copy it (same requirements).  We also need to check if it is in use.  It
		// is allowed to be 
		if (!CanCopyKey || _bibEntry == null)
		{
			IsKeyValid = false;
			return;
		}

		if (_addMode == true)
		{
			// In add mode, we need to make sure the key is not already in the bibliography.
			IsKeyValid = !BibtexProject.Instance.Bibliography.IsKeyInUse(_bibEntry!.Key);
			return;
		}

		// In edit mode, we accept the orignal key.  If it is not the orginal key, it must be unique.
		if (_bibEntry.Key == _originalKey)
		{
			IsKeyValid = true;
		}
		else
		{
			IsKeyValid = !BibtexProject.Instance.Bibliography.IsKeyInUse(_bibEntry!.Key);
		}
	}

	public bool ValidateSubmittable() => IsSubmittable =
		_modified &&
		_bibEntry != null &&
		IsKeyValid;

	#endregion

	#region Events

	partial void OnRawBibEntryChanged(string value)
	{
		_modified = true;
		TryParse();
	}

	#endregion

	#region Commands

	[RelayCommand]
	private void CopyCiteKeyToClipboard()
	{
		System.Diagnostics.Debug.Assert(_bibEntry != null);
		Clipboard.Default.SetTextAsync(_bibEntry.Key);
	}

	/// <summary>
	/// Check the quality of the text in the text box.
	/// </summary>
	public IEnumerable<TagProcessingData> CheckQuality()
	{
		// Mapping.
		BibtexProject.Instance!.RemapEntryNames(BibEntry);

		// Cleaning.

		foreach (TagProcessingData tagProcessingData in BibtexProject.Instance.CleanEntry(BibEntry))
		{
			yield return tagProcessingData;
		}

		// String constants replacement.
		BibtexProject.Instance.ApplyStringConstants(BibEntry);

		// Key.
		if (_addMode)
		{
			BibtexProject.Instance.GenerateNewKey(BibEntry);
		}
		else
		{
			BibtexProject.Instance.ValidateKey(BibEntry);
		}

		RawBibEntry = BibEntry.ToString(BibtexProject.Instance.Settings.WriteSettings);
	}

	#endregion

	#region Methods

    private void CheckClipboard()
    {
		CanPaste = Clipboard.Default.HasText;
    }

	#endregion

	#region

	/// <summary>
	/// Parse the text in the text box.  Returns true if successful and false otherwise.
	/// </summary>
	private void TryParse()
	{
		try
		{
			_bibEntry = BibtexProject.Instance!.ParseSingleEntryText(RawBibEntry);
		}
		catch
		{
			_bibEntry = null;
		}

		ValidateKey();
		ValidateSubmittable();
	}


	#endregion
}