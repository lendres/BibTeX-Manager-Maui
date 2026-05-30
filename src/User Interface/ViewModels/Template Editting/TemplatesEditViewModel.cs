using BibTeXLibrary;
using BibTeXManager.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.ComponentModel;
using DigitalProduction.Maui.Enums;
using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Xml.Serialization;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace BibTeXManager.ViewModels;

public partial class TemplatesEditViewModel : DataGridBaseViewModel<NameMap>
{
	#region Fields

	private bool _isButtonPressed;

	#endregion

	#region Construction

	public TemplatesEditViewModel()
	{
		Initializer = BibTeXProject.Instance!.BibEntryInitialization;
		Initialize();
		AddValidations();
		SetModified(false);
	 }

	#endregion

	#region Properties

	public BibEntryInitialization									Initializer { get; set; }

	[ObservableProperty]
	public partial InitializationPartType							ActiveInitializationPart { get; set; }

	[ObservableProperty]
	public partial List<string>?									TemplateNames { get; set; }

	[ObservableProperty]
	public partial string?											SelectedTemplate { get; set; }

	[ObservableProperty]
	public partial ObservableCollection<ObservableWrapper<string>>	ObservableTemplateFieldNames { get; set; } = new();

	private SerializableDictionary<string, List<string>>			TemplatesDictionary { get; set; }
	private string?													LastTemplateSelected { get; set; } = null;

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(DeleteFieldCommand))]
	[NotifyCanExecuteChangedFor(nameof(MoveFieldUpCommand))]
	[NotifyCanExecuteChangedFor(nameof(MoveFieldDownCommand))]
	public partial ObservableString?								SelectedField { get; set; }

	#endregion

	#region Initialization

	[MemberNotNull(nameof(TemplatesDictionary))]
	private void Initialize()
	{
		Items				= new ObservableCollection<NameMap>(Initializer.NameMaps);
		TemplatesDictionary	= new SerializableDictionary<string, List<string>>(Initializer.Templates);
		TemplateNames		= Initializer.TemplateNames;
		if (TemplateNames.Count > 0)
		{
			LastTemplateSelected = TemplateNames[0];
			// Add new observable strings for the field names for the currently selected template.
			foreach (string fieldName in TemplatesDictionary[LastTemplateSelected])
			{
				AddTemlateFieldName(fieldName);
			}
		}
	}

	#endregion

	#region Validation

	private void AddValidations()
	{
		//AuxiliaryFile.Validations.Add(new ConditionalIsNotNullOrEmptyRule { IsRequired = () => Settings.UseAuxiliaryFile, ValidationMessage = "A file name is required." });
		//AuxiliaryFile.Validations.Add(new ConditionalFileExistsRule { IsRequired = () => Settings.UseAuxiliaryFile, ValidationMessage = "The file does not exist." });
		//ValidateAuxiliaryFile();
	}

	[RelayCommand]
	private void ValidateAuxiliaryFile()
	{
		//if (AuxiliaryFile.Validate())
		//{
		//	Settings.AuxiliaryFile = AuxiliaryFile.Value!;
		//}
		ValidateSubmittable();
	}
	
	#endregion

	#region Events

	private void OnChildModifiedChanged(object sender, bool modified)
	{
		if (modified)
		{
			SetModified(true);
		}
	}

	#endregion

	#region Commands

	[RelayCommand]
	private void SelectedTemplateChanged()
	{
		// Store all the field names that were on the UI back into our dictionary, then clear the list.
		StoreFieldNames();
		ObservableTemplateFieldNames.Clear();

		if (SelectedTemplate == null)
		{
			return;
		}

		// Add new observable strings for the field names for the currently selected template.
		foreach (string fieldName in TemplatesDictionary[SelectedTemplate])
		{
			AddTemlateFieldName(fieldName);
		}
	}

	[RelayCommand]
	public void AddField()
	{
		AddTemlateFieldName(string.Empty);
		SetModified(true);
	}

	[RelayCommand(CanExecute = nameof(CanDeleteField))]
	public void DeleteField()
	{
		if (SelectedField is null)
		{
			return;
		}

		ObservableTemplateFieldNames.Remove(SelectedField);
		SelectedField = null;
		_isButtonPressed = false;
		SetModified(true);
	}

	public void BeginButtonPress()
	{
		_isButtonPressed = true;
	}

	public bool ShouldIgnoreUnfocus()
	{
		return _isButtonPressed;
	}

	private bool CanDeleteField()
	{
		return SelectedField is not null;
	}

	[RelayCommand(CanExecute = nameof(CanMoveFieldUp))]
	public void MoveFieldUp()
	{
		if (SelectedField is null)
		{
			return;
		}

		int index = ObservableTemplateFieldNames.IndexOf(SelectedField);

		if (index <= 0)
		{
			return;
		}

		ObservableTemplateFieldNames.Move(index, index - 1);
		SelectedField = null;
		_isButtonPressed = false;
		SetModified(true);
	}

	private bool CanMoveFieldUp()
	{
		return SelectedField is not null && ObservableTemplateFieldNames.IndexOf(SelectedField) > 0;
	}

	[RelayCommand(CanExecute = nameof(CanMoveFieldDown))]
	public void MoveFieldDown()
	{
		if (SelectedField is null)
		{
			return;
		}

		int index = ObservableTemplateFieldNames.IndexOf(SelectedField);

		if (index < 0 || index >= ObservableTemplateFieldNames.Count - 1)
		{
			return;
		}

		ObservableTemplateFieldNames.Move(index, index + 1);
		SelectedField = null;
		_isButtonPressed = false;
		SetModified(true);
	}

	private bool CanMoveFieldDown()
	{
		return SelectedField is not null && ObservableTemplateFieldNames.IndexOf(SelectedField) >= 0 && ObservableTemplateFieldNames.IndexOf(SelectedField) < ObservableTemplateFieldNames.Count - 1;
	}

	#endregion

	#region Methods

	private void AddTemlateFieldName(string fieldName)
	{
		ObservableString observableString	=  new(fieldName);
		observableString.ModifiedChanged	+= OnChildModifiedChanged;
		ObservableTemplateFieldNames.Add(observableString);
	}

	private void StoreFieldNames()
	{
		if (LastTemplateSelected != null)
		{
			List<string> templateFields = TemplatesDictionary[LastTemplateSelected];
			templateFields.Clear();
			foreach (ObservableString observableFieldName in ObservableTemplateFieldNames)
			{
				templateFields.Add(observableFieldName.Value!);
			}
		}
		LastTemplateSelected = SelectedTemplate;
	}

	private void SetModified(bool modified)
	{
		Modified = modified;
		ValidateSubmittable();
	}

	[RelayCommand]
	public void Save()
	{
		// Type to template mappings.
		SerializableDictionary<string, NameMap> nameMapDictionary = Initializer.TypeToTemplateMappings;
		nameMapDictionary.Clear();

		foreach (NameMap nameMap in Items!)
		{
			nameMapDictionary[nameMap.From] = nameMap;
		}

		
		// Save any field names updates on the UI to the template they belong to.
		StoreFieldNames();
 
		// Take all the templates from the UI and push them to the model.
		Initializer.Templates.Clear();
		foreach (KeyValuePair<string, List<string>> template in TemplatesDictionary)
		{
			Initializer.Templates.Add(template.Key, new List<string>(template.Value));
		}

		// Save the file.
		Initializer.Serialize();

		// Set the observable strings to be saved (not modified).
		foreach (ObservableString observableString in ObservableTemplateFieldNames)
		{
			observableString.Save();
		}
		SetModified(false);
	}

	public override SearchResult Find(string search)
	{
		throw new NotImplementedException();
	}

	#endregion
}