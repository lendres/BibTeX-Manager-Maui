using BibTexManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTexManager.Views;

public partial class WebSearchSettingsView : PopupView
{
	#region Fields

	private readonly CorrectionViewModel _viewModel;

	#endregion
	
	#region Construction

	public WebSearchSettingsView(CorrectionViewModel viewModel)
	{
		BindingContext	= viewModel;
		_viewModel		= viewModel;
		InitializeComponent();
	}

	#endregion

	#region Events

	#endregion
}