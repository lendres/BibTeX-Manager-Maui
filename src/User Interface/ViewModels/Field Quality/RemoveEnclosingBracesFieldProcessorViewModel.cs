namespace BibTeXManager.ViewModels;

[QueryProperty(nameof(AddFieldProcessorViewModelCallback), "AddFieldProcessorViewModelCallback")]
public partial class RemoveEnclosingBracesFieldProcessorViewModel : FieldProcessorViewModel
{
    #region Construction

    public RemoveEnclosingBracesFieldProcessorViewModel() :
		base(nameof(RemoveEnclosingBracesFieldProcessor))
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
            Pattern			= SearchPattern.Value!,
            FieldNames		= Fields.Where(field => !string.IsNullOrWhiteSpace(field)).ToList()
        };
    }

    #endregion
}