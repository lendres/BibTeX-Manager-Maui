using BibTexManager.ViewModels;
using DigitalProduction.Maui.Views;
using System;

namespace BibTexManager.Views;

public partial class ProgramOptionsView : PopupView
{
	ProgramOptionsViewModel _viewModel;

	public ProgramOptionsView(ProgramOptionsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	protected override void OnSaveButtonClicked(object? sender, EventArgs eventArgs)
	{
		_viewModel.Save();
		base.OnSaveButtonClicked(sender, eventArgs);
	}
}