using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class ProjectOptionsView : PopupView
{
	readonly ProjectOptionsViewModel	_viewModel;
	readonly IBibTeXFilePicker			_filePicker		= DigitalProduction.Maui.Services.ServiceProvider.GetService<IBibTeXFilePicker>();

	public ProjectOptionsView(ProjectOptionsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel		= viewModel;
		BindingContext	= viewModel;
	}

	async void OnBrowseForAuxiliaryFileFile(object sender, EventArgs eventArgs)
	{
		AuxiliaryFileEntry.Text = await _filePicker.BrowseForBibliographyFile();
	}

	async void OnBrowseTagOrderFile(object sender, EventArgs eventArgs)
	{
		TagOrderEntry.Text = await _filePicker.BrowseForTagOrderFile();
	}

	async void OnBrowseTagQualityFile(object sender, EventArgs eventArgs)
	{
		TagQualityEntry.Text = await _filePicker.BrowseForTagQualityFile();
	}

	async void OnBrowseNameRemappingFile(object sender, EventArgs eventArgs)
	{
		NameRemappingEntry.Text = await _filePicker.BrowseForNameRemappingFile();
	}

	protected override void OnSaveButtonClicked(object? sender, EventArgs eventArgs)
	{
		_viewModel.Save();
		base.OnSaveButtonClicked(sender, eventArgs);
	}
}