using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Strings;

namespace BibTeXManager.ViewModels;

[QueryProperty(nameof(AddFieldProcessorViewModelCallback), "AddFieldProcessorViewModelCallback")]
public partial class StringCaseFieldProcessorViewModel : FieldProcessorViewModel
{
    #region Construction

    public StringCaseFieldProcessorViewModel() :
		base(nameof(StringCaseFieldProcessor))
    {
    }

    #endregion

    #region Properties

    [ObservableProperty]
    public partial StringCase					StringCase { get; set; }				= StringCase.TitleCase;

    [ObservableProperty]
    public partial string						Culture { get; set; }					= string.Empty;

	#endregion

	#region Methods

	override public void SetProcessor(FieldProcessor processor)
	{
		base.SetProcessor(processor);
		StringCase	= ((StringCaseFieldProcessor)processor).StringCase;
		Culture		= ((StringCaseFieldProcessor)processor).Culture;
	}

	public override FieldProcessor ToProcessor()
    {
        return new StringCaseFieldProcessor
		{
            FieldsToProcess	= FieldsToProcess,
            Pattern			= SearchPattern.Value!,
            StringCase		= StringCase,
			Culture			= Culture,
            FieldNames		= Fields.Where(field => !string.IsNullOrWhiteSpace(field)).ToList()
        };
    }

    #endregion
}