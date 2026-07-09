using CommunityToolkit.Mvvm.ComponentModel;

namespace BibTeXManager.ViewModels;

public partial class SentenceEndingSpacesFieldProcessorViewModel : FieldProcessorViewModel
{
    #region Construction

    public SentenceEndingSpacesFieldProcessorViewModel() :
		base(nameof(SentenceEndingSpacesFieldProcessor))
	{
    }

    #endregion

    #region Properties

    [ObservableProperty]
    public partial bool						FrenchSpacing { get; set; }					= false;

    [ObservableProperty]
    public partial List<string>				ExcludePatterns { get; set; }				= [];

	#endregion

	#region Methods

	override public void SetProcessor(FieldProcessor processor)
	{
		base.SetProcessor(processor);
		FrenchSpacing	= ((SentenceEndingSpacesFieldProcessor)processor).FrenchSpacing;
		ExcludePatterns = ((SentenceEndingSpacesFieldProcessor)processor).ExcludePatterns;
	}

	public override FieldProcessor ToProcessor()
    {
        return new SentenceEndingSpacesFieldProcessor
        {
            FieldsToProcess	= FieldsToProcess,
            Pattern			= SearchPattern.Value!,
            FrenchSpacing	= FrenchSpacing,
            ExcludePatterns	= ExcludePatterns,
            FieldNames		= ObservableFieldNames.Select(field => field.Value).Where(value => !string.IsNullOrWhiteSpace(value)).ToList()!
		};
    }

    #endregion
}