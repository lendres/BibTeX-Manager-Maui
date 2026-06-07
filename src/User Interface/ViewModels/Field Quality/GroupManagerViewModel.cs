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

	[ObservableProperty]
	public partial bool													Modified  { get; set; }								= false;

	[ObservableProperty]
	public partial bool													IsSubmittable { get; set; }

	public List<string>													AvailableIncludeNames								=> FieldProcessingGroups.Select(include => include.Name).ToList();

	[ObservableProperty]
	public partial GroupManagerIncludeViewModel?						SelectedFieldProcessingGroup { get; set; }			= null;

	[ObservableProperty]
	public partial ObservableCollection<GroupManagerIncludeViewModel>	FieldProcessingGroups { get; set; }					= new();

	#endregion

	#region Events

	partial void OnModifiedChanged(bool oldValue, bool newValue)
	{
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = Modified;

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
		Modified = true;
	}

	public void RenameFieldProcessingGroup(string newName)
	{
		SelectedFieldProcessingGroup!.Name = newName;
		Modified = true;
	}

	public void DeleteFieldProcessingGroup()
	{
		FieldProcessingGroups.Remove(SelectedFieldProcessingGroup!);
		Modified = true;
	}

	[RelayCommand]
	public async Task EditSelected()
	{
		string fileName = Path.ChangeExtension(SelectedFieldProcessingGroup!.Name, ".qlty");
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

		GroupManager.DeleteAllFieldQualityProcessingGroups();
		foreach (GroupManagerIncludeViewModel include in FieldProcessingGroups)
		{
			include.FieldProcessorGroup.Serialize(Path.Combine(GroupManager.Directory, Path.ChangeExtension(include.Name, GroupManager.FieldQualityProcessingGroupExtension)));
		}
		GroupManager.Serialize();
		Modified = true;
	}
}