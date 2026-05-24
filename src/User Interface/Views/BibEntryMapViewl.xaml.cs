using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class BibEntryMapView : PopupView
{
	public BibEntryMapView(BibEntryMapViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	protected override void OnSaveButtonClicked(object? sender, EventArgs eventArgs)
	{
		((BibEntryMapViewModel)BindingContext).Save();
		base.OnSaveButtonClicked(sender, eventArgs);
	}
}