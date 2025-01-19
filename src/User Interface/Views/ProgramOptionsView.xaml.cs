namespace BibTexManager.Views;

using BibTexManager.ViewModels;
using DigitalProduction.Maui.Views;
using System;

public partial class ProgramOptionsView : PopupView
{
	public ProgramOptionsView(ProgramOptionsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	protected override void OnSaveButtonClicked(object? sender, EventArgs eventArgs)
	{
		ProgramOptionsViewModel viewModel = (ProgramOptionsViewModel)BindingContext;
		viewModel.Save();
		base.OnSaveButtonClicked(sender, eventArgs);
	}
}