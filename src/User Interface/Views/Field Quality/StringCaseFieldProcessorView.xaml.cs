using BibTeXManager.ViewModels;

namespace BibTeXManager.Views;

public partial class StringCaseFieldProcessorView : ContentPage
{
	#region Construction

	public StringCaseFieldProcessorView(StringCaseFieldProcessorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext						= viewModel;
		_fieldProcessorHeaderView.ViewModel = viewModel;
		_fieldProcessorFieldsView.ViewModel = viewModel;
	}

	#endregion
}