using BibTeXLibrary;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;

namespace BibTeXManager.Views;

public partial class StringsEditView : ContentView
{
	#region Fields

	private readonly StringsEditViewModel	_viewModel;
	private readonly bool					_animateScrollToSelection		= false;

	#endregion

	#region Construction

	public StringsEditView()
	{
		InitializeComponent();

		_viewModel					= MauiProgram.Services.GetRequiredService<StringsEditViewModel>();
		_mainGrid.BindingContext	= _viewModel;
	}

	#endregion

	#region Properties


	#endregion

	#region Menu Events


	#endregion

	#region Button Events

	async private void OnNewString(object sender, EventArgs eventArgs)
	{
		StringEditViewModel	viewModel	= new();
		StringEditView		view		= new(viewModel);
		object?				result				= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			_viewModel.Insert(viewModel.StringEntry);
		}
	}

	async private void OnEditString(object sender, EventArgs eventArgs)
	{
		StringEntry			stringEntry	= new(_viewModel.SelectedItem!);
		StringEditViewModel	viewModel	= new(stringEntry);
		StringEditView		view		= new(viewModel);
		object?				result		= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			_viewModel.ReplaceSelected(viewModel.StringEntry);
		}
	}

	async private void OnDeleteString(object sender, EventArgs eventArgs)
	{
//bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");
bool result = true;
		if (result)
		{
			_viewModel.Delete();
			StringsDataGrid.ScrollTo(_viewModel.SelectedItem!, ScrollToPosition.Center, _animateScrollToSelection);
		}
	}

	async private void OnOK(object sender, EventArgs eventArgs)
	{
		Dictionary<string, object?> navigationParameter = new()
		{
			{ "NavigationCommand",	"Do Nothing" },
			{ "NavigationObject",	null }
		};
		await Shell.Current.GoToAsync("../", true, navigationParameter);
	}

	async private void OnCancel(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync("../", true);
	}

	#endregion

	#region Methods



	#endregion
}