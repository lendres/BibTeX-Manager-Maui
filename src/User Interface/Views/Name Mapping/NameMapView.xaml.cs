using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class NameMapView : PopupView
{
	public NameMapView(NameMapViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}