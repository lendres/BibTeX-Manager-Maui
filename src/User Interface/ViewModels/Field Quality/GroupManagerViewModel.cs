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
		string directory			= Path.GetDirectoryName(FieldQualityProcessingFile) ?? string.Empty;
		GroupManager				= GroupManager.Deserialize(FieldQualityProcessingFile) ?? throw new Exception("Failed to deserialize group manager.");

		List<string> includeNames	= GroupManager.IncludeNames;
		List<string> availableNames	= GroupManager.GetAvailableQualityFiles(FieldQualityProcessingFile);

		Includes = new ObservableCollection<GroupManagerIncludeViewModel>(
			availableNames.Select(name => new GroupManagerIncludeViewModel
			{
				IncludeName			= name,
				IsIncluded			= includeNames.Contains(name, StringComparer.CurrentCultureIgnoreCase),
				FieldProcessorGroup	= FieldProcessorGroup.Deserialize(Path.Combine(directory, Path.ChangeExtension(name, QualityFileExtension))) ?? new FieldProcessorGroup()
			}));
	}

	#endregion

	#region Properties

	public GroupManager													GroupManager { get; }

	public string														FieldQualityProcessingFile { get; }

	public List<string>													AvailableIncludeNames					=> Includes.Select(include => include.IncludeName).ToList();

	[ObservableProperty]
	public partial GroupManagerIncludeViewModel?						SelectedInclude { get; set; }			= null;

	[ObservableProperty]
	public partial ObservableCollection<GroupManagerIncludeViewModel>	Includes { get; set; }					= new();

	#endregion

	public void NewFieldProcessingGroup(string name)
	{
		Includes.Add(
			new GroupManagerIncludeViewModel
			{
				IncludeName			= name,
				IsIncluded			= false,
				FieldProcessorGroup	= new FieldProcessorGroup()
			}
		);
	}

	public void RenameFieldProcessingGroup(string newName)
	{
		SelectedInclude!.IncludeName = newName;
	}

	public void DeleteFieldProcessingGroup()
	{
		Includes.Remove(SelectedInclude!);
	}

	[RelayCommand]
	public async Task EditSelected()
	{
		string fileName = Path.ChangeExtension(SelectedInclude!.IncludeName, ".qlty");
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