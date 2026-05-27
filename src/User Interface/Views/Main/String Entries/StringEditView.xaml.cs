using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class StringEditView : PopupView
{
	#region Construction

	public StringEditView(StringEditViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	#endregion
}