using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Services;
using System.Collections.ObjectModel;

namespace BibTexManager.ViewModels;

public partial class ProgramOptionsViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public ProgramOptionsViewModel()
	{
		Initialize();
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial bool							OpenLastProjectAtStartUp { get; set; }			= false;

	[ObservableProperty]
	public partial bool							RemoveNotFoundPaths { get; set; }

	[ObservableProperty]
	public partial int							NumberOfItemsShown { get; set; }

	[ObservableProperty]
	public partial int							NumberOfItemsToStore { get; set; }

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }						= true;

	#endregion

	private void Initialize()
	{
		OpenLastProjectAtStartUp	= Preferences.LoadLastProjectAtStartUp;
		RemoveNotFoundPaths			= Preferences.RecentPathsManagerService.RemoveNotFoundPaths;
		NumberOfItemsShown			= (int)Preferences.RecentPathsManagerService.NumberOfItemsShown;
		NumberOfItemsToStore		= (int)Preferences.RecentPathsManagerService.MaxSize;
	}

	public void Save()
	{
		Preferences.LoadLastProjectAtStartUp						= OpenLastProjectAtStartUp;
		Preferences.RecentPathsManagerService.RemoveNotFoundPaths	= RemoveNotFoundPaths;
		Preferences.RecentPathsManagerService.NumberOfItemsShown	= (uint)NumberOfItemsShown;
		Preferences.RecentPathsManagerService.MaxSize				= (uint)NumberOfItemsToStore;
	}
}