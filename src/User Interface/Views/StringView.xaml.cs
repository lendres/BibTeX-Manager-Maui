using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace Data.Translation.Pages;

public partial class StringView : PopupView
{
	#region Construction

	public StringView(StringViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	#endregion

	#region Events
	#endregion
}