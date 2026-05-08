using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class SettingsView : ContentPage
{
	readonly ProjectOptionsViewModel	_viewModel;

	readonly IBibTeXFilePicker			_filePicker		= IPlatformApplication.Current!.Services.GetRequiredService<IBibTeXFilePicker>();

	public SettingsView(ProjectOptionsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel		= viewModel;
		BindingContext	= viewModel;
	}

	async private void OnBrowseForAuxiliaryFileFile(object sender, EventArgs eventArgs)
	{
		AuxiliaryFileEntry.Text = await _filePicker.BrowseForBibliographyFile();
	}

	async private void OnBrowseFieldOrderFile(object sender, EventArgs eventArgs)
	{
		FieldOrderEntry.Text = await _filePicker.BrowseForFieldOrderFile();
	}

	async private void OnBrowseFieldQualityFile(object sender, EventArgs eventArgs)
	{
		FieldQualityEntry.Text = await _filePicker.BrowseForFieldQualityFile();
	}

	async private void OnBrowseNameRemappingFile(object sender, EventArgs eventArgs)
	{
		NameRemappingEntry.Text = await _filePicker.BrowseForNameRemappingFile();
	}

	async private void OnSave(object? sender, EventArgs eventArgs)
	{
		_viewModel.Save();
		Dictionary<string, object?> navigationParameter = new()
		{
			{ "NavigationCommand",  "Do Nothing" },
			{ "NavigationObject",   null }
		};
		await Shell.Current.GoToAsync("../", true, navigationParameter);
	}

	async private void OnCancel(object sender, EventArgs eventArgs)
	{
		// Navigate back with a result.
		Dictionary<string, object?> navigationParameter = new()
		{
			{ "NavigationCommand",  "Do Nothing" },
			{ "NavigationObject",   null }
		};
		await Shell.Current.GoToAsync("../", true, navigationParameter);
	}
}