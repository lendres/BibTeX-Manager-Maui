using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace BibTeXManager.ViewModels;

public partial class TemplateSelectionViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public TemplateSelectionViewModel()
	{
		Debug.Assert(BibTeXProject.Instance != null, "Project is null.");
		Types = BibTeXProject.Instance.BibEntryInitialization.TypeNames;
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string						Type { get; set; }			= string.Empty;

	[ObservableProperty]
	public partial List<string>					Types { get; set; }

	#endregion
}