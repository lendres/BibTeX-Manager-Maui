using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class TypeToTemplateView : PopupView
{
	public TypeToTemplateView(NameMapViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}