using BibTeXManager.ViewModels;

namespace BibTeXManager.Views;

public partial class RemoveEnclosingBracesFieldProcessorView : ContentPage
{
	#region Construction

	public RemoveEnclosingBracesFieldProcessorView(RemoveEnclosingBracesFieldProcessorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext						= viewModel;
		_fieldProcessorHeaderView.ViewModel = viewModel;
		_fieldProcessorFieldsView.ViewModel = viewModel;
	}

	#endregion
}