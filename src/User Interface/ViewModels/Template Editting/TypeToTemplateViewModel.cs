using BibTeXLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;
using System.Diagnostics.CodeAnalysis;

namespace BibTeXManager.ViewModels;

public partial class TypeToTemplateViewModel : NameMapViewModel
{
	#region Construction

	public TypeToTemplateViewModel(List<string> existingNames) :
		base(existingNames)
	{
		Initialize();
	}

	public TypeToTemplateViewModel(NameMap namemap, List<string> existingNames) :
		base(namemap, existingNames)
	{
		Initialize();
	}

	[MemberNotNull(nameof(TemplateNames))]
	private void Initialize()
	{
		TemplateNames = Project.BibEntryInitialization.TemplateNames;
	}

	#endregion

	#region Properties

		[ObservableProperty]
	public partial List<string>						TemplateNames { get; set; }

	#endregion
}