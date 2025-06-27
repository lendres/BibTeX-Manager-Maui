using BibTexManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTexManager.Views;

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