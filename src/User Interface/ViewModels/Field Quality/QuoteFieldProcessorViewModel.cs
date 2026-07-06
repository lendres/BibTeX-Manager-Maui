using CommunityToolkit.Mvvm.ComponentModel;

namespace BibTeXManager.ViewModels;

public partial class QuoteFieldProcessorViewModel : FieldProcessorViewModel
{
    #region Construction

    public QuoteFieldProcessorViewModel()
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
            Pattern			= Pattern,
            FieldNames		= Fields.Where(field => !string.IsNullOrWhiteSpace(field)).ToList()
        };
    }

    #endregion
}