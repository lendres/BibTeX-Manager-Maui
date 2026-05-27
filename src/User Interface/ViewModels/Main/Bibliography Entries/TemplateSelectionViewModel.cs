using CommunityToolkit.Mvvm.ComponentModel;

namespace BibTeXManager.ViewModels;

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