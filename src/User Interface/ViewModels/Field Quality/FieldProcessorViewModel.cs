using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BibTeXManager.ViewModels;

public abstract partial class FieldProcessorViewModel : ObservableObject
{
    #region Construction

    public FieldProcessorViewModel()
    {
    }

    #endregion

    #region Properties

    [ObservableProperty]
    public partial string						Type	 { get; set; }						= string.Empty;

    [ObservableProperty]
    public partial string						Pattern { get; set; }						= string.Empty;

    public ObservableCollection<string>			Fields { get; }								= new();

	public IReadOnlyList<string>				FieldsToProcessOptions { get; set; }		= DigitalProduction.Reflection.Enumerations.GetAllDescriptionAttributesForType<FieldsToProcess>();

    [ObservableProperty]
    public partial FieldsToProcess				FieldsToProcess { get; set; }				= FieldsToProcess.OnlySpecified;

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
}