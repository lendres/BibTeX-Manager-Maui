using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
			FieldProcessorGroup = FieldProcessorGroup.Deserialize(value) ?? throw new Exception("Invalid file path.");

			foreach (FieldProcessor processor in FieldProcessorGroup.FieldProcessors)
			{

				string className					= processor.XsiType + "ViewModel";
				string? nameSpace					= typeof(FieldProcessorGroupEditorViewModel).Namespace;
				Type? type							= typeof(FieldProcessorGroupEditorViewModel).Assembly.GetType($"{nameSpace}.{className}");
				Debug.Assert(type != null);

				FieldProcessorViewModel instance	= (FieldProcessorViewModel)(Activator.CreateInstance(type) ?? throw new Exception("Failed to create field processor instance."));

				instance.SetProcessor(processor);
				Processors.Add(instance);
			}

			SelectedProcessor = Processors.FirstOrDefault();
		}
	}

	private bool													Modified  { get; set; }

	[ObservableProperty]
	public partial bool												IsSubmittable { get; set; }

	[ObservableProperty]
	public partial FieldProcessorGroup?								FieldProcessorGroup { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<FieldProcessorViewModel>	Processors { get; set; }			= new();

    [ObservableProperty]
    public partial FieldProcessorViewModel?							SelectedProcessor { get; set; }

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

        FieldProcessorViewModel processor = SelectedProcessor;

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
        FieldProcessorGroup!.FieldProcessors.Clear();

        foreach (FieldProcessorViewModel processorViewModel in Processors)
        {
            FieldProcessorGroup.FieldProcessors.Add(processorViewModel.ToProcessor());
        }
    }

    #endregion
}