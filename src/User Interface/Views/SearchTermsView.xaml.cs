using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class SearchTermsView : PopupView
{
	#region Construction

	public SearchTermsView(SearchTermsViewModel viewModel)
	{
		BindingContext	= viewModel;
		InitializeComponent();
	}

	#endregion
}