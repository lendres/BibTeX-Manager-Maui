using BibTeXManager.ViewModels;

namespace BibTeXManager.Views;

public partial class SentenceEndingSpacesFieldProcessorView : ContentPage
{
	#region Construction

	public SentenceEndingSpacesFieldProcessorView(SentenceEndingSpacesFieldProcessorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext					= viewModel;
		_fieldProcessorView.ViewModel	= viewModel;
	}

	#endregion
}