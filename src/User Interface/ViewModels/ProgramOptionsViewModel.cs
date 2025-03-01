using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Services;
using System.Collections.ObjectModel;

namespace BibTexManager.ViewModels;

public partial class ProgramOptionsViewModel : ObservableObject
{
	#region Fields

	private readonly IRecentPathsManagerService      _recentPathsManagerService;

	#endregion

	#region Construction

	public ProgramOptionsViewModel()
	{
		_recentPathsManagerService = DigitalProduction.Maui.Services.ServiceProvider.GetService<IRecentPathsManagerService>();
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
		RemoveNotFoundPaths			= _recentPathsManagerService.RemoveNotFoundPaths;
		NumberOfItemsShown			= (int)_recentPathsManagerService.NumberOfItemsShown;
		NumberOfItemsToStore		= (int)_recentPathsManagerService.MaxSize;
	}

	public void Save()
	{
		Preferences.LoadLastProjectAtStartUp			= OpenLastProjectAtStartUp;
		_recentPathsManagerService.RemoveNotFoundPaths	= RemoveNotFoundPaths;
		_recentPathsManagerService.NumberOfItemsShown	= (uint)NumberOfItemsShown;
		_recentPathsManagerService.MaxSize				= (uint)NumberOfItemsToStore;
	}
}