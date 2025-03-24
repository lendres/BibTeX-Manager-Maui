using BibTexManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTexManager.Views;

public partial class ProjectOptionsView : PopupView
{
	readonly ProjectOptionsViewModel	_viewModel;
	readonly IBibTexFilePicker			_filePicker		= DigitalProduction.Maui.Services.ServiceProvider.GetService<IBibTexFilePicker>();

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

	async void OnBrowseForAuxiliaryFileFile(object sender, EventArgs eventArgs)
	{
		AuxiliaryFileEntry.Text = _viewModel.ConvertToRelativePath(await _filePicker.BrowseForBibliographyFile());
	}

	async void OnBrowseTagOrderFile(object sender, EventArgs eventArgs)
	{
		TagOrderEntry.Text = _viewModel.ConvertToRelativePath(await _filePicker.BrowseForTagOrderFile());
	}

	async void OnBrowseTagQualityFile(object sender, EventArgs eventArgs)
	{
		TagQualityEntry.Text = _viewModel.ConvertToRelativePath(await _filePicker.BrowseForTagQualityFile());
	}

	async void OnBrowseNameRemappingFile(object sender, EventArgs eventArgs)
	{
		NameRemappingEntry.Text = _viewModel.ConvertToRelativePath(await _filePicker.BrowseForNameRemappingFile());
	}
}