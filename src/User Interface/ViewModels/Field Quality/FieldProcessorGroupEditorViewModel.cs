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
			if (string.IsNullOrEmpty(value))
			{
				return;
			}

			FieldProcessorGroup = FieldProcessorGroup.Deserialize(value) ?? throw new Exception("Invalid file path.");

			foreach (FieldProcessor processor in FieldProcessorGroup.FieldProcessors)
			{

				string className					= processor.XsiType + "ViewModel";
				string? nameSpace					= typeof(FieldProcessorGroupEditorViewModel).Namespace;
				Type? type							= typeof(FieldProcessorGroupEditorViewModel).Assembly.GetType($"{nameSpace}.{className}");
				Debug.Assert(type != null);

				FieldProcessorViewModel instance	= (FieldProcessorViewModel)(Activator.CreateInstance(type) ?? throw new Exception("Failed to create field processor instance."));

				instance.SetProcessor(processor);
				ProcessorViewModels.Add(instance);
			}

			SelectedProcessor = ProcessorViewModels.FirstOrDefault();
		}
	}

	private bool													Modified  { get; set; }

	[ObservableProperty]
	public partial bool												IsSubmittable { get; set; }

	[ObservableProperty]
	public partial FieldProcessorGroup?								FieldProcessorGroup { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<FieldProcessorViewModel>	ProcessorViewModels { get; set; }			= new();

    [ObservableProperty]
    public partial FieldProcessorViewModel?							SelectedProcessor { get; set; }

    #endregion

    #region Commands

    [RelayCommand]
    public void AddProcessor(FieldProcessorViewModel fieldProcessorViewModel)
    {
        ProcessorViewModels.Add(fieldProcessorViewModel);
        SelectedProcessor = fieldProcessorViewModel;
    }

    [RelayCommand]
    public void DeleteProcessor()
    {
        if (SelectedProcessor is null)
        {
            return;
        }

        FieldProcessorViewModel processor = SelectedProcessor;

        ProcessorViewModels.Remove(processor);
        SelectedProcessor = ProcessorViewModels.FirstOrDefault();
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

        foreach (FieldProcessorViewModel processorViewModel in ProcessorViewModels)
        {
            FieldProcessorGroup.FieldProcessors.Add(processorViewModel.ToProcessor());
        }
    }

    #endregion
}