using BibTeXLibrary;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;
using DigitalProduction.Maui.Storage;
using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class StringConstantsView : ContentPage
{
	#region Fields

	private readonly StringsViewModel		_viewModel;

	#endregion

	#region Construction

	public StringConstantsView(StringsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext	= viewModel;
		_viewModel		= viewModel;
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



	#endregion
}