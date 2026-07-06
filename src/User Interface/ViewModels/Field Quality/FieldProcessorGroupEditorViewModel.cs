using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BibTeXManager.ViewModels;

[QueryProperty(nameof(FieldQualityProcessingFile), "FieldQualityProcessingFile")]
public partial class FieldProcessorGroupEditorViewModel : ObservableObject
{
    #region Fields

    #endregion

    #region Construction

    public FieldProcessorGroupEditorViewModel()
    {
    }

    #endregion

    #region Properties

	private bool								Modified  { get; set; }

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }

	public string																	FieldQualityProcessingFile
	{ 
		set
		{
			FieldProcessorGroup = FieldProcessorGroup.Deserialize(value) ?? throw new Exception("Invalid file path.");

			foreach (FieldProcessor processor in FieldProcessorGroup.FieldProcessors)
			{
				if (processor is StringReplacementFieldProcessor stringReplacementProcessor)
				{
					Processors.Add(new StringReplacementFieldProcessorViewModel(stringReplacementProcessor));
				}
			}

			SelectedProcessor = Processors.FirstOrDefault();
		}
	}

	[ObservableProperty]
	public partial FieldProcessorGroup												FieldProcessorGroup { get; set; }

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
	private void SelectedProcessorChanged()
	{
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
        FieldProcessorGroup.FieldProcessors.Clear();

        foreach (StringReplacementFieldProcessorViewModel processorViewModel in Processors)
        {
            FieldProcessorGroup.FieldProcessors.Add(processorViewModel.ToProcessor());
        }
    }

    #endregion
}