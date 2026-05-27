using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class GetNameView : PopupView
{
	public GetNameView(GetNameViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}