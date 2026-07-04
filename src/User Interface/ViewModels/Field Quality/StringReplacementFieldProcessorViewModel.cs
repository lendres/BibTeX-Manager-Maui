using BibTeXManager.Quality;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BibTeXManager.ViewModels;

public partial class StringReplacementFieldProcessorViewModel : ObservableObject
{
    #region Construction

    public StringReplacementFieldProcessorViewModel()
    {
    }

    public StringReplacementFieldProcessorViewModel(StringReplacementFieldProcessor processor)
    {
        FieldsToProcess = processor.FieldsToProcess;
        Pattern = processor.Pattern;
        Replacement = processor.Replacement;

        foreach (string field in processor.FieldNames)
        {
            Fields.Add(field);
        }
    }

    #endregion

    #region Properties

    [ObservableProperty]
    public partial FieldsToProcess FieldsToProcess { get; set; } = FieldsToProcess.OnlySpecified;

    [ObservableProperty]
    public partial string Pattern { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Replacement { get; set; } = string.Empty;

    public ObservableCollection<string> Fields { get; } = new();

    public List<FieldsToProcess> FieldsToProcessOptions { get; } =
        Enum.GetValues<FieldsToProcess>().ToList();

    #endregion

    #region Methods

    public StringReplacementFieldProcessor ToProcessor()
    {
        return new StringReplacementFieldProcessor
        {
            FieldsToProcess = FieldsToProcess,
            Pattern = Pattern,
            Replacement = Replacement,
            FieldNames = Fields.Where(field => !string.IsNullOrWhiteSpace(field)).ToList()
        };
    }

    #endregion
}