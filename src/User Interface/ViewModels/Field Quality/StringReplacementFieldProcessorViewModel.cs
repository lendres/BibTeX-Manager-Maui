using CommunityToolkit.Mvvm.ComponentModel;

namespace BibTeXManager.ViewModels;

public partial class StringReplacementFieldProcessorViewModel : FieldProcessorViewModel
{
    #region Construction

    public StringReplacementFieldProcessorViewModel() :
		base(new StringReplacementFieldProcessor())
    {
	}

    #endregion

    #region Properties

    [ObservableProperty]
    public partial string						Replacement { get; set; }					= string.Empty;

	#endregion

	#region Methods

	override public void SetProcessor(FieldProcessor processor)
	{
		base.SetProcessor(processor);
		Replacement = ((StringReplacementFieldProcessor)processor).Replacement;
	}

	public override FieldProcessor ToProcessor()
    {
        return new StringReplacementFieldProcessor
        {
            FieldsToProcess	= FieldsToProcess,
            Pattern			= SearchPattern.Value!,
            Replacement		= Replacement,
            FieldNames		= ObservableFieldNames.Select(field => field.Value).Where(value => !string.IsNullOrWhiteSpace(value)).ToList()!
		};
    }

    #endregion
}