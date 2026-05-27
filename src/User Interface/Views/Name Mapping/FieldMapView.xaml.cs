using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class FieldMapView : PopupView
{
	public FieldMapView(FieldMapViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}