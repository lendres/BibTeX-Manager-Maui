using BibtexManager;
using BibTeXManager.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BibTeXManager.ViewModels;

public partial class GroupManagerViewModel : ObservableObject
{
	#region Fields

	private const string QualityFileExtension = ".qlty";

	#endregion

	#region Construction

	public GroupManagerViewModel()
	{
		GroupManager				= new();
		FieldQualityProcessingFile	= BibTeXProject.Instance!.Settings.FieldQualityProcessingFile;

		List<string> includeNames	= GroupManager.IncludeNames;
		List<string> availableNames	= GroupManager.GetAvailableQualityFiles(FieldQualityProcessingFile);

		Includes = new ObservableCollection<GroupManagerIncludeViewModel>(
			availableNames.Select(name => new GroupManagerIncludeViewModel
			{
				IncludeName = name,
				IsIncluded = includeNames.Contains(name, StringComparer.OrdinalIgnoreCase)
			}));
	}

	#endregion

	#region Properties

	public GroupManager GroupManager { get; }

	public string FieldQualityProcessingFile { get; }

	[ObservableProperty]
	public partial string? SelectedIncludeName { get; set; }

	public List<string> AvailableIncludeNames => GroupManager.GetAvailableQualityFiles(FieldQualityProcessingFile);

	[ObservableProperty]
	public partial ObservableCollection<GroupManagerIncludeViewModel> Includes { get; set; } = new();

	#endregion

	[RelayCommand]
	public void AddSelected()
	{
		if (GroupManager is null || string.IsNullOrWhiteSpace(SelectedIncludeName))
		{
			return;
		}

		GroupManager.AddQualityFile(FieldQualityProcessingFile, SelectedIncludeName);
		//Load(GroupManager, FieldQualityProcessingFile);
	}

	[RelayCommand]
	public void DeleteSelected()
	{
		if (GroupManager is null || string.IsNullOrWhiteSpace(SelectedIncludeName))
		{
			return;
		}

		GroupManager.DeleteQualityFile(FieldQualityProcessingFile, SelectedIncludeName);
		//Load(GroupManager, FieldQualityProcessingFile);
	}

	[RelayCommand]
	public async Task EditSelected()
	{
		if (string.IsNullOrWhiteSpace(SelectedIncludeName))
		{
			return;
		}

		string fileName = Path.ChangeExtension(SelectedIncludeName, ".qlty");
		string filePath = Path.Combine(Path.GetDirectoryName(FieldQualityProcessingFile) ?? string.Empty, fileName);

		await Shell.Current.GoToAsync(nameof(GroupManagerView), true, new Dictionary<string, object>
	{
		{ "FieldQualityProcessingFile", filePath }
	});
	}

	public void Save()
	{
		GroupManager.IncludeNames = Includes
			.Where(include => include.IsIncluded)
			.Select(include => include.IncludeName)
			.ToList();
	}

	[RelayCommand]
	public void Delete(GroupManagerIncludeViewModel include)
	{
		GroupManager.DeleteQualityFile(FieldQualityProcessingFile, include.IncludeName);
		Includes.Remove(include);
	}

	public void Add(string includeName)
	{
		GroupManager.AddQualityFile(FieldQualityProcessingFile, includeName);

		Includes.Add(new GroupManagerIncludeViewModel
		{
			IncludeName = includeName,
			IsIncluded = true
		});
	}
}