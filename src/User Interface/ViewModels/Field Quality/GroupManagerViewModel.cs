using BibTeXLibrary;
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

		FieldProcessingGroups = new ObservableCollection<GroupManagerIncludeViewModel>();
		foreach (string name in availableNames)
		{
			AddNewGroupManagerIncludeViewModel(
				Path.GetFileNameWithoutExtension(name),
				includeNames.Contains(name, StringComparer.CurrentCultureIgnoreCase),
				FieldProcessorGroup.Deserialize(Path.Combine(GroupManager.Directory, name)) ?? new FieldProcessorGroup()
			);
		}
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

	#region Field Processing Group Management

	public void NewFieldProcessingGroup(string name)
	{
		AddNewGroupManagerIncludeViewModel(name, false, new FieldProcessorGroup());
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

	public string GetSelectedQualityFilePath()
	{
		string fileName = Path.ChangeExtension(SelectedFieldProcessingGroup!.Name, GroupManager.FieldQualityProcessingGroupExtension);
		return Path.Combine(Path.GetDirectoryName(FieldQualityProcessingFile) ?? string.Empty, fileName);
	}

	#endregion

	#region Methods

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

	private void AddNewGroupManagerIncludeViewModel(string name, bool isIncluded, FieldProcessorGroup fieldProcessorGroup)
	{
		GroupManagerIncludeViewModel groupManagerIncludeViewModel = new GroupManagerIncludeViewModel
		{
			Name = name,
			IsIncluded = isIncluded,
			FieldProcessorGroup = fieldProcessorGroup
		};
		groupManagerIncludeViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName == nameof(GroupManagerIncludeViewModel.IsIncluded))
			{
				Modified = true;
			}
		};
		FieldProcessingGroups.Add(groupManagerIncludeViewModel);
	}

	#endregion
}