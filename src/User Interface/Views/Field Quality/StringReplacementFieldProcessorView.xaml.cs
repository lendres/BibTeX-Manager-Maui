using BibTeXManager.ViewModels;

namespace BibTeXManager.Views;

public partial class StringReplacementFieldProcessorView : ContentPage
{
	public StringReplacementFieldProcessorView(StringReplacementFieldProcessorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_fieldProcessorView.ViewModel = viewModel;
	}
}