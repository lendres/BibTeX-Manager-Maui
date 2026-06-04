using BibTeXManager.Quality;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BibTeXManager.ViewModels;

public partial class GroupManagerIncludeViewModel : ObservableObject
{
	[ObservableProperty]
	public partial bool IsIncluded { get; set; }

	[ObservableProperty]
	public partial string IncludeName { get; set; } = string.Empty;

	public FieldProcessorGroup FieldProcessorGroup { get; set; } = new FieldProcessorGroup();
}