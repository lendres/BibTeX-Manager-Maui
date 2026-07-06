using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BibTeXManager.ViewModels;

public partial class StringReplacementFieldProcessorViewModel : FieldProcessorViewModel
{
    #region Construction

    public StringReplacementFieldProcessorViewModel()
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
            Pattern			= Pattern,
            Replacement		= Replacement,
            FieldNames		= Fields.Where(field => !string.IsNullOrWhiteSpace(field)).ToList()
        };
    }

    #endregion
}