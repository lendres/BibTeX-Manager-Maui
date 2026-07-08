using BibTeXLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.ComponentModel;
using DigitalProduction.Maui.Enums;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BibTeXManager.ViewModels;

public abstract partial class FieldProcessorViewModel : ObservableObject
{
	#region Fields

	private bool _isButtonPressed;

	#endregion

	#region Construction

	public FieldProcessorViewModel()
    {
    }

    #endregion

    #region Properties

    [ObservableProperty]
    public partial bool							Modified { get; set; }						= false;

    [ObservableProperty]
    public partial bool							IsSubmittable { get; set; }					= false;

    [ObservableProperty]
    public partial string						Type { get; set; }							= string.Empty;

    [ObservableProperty]
    public partial string						Pattern { get; set; }						= string.Empty;

    public ObservableCollection<string>			Fields { get; }								= new();

	public IReadOnlyList<string>				FieldsToProcessOptions { get; set; }		= DigitalProduction.Reflection.Enumerations.GetAllDescriptionAttributesForType<FieldsToProcess>();

    [ObservableProperty]
    public partial FieldsToProcess				FieldsToProcess { get; set; }				= FieldsToProcess.OnlySpecified;


	[ObservableProperty]
	public partial ObservableCollection<ObservableWrapper<string>>	ObservableTemplateFieldNames { get; set; } = new();

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(DeleteFieldCommand))]
	[NotifyCanExecuteChangedFor(nameof(MoveFieldUpCommand))]
	[NotifyCanExecuteChangedFor(nameof(MoveFieldDownCommand))]
	public partial ObservableString?								SelectedField { get; set; }

	#endregion

	#region Methods

	virtual public void SetProcessor(FieldProcessor processor)
	{
		FieldsToProcess	= processor.FieldsToProcess;
		Pattern			= processor.Pattern;
		Type			= processor.XsiType;

		foreach (string field in processor.FieldNames)
		{
			Fields.Add(field);
		}
	}

	abstract public FieldProcessor ToProcessor();

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

	#region Template Field Editting

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

	private void AddTemlateFieldName(string fieldName)
	{
		ObservableString observableString = new(fieldName);
		observableString.ModifiedChanged += OnChildModifiedChanged;
		ObservableTemplateFieldNames.Add(observableString);
	}

	//private void StoreFieldNames()
	//{
	//	if (LastTemplateSelected != null)
	//	{
	//		List<string> templateFields = TemplatesDictionary[LastTemplateSelected];
	//		templateFields.Clear();
	//		foreach (ObservableString observableFieldName in ObservableTemplateFieldNames)
	//		{
	//			templateFields.Add(observableFieldName.Value!);
	//		}
	//	}
	//	LastTemplateSelected = SelectedTemplate;
	//}

	#endregion

	#region Helper Functions

	private void SetModified(bool modified)
	{
		Modified = modified;
		ValidateSubmittable();
	}

	/// <summary>
	/// Checks if a given template is in use by any of the NameMaps.
	/// </summary>
	/// <param name="template">The template name to check for.</param>
	/// <returns>True if any of the NameMaps uses the template, false otherwise.</returns>
	public bool IsCurrentTemplateInUse()
	{
		//Trace.Assert(Items != null);
		//foreach (NameMap nameMap in Items)
		//{
		//	if (nameMap.To == SelectedTemplate)
		//	{
		//		return true;
		//	}
		//}
		return false;
	}

	public virtual bool ValidateSubmittable()
	{
		return IsSubmittable = Modified;
	}

	#endregion
}