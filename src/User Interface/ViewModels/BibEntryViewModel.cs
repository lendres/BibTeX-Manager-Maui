using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Validation;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibTeXLibrary;
using Microsoft.Extensions.Configuration;
using CommunityToolkit.Mvvm.Input;
using Data.Translation.Validation;
using DocumentFormat.OpenXml.Wordprocessing;
using BibtexManager;
using DigitalProduction.Projects;

namespace XSLTProcessorMaui.ViewModels;

public partial class BibEntryViewModel : ObservableObject
{
	#region Fields

	[ObservableProperty]
	private string							_title;
	private bool							_addMode;

	[ObservableProperty]
	private string							_rawBibEntry;

	[ObservableProperty]
	private BibEntry						_bibEntry;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_key								= new();
	private readonly string					_previousKey						= "";
	private readonly List<string>			_existingKeys;

	[ObservableProperty]
	private bool							_isSubmittable;

	#endregion

	#region Construction

	public BibEntryViewModel(List<string> configurationKeys)
	{
		BibEntry		= new();
		Title			= "Add Bibtex Entry";
		_addMode		= true;
		Initialize();
	}

	public BibEntryViewModel(BibEntry bibEntry, List<string> configurationKeys)
    {
		BibEntry		= bibEntry;
		Title			= "Edit Bibtex Entry";
		_addMode		= false;
		Initialize();
	}

	#endregion

	#region Initialize and Validation

	private void Initialize()
	{
		InitializeValues();
		AddValidations();
		ValidateSubmittable();
	}

	private void InitializeValues()
	{
		Key.Value			= BibEntry.Key;
	}

	private void AddValidations()
	{
		Key.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A Key is required." });
		Key.Validations.Add(new IsNotDuplicateStringRule
		{
			ValidationMessage		= "The value is already in use.",
			Values					= _existingKeys,
			ExcludeValue			= _previousKey
		});
		ValidateKey();
	}

	[RelayCommand]
	private void ValidateKey()
	{
		if (Key.Validate())
		{
			BibEntry.Key = Key.Value ?? "";
		}
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = Key.IsValid;

	#endregion

	#region

	
	/// <summary>
	/// Check the quality of the text in the text box.
	/// </summary>
	private void CheckQuality()
	{
		if (Parse())
		{
			// Mapping.
			BibtexProject.Instance!.RemapEntryNames(BibEntry);

			// Cleaning.
			bool breakNext = false;
			foreach (TagProcessingData tagProcessingData in BibtexProject.Instance.CleanEntry(BibEntry))
			{
				// If the processing was cancelled, we break.  We have to loop back around here to give the
				// processing a chance to finish (it was yielded).  Now exit before processing another entry.
				if (breakNext)
				{
					break;
				}

				//CorrectionForm correctionForm	= new CorrectionForm(tagProcessingData);
				//DialogResult dialogResult		= correctionForm.Show(this);

				//breakNext = dialogResult == DialogResult.Cancel;
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

				RawBibEntry = BibEntry.ToString(BibtexProject.Instance.WriteSettings);
			}
		}

	/// <summary>
	/// Parse the text in the text box.  Returns true if successful and false otherwise.
	/// </summary>
	private bool Parse()
	{
		bool success = true;
		try
		{
			BibEntry = BibtexProject.Instance!.ParseSingleEntryText(RawBibEntry);
		}
		catch (Exception exception)
		{
			// The text entered contained an error.  Display it and cancel the "ok" (return).
			//MessageBox.Show(this, exception.Message, "Error Parsing Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
			success = false;
		}

		return success;
	}


	#endregion
}