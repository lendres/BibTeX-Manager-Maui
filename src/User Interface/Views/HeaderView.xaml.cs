using BibTeXLibrary;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;
using System.ComponentModel;

namespace BibTeXManager.Views;

public partial class HeaderView : BibliographyPartView<HeaderViewModel>
{
	#region Construction

	public HeaderView() :
		base(MauiProgram.Services.GetRequiredService<HeaderViewModel>())
	{
		InitializeComponent();
		_mainGrid.BindingContext	= ViewModel;
	}

	#endregion

	#region Properties
	#endregion

	#region Button Events
	#endregion
}