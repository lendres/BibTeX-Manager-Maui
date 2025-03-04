using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Services;
using System.Collections.ObjectModel;

namespace BibTexManager.ViewModels;

public partial class TemplateSelectionViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public TemplateSelectionViewModel(List<string> templateNames)
	{
		Templates = templateNames;
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string						Template { get; set; }			= string.Empty;

	[ObservableProperty]
	public partial List<string>					Templates { get; set; }

	#endregion
}