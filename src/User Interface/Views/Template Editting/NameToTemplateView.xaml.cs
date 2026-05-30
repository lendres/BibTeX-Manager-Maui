using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class NameToTemplateView : PopupView
{
	public NameToTemplateView(NameMapViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}