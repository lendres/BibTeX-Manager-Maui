using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.ComponentModel;
using System.Diagnostics;

namespace BibTeXManager.ViewModels;

public partial class FieldProcessorSimpleViewModel : ObservableObject
{
	#region Fields

	private FieldProcessor	_fieldProcessor = new StringReplacementFieldProcessor();

	#endregion

	#region Construction

	public FieldProcessorSimpleViewModel(FieldProcessor fieldProcessor)
    {
		FieldProcessor = fieldProcessor;
	}

    #endregion

    #region Properties

    [ObservableProperty]
    public partial bool					Modified { get; set; }						= false;

    [ObservableProperty]
    public partial ObservableString		SearchPattern { get; set; }					= new ObservableString();

    [ObservableProperty]
    public partial string				Type { get; set; }							= string.Empty;

	public FieldProcessor				FieldProcessor { get => _fieldProcessor; set => SetProcessor(value!); } 

	#endregion

	#region Methods

	public void SetProcessor(FieldProcessor processor)
	{
		Debug.Assert(processor != null, "Processor cannot be null.");

		_fieldProcessor		= processor;
		SearchPattern.Value		= processor.Pattern;
		Type				= processor.XsiType;

		SetModified(false);
	}

	public void UpdateFromProcessor()
	{
		SearchPattern.Value	= _fieldProcessor.Pattern;
		Type				= _fieldProcessor.XsiType;
	}

	#endregion

	#region Helper Functions

	protected void SetModified(bool modified)
	{
		Modified = modified;
	}

	#endregion
}