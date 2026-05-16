using BibTeXManager.ViewModels;

namespace BibTeXManager.Views;

public partial class SettingsView : ContentPage
{
	readonly SettingsViewModel	_viewModel;

	readonly IBibTeXFilePicker			_filePicker		= IPlatformApplication.Current!.Services.GetRequiredService<IBibTeXFilePicker>();

	public SettingsView(SettingsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel		= viewModel;
		BindingContext	= viewModel;
	}

	async private void OnBrowseForAuxiliaryFileFile(object sender, EventArgs eventArgs)
	{
		string result = await _filePicker.BrowseForBibliographyFile();
		if (result != string.Empty)
		{
			AuxiliaryFileEntry.Text = result;
		}
	}

	async private void OnBrowseFieldOrderFile(object sender, EventArgs eventArgs)
	{
		string result = await _filePicker.BrowseForFieldOrderFile();
		if (result != string.Empty)
		{
			FieldOrderEntry.Text = result;
		}
	}

	async private void OnBrowseFieldQualityFile(object sender, EventArgs eventArgs)
	{
		string result = await _filePicker.BrowseForFieldQualityFile();
		if (result != string.Empty)
		{
			FieldQualityEntry.Text = result;
		}
	}

	async private void OnBrowseNameRemappingFile(object sender, EventArgs eventArgs)
	{
		string result = await _filePicker.BrowseForNameRemappingFile();
		if (result != string.Empty)
		{
			NameRemappingEntry.Text = result;
		}
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