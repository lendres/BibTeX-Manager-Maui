using BibTexManager.ViewModels;
using DigitalProduction.Maui.Views;
using DocumentFormat.OpenXml.Spreadsheet;
using System;

namespace BibTexManager.Views;

public partial class ProjectOptionsView : PopupView
{
	ProjectOptionsViewModel _viewModel;
	IBibTexFilePicker       _filePicker     = DigitalProduction.Maui.Services.ServiceProvider.GetService<IBibTexFilePicker>();

	public ProjectOptionsView(ProjectOptionsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel		= viewModel;
		BindingContext	= viewModel;
	}

	async void OnBrowseForInputFile(object sender, EventArgs eventArgs)
	{
		BibliographyFileEntry.Text = await _filePicker.BrowseForBibliographyFile();
	}


	protected override void OnSaveButtonClicked(object? sender, EventArgs eventArgs)
	{
		_viewModel.Save();
		base.OnSaveButtonClicked(sender, eventArgs);
	}
}