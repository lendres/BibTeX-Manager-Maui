using BibTexManager.ViewModels;
using DigitalProduction.Maui.Views;
using System;

namespace BibTexManager.Views;

public partial class ProjectOptionsView : PopupView
{
	ProjectOptionsViewModel _viewModel;

	public ProjectOptionsView(ProjectOptionsViewModel viewModel)
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