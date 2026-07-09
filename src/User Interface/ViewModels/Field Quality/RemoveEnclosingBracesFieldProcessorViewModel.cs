namespace BibTeXManager.ViewModels;

public partial class RemoveEnclosingBracesFieldProcessorViewModel : FieldProcessorViewModel
{
    #region Construction

    public RemoveEnclosingBracesFieldProcessorViewModel() :
		base(new RemoveEnclosingBracesFieldProcessor())
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
            FieldNames		= ObservableFieldNames.Select(field => field.Value).Where(value => !string.IsNullOrWhiteSpace(value)).ToList()!
        };
    }

    #endregion
}