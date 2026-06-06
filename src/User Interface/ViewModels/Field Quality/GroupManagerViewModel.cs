using BibtexManager;
using BibTeXManager.Quality;
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
		FieldQualityProcessingFile	= BibTeXProject.Instance!.Settings.FieldQualityProcessingFile;

		GroupManager				= GroupManager.Deserialize(FieldQualityProcessingFile) ?? throw new Exception("Failed to deserialize group manager.");

		List<string> includeNames	= GroupManager.IncludeNames;
		List<string> availableNames	= GroupManager.GetAvailableQualityFiles();

		FieldProcessingGroups = new ObservableCollection<GroupManagerIncludeViewModel>(
			availableNames.Select(name => new GroupManagerIncludeViewModel
			{
				Name				= Path.GetFileNameWithoutExtension(name),
				IsIncluded			= includeNames.Contains(name, StringComparer.CurrentCultureIgnoreCase),
				FieldProcessorGroup	= FieldProcessorGroup.Deserialize(Path.Combine(GroupManager.Directory, name)) ?? new FieldProcessorGroup()
			}));
	}

	#endregion

	#region Properties

	public GroupManager													GroupManager { get; }

	public string														FieldQualityProcessingFile { get; }

	public List<string>													AvailableIncludeNames					=> FieldProcessingGroups.Select(include => include.Name).ToList();

	[ObservableProperty]
	public partial GroupManagerIncludeViewModel?						SelectedInclude { get; set; }			= null;

	[ObservableProperty]
	public partial ObservableCollection<GroupManagerIncludeViewModel>	FieldProcessingGroups { get; set; }					= new();

	#endregion

	public void NewFieldProcessingGroup(string name)
	{
		FieldProcessingGroups.Add(
			new GroupManagerIncludeViewModel
			{
				Name			= name,
				IsIncluded			= false,
				FieldProcessorGroup	= new FieldProcessorGroup()
			}
		);
	}

	public void RenameFieldProcessingGroup(string newName)
	{
		SelectedInclude!.Name = newName;
	}

	public void DeleteFieldProcessingGroup()
	{
		FieldProcessingGroups.Remove(SelectedInclude!);
	}

	[RelayCommand]
	public async Task EditSelected()
	{
		string fileName = Path.ChangeExtension(SelectedInclude!.Name, ".qlty");
		string filePath = Path.Combine(Path.GetDirectoryName(FieldQualityProcessingFile) ?? string.Empty, fileName);

		await Shell.Current.GoToAsync(nameof(GroupManagerView), true, new Dictionary<string, object>
		{
			{ "FieldQualityProcessingFile", filePath }
		});
	}

	public void Save()
	{
		GroupManager.IncludeNames = FieldProcessingGroups
			.Where(include => include.IsIncluded)
			.Select(include => include.Name)
			.ToList();
	
		foreach (GroupManagerIncludeViewModel include in FieldProcessingGroups)
		{
			include.FieldProcessorGroup.Serialize(Path.Combine(Path.GetDirectoryName(FieldQualityProcessingFile) ?? string.Empty, Path.ChangeExtension(include.Name, QualityFileExtension)));
		}
	}

	[RelayCommand]
	public void Delete(GroupManagerIncludeViewModel include)
	{
		FieldProcessingGroups.Remove(include);
	}

	public void Add(string includeName)
	{
		FieldProcessingGroups.Add(new GroupManagerIncludeViewModel
		{
			Name	= includeName,
			IsIncluded	= true
		});
	}
}