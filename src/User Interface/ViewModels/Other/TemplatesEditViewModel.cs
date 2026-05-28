using BibTeXLibrary;
using BibTeXManager.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.ComponentModel;
using DigitalProduction.Maui.Enums;
using DigitalProduction.Maui.ViewModels;
using System.Collections.ObjectModel;

namespace BibTeXManager.ViewModels;

public partial class ObservableString : ObservableWrapper<string>
{
	public ObservableString() { }
	public ObservableString(string value) : base(value) { }
}

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
	public partial ObservableCollection<ObservableWrapper<string>>	TemplateFieldNames { get; set; } = new();

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(DeleteFieldCommand))]
	[NotifyCanExecuteChangedFor(nameof(MoveFieldUpCommand))]
	[NotifyCanExecuteChangedFor(nameof(MoveFieldDownCommand))]
	public partial ObservableString?								SelectedField { get; set; }


	[ObservableProperty]
	public partial bool												IsSubmittable { get; set; }

	#endregion

	#region Initialization

	private void Initialize()
	{
		Items = new ObservableCollection<NameMap>(Initializer.NameMaps);
		TemplateNames = Initializer.TemplateNames;
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
	public bool ValidateSubmittable() => IsSubmittable =
		Modified;


	#endregion

	#region Events


	#endregion

	#region Commands

	[RelayCommand]
	private void SelectedTemplateChanged()
	{
		TemplateFieldNames.Clear();

		if (SelectedTemplate == null)
		{
			return;
		}

		foreach (string fieldName in Initializer.GetDefaultFields(SelectedTemplate))
		{
			TemplateFieldNames.Add(new ObservableString(fieldName));
		}

		//SelectedBibliographyEntryMap = NameMapper.Maps.TryGetValue(SelectedTemplate, out BibliographyEntryMap? map) ? map : null;
		//if (SelectedBibliographyEntryMap != null)
		//{
		//	Items = NameMapper.Maps[SelectedTemplate!].FieldNameMaps;
		//	ToType.Value = SelectedBibliographyEntryMap.ToType;
		//}
		//else
		//{
		//	// DataGrid selected item.
		//	SelectedItem = null;
		//}

	}

	[RelayCommand]
	public void AddField()
	{
		TemplateFieldNames.Add(new ObservableWrapper<string>(""));
		SetModified(true);
	}

	[RelayCommand(CanExecute = nameof(CanDeleteField))]
	public void DeleteField()
	{
		if (SelectedField is null)
		{
			return;
		}

		TemplateFieldNames.Remove(SelectedField);
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

		int index = TemplateFieldNames.IndexOf(SelectedField);

		if (index <= 0)
		{
			return;
		}

		TemplateFieldNames.Move(index, index - 1);
		SelectedField = null;
		_isButtonPressed = false;
		SetModified(true);
	}

	private bool CanMoveFieldUp()
	{
		return SelectedField is not null && TemplateFieldNames.IndexOf(SelectedField) > 0;
	}

	[RelayCommand(CanExecute = nameof(CanMoveFieldDown))]
	public void MoveFieldDown()
	{
		if (SelectedField is null)
		{
			return;
		}

		int index = TemplateFieldNames.IndexOf(SelectedField);

		if (index < 0 || index >= TemplateFieldNames.Count - 1)
		{
			return;
		}

		TemplateFieldNames.Move(index, index + 1);
		SelectedField = null;
		_isButtonPressed = false;
		SetModified(true);
	}

	private bool CanMoveFieldDown()
	{
		return SelectedField is not null && TemplateFieldNames.IndexOf(SelectedField) >= 0 && TemplateFieldNames.IndexOf(SelectedField) < TemplateFieldNames.Count - 1;
	}

	#endregion

	#region Methods

	private void SetModified(bool modified)
	{
		Modified = modified;
		ValidateSubmittable();
	}

	public void Save()
	{
	}

	public override SearchResult Find(string search)
	{
		throw new NotImplementedException();
	}

	#endregion
}