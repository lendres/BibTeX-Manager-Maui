using BibTeXLibrary;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;
using DigitalProduction.Maui.Storage;
using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class StringsEditView : DigitalProductionMainPage
{
	#region Fields

	private readonly MainViewModel		_viewModel;

	#endregion

	#region Construction

	public StringsEditView(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_viewModel = viewModel;

		if (Preferences.LoadLastProjectAtStartUp)
		{
			OpenLastProject();
		}
	}

	#endregion

	#region Properties


	#endregion

	#region Menu Events


	#endregion

	#region Button Events

	async void OnNewString(object sender, EventArgs eventArgs)
	{

	}

	async void OnEditString(object sender, EventArgs eventArgs)
	{

	}

	async void OnDeleteString(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			_viewModel.Delete();
			StringsDataGrid.ScrollTo(_viewModel.SelectedItem!, ScrollToPosition.Center, true);
		}
	}

	async public void OnOK(object sender, EventArgs eventArgs)
	{
		// TODO: Save strings.
		await Shell.Current.GoToAsync("../", true);
	}

	async public void OnCancel(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync("../", true);
	}

	#endregion

	#region Methods

	private void OpenLastProject()
	{
		//_viewModel.OpenProjectWithPathSave(Preferences.RecentPathsManagerService.TopPath);
		List<string> paths = Preferences.RecentPathsManagerService.GetRecentPaths();
		if (paths.Count > 0)
		{
			_viewModel.Open(paths[0]);
		}
	}

	private async Task<string?> BrowseForInputFile()
	{
		MainViewModel? viewModel = BindingContext as MainViewModel;
		System.Diagnostics.Debug.Assert(viewModel != null);

		try
		{
			PickOptions pickOptions = new() { PickerTitle="Select an Input File" }; //, FileTypes=viewModel.GetInputFileTypes() };
			FileResult? result      = await BrowseForFile(pickOptions);

			if (result != null)
			{
				return result.FullPath;
			}
		}
		catch (Exception exception)
		{
			await DisplayAlert("Error", "An exception occured:"+Environment.NewLine+exception.Message, "OK");
		}

		return null;
	}

	public static async Task<FileResult?> BrowseForFile(PickOptions options)
	{
		try
		{
			return await FilePicker.PickAsync(options);
		}
		catch
		{
			// The user canceled or something went wrong.
		}

		return null;
	}

	#endregion
}