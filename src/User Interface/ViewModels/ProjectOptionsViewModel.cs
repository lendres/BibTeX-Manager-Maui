using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Services;
using System.Collections.ObjectModel;

namespace BibTexManager.ViewModels;

public partial class ProjectOptionsViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public ProjectOptionsViewModel()
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
	}

	public void Save()
	{

	}
}