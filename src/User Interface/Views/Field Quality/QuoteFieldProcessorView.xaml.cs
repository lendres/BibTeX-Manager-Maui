using BibTeXManager.ViewModels;

namespace BibTeXManager.Views;

public partial class QuoteFieldProcessorView : ContentPage
{
	#region Construction

	public QuoteFieldProcessorView(QuoteFieldProcessorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext					= viewModel;
		_fieldProcessorView.ViewModel	= viewModel;
	}

	#endregion
}