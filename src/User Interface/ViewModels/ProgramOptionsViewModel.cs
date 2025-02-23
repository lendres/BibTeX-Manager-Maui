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
	public partial ObservableCollection<int>	DisplayedItemsNumbers { get; set; }				= [5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];

	[ObservableProperty]
	public partial int							MaximumNumberOfItems { get; set; }

	[ObservableProperty]
	public partial ObservableCollection<int>	MaximumItemsNumbers { get; set; }				= [10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }						= true;

	#endregion

	private void Initialize()
	{
		OpenLastProjectAtStartUp	= Preferences.LoadLastProjectAtStartUp;
		RemoveNotFoundPaths			= _recentPathsManagerService.RemoveNotFoundPaths;
		NumberOfItemsShown			= (int)_recentPathsManagerService.NumberOfItemsShown;
		MaximumNumberOfItems		= (int)_recentPathsManagerService.MaxSize;
	}

	partial void OnMaximumNumberOfItemsChanged(int value)
	{
		// Store the current value and then assign a value that doesn't exist.  We need to assign a new value (different
		// from the current) to get the form to update.
		int storedValue = NumberOfItemsShown;
		NumberOfItemsShown = -1;

		DisplayedItemsNumbers.Clear();
		for (int i = 5; i <= value; i++)
		{
			DisplayedItemsNumbers.Add(i);
		}

		if (storedValue >  MaximumNumberOfItems)
		{
			NumberOfItemsShown = value;
		}
		else
		{
			NumberOfItemsShown = storedValue;
		}
	}

	public void Save()
	{
		Preferences.LoadLastProjectAtStartUp			= OpenLastProjectAtStartUp;
		_recentPathsManagerService.RemoveNotFoundPaths	= RemoveNotFoundPaths;
		_recentPathsManagerService.NumberOfItemsShown	= (uint)NumberOfItemsShown;
		_recentPathsManagerService.MaxSize				= (uint)MaximumNumberOfItems;
	}
}