using CommunityToolkit.Mvvm.ComponentModel;

namespace BibTeXManager.ViewModels;

[QueryProperty(nameof(AddFieldProcessorViewModelCallback), "AddFieldProcessorViewModelCallback")]
public partial class QuoteFieldProcessorViewModel : FieldProcessorViewModel
{
    #region Construction

    public QuoteFieldProcessorViewModel() :
		base(nameof(QuoteFieldProcessor))
	{
    }

    #endregion

    #region Properties

	#endregion

	#region Methods

	override public void SetProcessor(FieldProcessor processor)
	{
		base.SetProcessor(processor);
	}

	public override FieldProcessor ToProcessor()
    {
        return new QuoteFieldProcessor
		{
            FieldsToProcess	= FieldsToProcess,
            Pattern			= SearchPattern.Value!,
            FieldNames		= Fields.Where(field => !string.IsNullOrWhiteSpace(field)).ToList()
        };
    }

    #endregion
}