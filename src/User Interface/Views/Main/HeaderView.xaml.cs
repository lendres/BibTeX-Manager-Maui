using BibTeXManager.ViewModels;

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
}