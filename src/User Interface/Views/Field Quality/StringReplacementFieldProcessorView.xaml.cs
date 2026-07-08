using BibTeXManager.ViewModels;

namespace BibTeXManager.Views;

public partial class StringReplacementFieldProcessorView : ContentPage
{
	#region Construction

	public StringReplacementFieldProcessorView(StringReplacementFieldProcessorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext						= viewModel;
		_fieldProcessorHeaderView.ViewModel	= viewModel;
		_fieldProcessorFieldsView.ViewModel	= viewModel;
	}

	#endregion
}