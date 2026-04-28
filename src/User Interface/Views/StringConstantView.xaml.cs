using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class StringConstantView : PopupView
{
	#region Construction

	public StringConstantView(StringViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	#endregion

	#region Events
	#endregion
}