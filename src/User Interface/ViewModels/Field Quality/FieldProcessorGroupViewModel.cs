using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BibTeXManager.ViewModels;

[QueryProperty(nameof(FieldQualityProcessingFile), "FieldQualityProcessingFile")]
public partial class FieldProcessorGroupViewModel : ObservableObject
{
    #region Fields

    #endregion

    #region Construction

    public FieldProcessorGroupViewModel()
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
				ProcessorsViewModels.Add(new FieldProcessorSimpleViewModel(processor));
			}

			SelectedProcessorViewModel = ProcessorsViewModels.FirstOrDefault();
		}
	}

	private bool														Modified  { get; set; }

	[ObservableProperty]
	public partial bool													IsSubmittable { get; set; }

	[ObservableProperty]
	public partial FieldProcessorGroup?									FieldProcessorGroup { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<FieldProcessorSimpleViewModel>	ProcessorsViewModels { get; set; }			= new();

    [ObservableProperty]
    public partial FieldProcessorSimpleViewModel?						SelectedProcessorViewModel { get; set; }

	#endregion

	#region Commands

	[RelayCommand]
    public void AddProcessor(FieldProcessor fieldProcessor)
    {
        ProcessorsViewModels.Add(new FieldProcessorSimpleViewModel(fieldProcessor));
        SelectedProcessorViewModel = ProcessorsViewModels.Last();
		SetModified(true);
    }

	[RelayCommand]
	public void UpdateProcessor(FieldProcessor fieldProcessor)
	{
		SelectedProcessorViewModel!.UpdateFromProcessor();
		SetModified(true);
	}

	[RelayCommand]
    public void DeleteProcessor()
    {
        if (SelectedProcessorViewModel is null)
        {
            return;
        }

        FieldProcessorSimpleViewModel processor = SelectedProcessorViewModel;

        ProcessorsViewModels.Remove(processor);
        SelectedProcessorViewModel = ProcessorsViewModels.FirstOrDefault();
		SetModified(true);
	}

    public void Save()
    {
        FieldProcessorGroup!.FieldProcessors.Clear();

        foreach (FieldProcessorSimpleViewModel processor in ProcessorsViewModels)
        {
            FieldProcessorGroup.FieldProcessors.Add(processor.FieldProcessor);
        }
		FieldProcessorGroup.Serialize();
		SetModified(false);
	}

	#endregion

	#region Helper Functions

	public virtual bool ValidateSubmittable() => IsSubmittable = Modified;

	public void SetModified(bool modified)
	{
		Modified = modified;
		ValidateSubmittable();
	}

	#endregion
}