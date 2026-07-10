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

	public string													FieldQualityProcessingFile
	{ 
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				return;
			}

			FieldProcessorGroup = FieldProcessorGroup.Deserialize(value) ?? throw new Exception("Invalid file path.");

			foreach (FieldProcessor processor in FieldProcessorGroup.FieldProcessors)
			{
				Processors.Add(new FieldProcessorSimpleViewModel(processor));
			}

			SelectedProcessor = Processors.FirstOrDefault();
		}
	}

	private bool														Modified  { get; set; }

	[ObservableProperty]
	public partial bool													IsSubmittable { get; set; }

	[ObservableProperty]
	public partial FieldProcessorGroup?									FieldProcessorGroup { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<FieldProcessorSimpleViewModel>	Processors { get; set; }			= new();

    [ObservableProperty]
    public partial FieldProcessorSimpleViewModel?						SelectedProcessor { get; set; }

	#endregion

	#region Commands

	[RelayCommand]
	public void AddNewProcessor()
	{
		AddProcessor(
		new StringReplacementFieldProcessor
		{
			Pattern = "New Processor",
			XsiType = "StringReplacementFieldProcessor"
		});
	}

	[RelayCommand]
    public void AddProcessor(FieldProcessor fieldProcessor)
    {
        Processors.Add(new FieldProcessorSimpleViewModel(fieldProcessor));
        SelectedProcessor = Processors.Last();
    }

	[RelayCommand]
	public void UpdateProcessor(FieldProcessor fieldProcessor)
	{
		SelectedProcessor!.UpdateFromProcessor();
	}

	[RelayCommand]
    public void DeleteProcessor()
    {
        if (SelectedProcessor is null)
        {
            return;
        }

        FieldProcessorSimpleViewModel processor = SelectedProcessor;

        Processors.Remove(processor);
        SelectedProcessor = Processors.FirstOrDefault();
    }

    public void Save()
    {
        FieldProcessorGroup!.FieldProcessors.Clear();

        foreach (FieldProcessorSimpleViewModel processor in Processors)
        {
            FieldProcessorGroup.FieldProcessors.Add(processor.FieldProcessor);
        }
    }

    #endregion
}