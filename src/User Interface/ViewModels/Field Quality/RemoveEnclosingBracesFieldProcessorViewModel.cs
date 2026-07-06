namespace BibTeXManager.ViewModels;

public partial class RemoveEnclosingBracesFieldProcessorViewModel : FieldProcessorViewModel
{
    #region Construction

    public RemoveEnclosingBracesFieldProcessorViewModel()
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
        return new RemoveEnclosingBracesFieldProcessor
		{
            FieldsToProcess	= FieldsToProcess,
            Pattern			= Pattern,
            FieldNames		= Fields.Where(field => !string.IsNullOrWhiteSpace(field)).ToList()
        };
    }

    #endregion
}