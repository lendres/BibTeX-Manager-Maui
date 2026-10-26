using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class AddFieldProcessorView : PopupView
{
	public AddFieldProcessorView(AddFieldProcessorViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
		_typePicker.SelectedIndex = 0;
	}
}