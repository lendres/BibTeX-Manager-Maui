using BibTeXManager.ViewModels;

namespace BibTeXManager.Views;

public partial class StringReplacementFieldProcessorView : ContentPage
{
	#region Fields

	private StringReplacementFieldProcessorViewModel? _viewModel;

	#endregion

	#region Construction

	public StringReplacementFieldProcessorView(StringReplacementFieldProcessorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext						= viewModel;
		_fieldProcessorHeaderView.ViewModel	= viewModel;
		_fieldProcessorFieldsView.ViewModel	= viewModel;
		_viewModel							= viewModel;
	}

	#endregion

	#region Events

	async private void OnTextChanged(object? sender, EventArgs eventArgs)
	{
		_viewModel?.SetModified(true);
	}

	#endregion
}