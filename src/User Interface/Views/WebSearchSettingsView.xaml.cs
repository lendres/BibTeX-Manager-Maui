using BibTexManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTexManager.Views;

public partial class WebSearchSettingsView : PopupView
{
	#region Fields

	private readonly WebSettingsViewModel _viewModel;

	#endregion
	
	#region Construction

	public WebSearchSettingsView(WebSettingsViewModel viewModel)
	{
		BindingContext	= viewModel;
		_viewModel		= viewModel;
		InitializeComponent();
	}

	#endregion

	#region Events

	protected override void OnSaveButtonClicked(object? sender, EventArgs eventArgs)
	{
		_viewModel.Save();
		base.OnSaveButtonClicked(sender, eventArgs);
	}

	#endregion
}