using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class SettingsView : PopupView
{
	readonly ProjectOptionsViewModel	_viewModel;

	readonly IBibTeXFilePicker			_filePicker		= IPlatformApplication.Current!.Services.GetRequiredService<IBibTeXFilePicker>();

	public SettingsView(ProjectOptionsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel		= viewModel;
		BindingContext	= viewModel;
	}

	async void OnBrowseForAuxiliaryFileFile(object sender, EventArgs eventArgs)
	{
		AuxiliaryFileEntry.Text = await _filePicker.BrowseForBibliographyFile();
	}

	async void OnBrowseFieldOrderFile(object sender, EventArgs eventArgs)
	{
		FieldOrderEntry.Text = await _filePicker.BrowseForFieldOrderFile();
	}

	async void OnBrowseFieldQualityFile(object sender, EventArgs eventArgs)
	{
		FieldQualityEntry.Text = await _filePicker.BrowseForFieldQualityFile();
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