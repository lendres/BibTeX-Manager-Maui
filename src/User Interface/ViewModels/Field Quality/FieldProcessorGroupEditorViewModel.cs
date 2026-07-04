using BibTeXManager.Quality;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BibTeXManager.ViewModels;

[QueryProperty(nameof(FieldQualityProcessingFile), "FieldQualityProcessingFile")]
public partial class FieldProcessorGroupEditorViewModel : ObservableObject
{
    #region Fields

    private readonly GroupManagerIncludeViewModel _groupViewModel;

    #endregion

    #region Construction

    public FieldProcessorGroupEditorViewModel(GroupManagerIncludeViewModel groupViewModel)
    {
        _groupViewModel = groupViewModel;

        GroupName = groupViewModel.FieldProcessorGroup.Name;

        foreach (FieldProcessor processor in groupViewModel.FieldProcessorGroup.FieldProcessors)
        {
            if (processor is StringReplacementFieldProcessor stringReplacementProcessor)
            {
                Processors.Add(new StringReplacementFieldProcessorViewModel(stringReplacementProcessor));
            }
        }

        SelectedProcessor = Processors.FirstOrDefault();
    }

    #endregion

    #region Properties

	public string																	FieldQualityProcessingFile { get; set; }

    [ObservableProperty]
    public partial string															GroupName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial ObservableCollection<StringReplacementFieldProcessorViewModel>	Processors { get; set; } = new();

    [ObservableProperty]
    public partial StringReplacementFieldProcessorViewModel?						SelectedProcessor { get; set; }

    #endregion

    #region Commands

    [RelayCommand]
    public void AddProcessor()
    {
        StringReplacementFieldProcessorViewModel processor = new()
        {
            FieldsToProcess	= FieldsToProcess.OnlySpecified,
            Pattern			= string.Empty,
            Replacement		= string.Empty
        };

        processor.Fields.Add("title");

        Processors.Add(processor);
        SelectedProcessor = processor;
    }

    [RelayCommand]
    public void DeleteProcessor()
    {
        if (SelectedProcessor is null)
        {
            return;
        }

        StringReplacementFieldProcessorViewModel processor = SelectedProcessor;

        Processors.Remove(processor);
        SelectedProcessor = Processors.FirstOrDefault();
    }

    [RelayCommand]
    public void AddField()
    {
        SelectedProcessor?.Fields.Add(string.Empty);
    }

    [RelayCommand]
    public void DeleteField(string field)
    {
        SelectedProcessor?.Fields.Remove(field);
    }

    public void Save()
    {
        _groupViewModel.FieldProcessorGroup.Name = GroupName;

        _groupViewModel.FieldProcessorGroup.FieldProcessors.Clear();

        foreach (StringReplacementFieldProcessorViewModel processorViewModel in Processors)
        {
            _groupViewModel.FieldProcessorGroup.FieldProcessors.Add(processorViewModel.ToProcessor());
        }
    }

    #endregion
}