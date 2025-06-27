using BibtexManager;
using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Controls;

namespace BibTexManager.ViewModels;

public partial class ImportErrorViewModel : ObservableObject
{
	#region Construction

	public ImportErrorViewModel()
	{
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string				Message { get; set; }					= "";

	#endregion

	#region Events


	#endregion

	#region Methods


	#endregion
}