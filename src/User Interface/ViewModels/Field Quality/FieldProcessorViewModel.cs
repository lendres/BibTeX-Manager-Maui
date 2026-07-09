using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.ComponentModel;
using DigitalProduction.Maui.Validation;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BibTeXManager.ViewModels;

public abstract partial class FieldProcessorViewModel : ObservableObject, IQueryAttributable
{
	#region Fields

	private bool			_isButtonPressed;
	private FieldProcessor?	_fieldProcessor;

	#endregion

	#region Construction

	public FieldProcessorViewModel(string type)
    {
        Type = type;
		Initialize();
	}

    #endregion

    #region Properties

    [ObservableProperty]
    public partial bool												Modified { get; set; }						= false;

    [ObservableProperty]
    public partial bool												IsSubmittable { get; set; }					= false;

    [ObservableProperty]
    public partial string											Type { get; set; }							= string.Empty;

    [ObservableProperty]
    public partial ValidatableObject<string>						SearchPattern { get; set; }					= new();

    [ObservableProperty]
    public partial string											ReplacementText { get; set; }				= string.Empty;

	public IReadOnlyList<string>									FieldsToProcessOptions { get; set; }		= DigitalProduction.Reflection.Enumerations.GetAllDescriptionAttributesForType<FieldsToProcess>();

    [ObservableProperty]
    public partial FieldsToProcess									FieldsToProcess { get; set; }				= FieldsToProcess.OnlySpecified;

    [ObservableProperty]
    public partial bool												ShowFields { get; set; }					= true;

	[ObservableProperty]
	public partial ObservableCollection<ObservableString>			ObservableFieldNames { get; set; }			= new();

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(DeleteFieldCommand))]
	[NotifyCanExecuteChangedFor(nameof(MoveFieldUpCommand))]
	[NotifyCanExecuteChangedFor(nameof(MoveFieldDownCommand))]
	public partial ObservableString?								SelectedField { get; set; }

	public Action<FieldProcessorViewModel>?							AddFieldProcessorViewModelCallback { get; set; }

	public FieldProcessor?											FieldProcessor { get => _fieldProcessor; set => SetProcessor(value!); }

	#endregion

	#region QueryAttributable Implementation

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		query.TryGetValue("AddFieldProcessorViewModelCallback", out var value);
		if (value is Action<FieldProcessorViewModel> callback)
		{
			AddFieldProcessorViewModelCallback = callback;
		}

		query.TryGetValue("FieldProcessor", out value);
		if (value is FieldProcessor processor)
		{
			FieldProcessor = processor;
		}
	}

	#endregion

	#region Initialization and Validation

	private void Initialize()
	{
		AddValidations();
		ValidateSubmittable();
	}

	private void AddValidations()
	{
		SearchPattern.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A name is required." });
		ValidateSearchPattern();
	}

	[RelayCommand]
	private void ValidateSearchPattern()
	{
		SearchPattern.Validate();
		SetModified(true);
	}

	public virtual bool ValidateSubmittable() => IsSubmittable = Modified && SearchPattern.IsValid;

	#endregion

	#region Methods

	public virtual void SetProcessor(FieldProcessor processor)
	{
		Debug.Assert(processor != null, "Processor cannot be null.");

		_fieldProcessor		= processor;
		FieldsToProcess		= processor.FieldsToProcess;
		SearchPattern.Value	= processor.Pattern;
		Type				= processor.XsiType;

		foreach (string field in processor.FieldNames)
		{
			ObservableFieldNames.Add(new ObservableString(field));
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

	#region Fields Editting

	[RelayCommand]
	private void SelectedFieldsToProcess()
	{
		switch (FieldsToProcess)
		{
			case FieldsToProcess.OnlySpecified:
				ShowFields = true;
				break;
			case FieldsToProcess.ExcludeSpecified:
				ShowFields = true;
				break;
			case FieldsToProcess.All:
				ShowFields = false;
				break;
			default:
				throw new ArgumentOutOfRangeException();
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

		ObservableFieldNames.Remove(SelectedField);
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

		int index = ObservableFieldNames.IndexOf(SelectedField);

		if (index <= 0)
		{
			return;
		}

		ObservableFieldNames.Move(index, index - 1);
		SelectedField = null;
		_isButtonPressed = false;
		SetModified(true);
	}

	private bool CanMoveFieldUp()
	{
		return SelectedField is not null && ObservableFieldNames.IndexOf(SelectedField) > 0;
	}

	[RelayCommand(CanExecute = nameof(CanMoveFieldDown))]
	public void MoveFieldDown()
	{
		if (SelectedField is null)
		{
			return;
		}

		int index = ObservableFieldNames.IndexOf(SelectedField);

		if (index < 0 || index >= ObservableFieldNames.Count - 1)
		{
			return;
		}

		ObservableFieldNames.Move(index, index + 1);
		SelectedField = null;
		_isButtonPressed = false;
		SetModified(true);
	}

	private bool CanMoveFieldDown()
	{
		return SelectedField is not null && ObservableFieldNames.IndexOf(SelectedField) >= 0 && ObservableFieldNames.IndexOf(SelectedField) < ObservableFieldNames.Count - 1;
	}

	private void AddTemlateFieldName(string fieldName)
	{
		ObservableString observableString = new(fieldName);
		observableString.ModifiedChanged += OnChildModifiedChanged;
		ObservableFieldNames.Add(observableString);
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

	protected void SetModified(bool modified)
	{
		Modified = modified;
		ValidateSubmittable();
	}

	#endregion
}