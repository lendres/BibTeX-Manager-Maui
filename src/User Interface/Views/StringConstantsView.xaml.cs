using BibTeXLibrary;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;

namespace BibTeXManager.Views;

public partial class StringConstantsView : ContentPage
{
	#region Fields

	private readonly StringConstantsViewModel		_viewModel;
	private readonly bool							_animateScrollToSelection		= false;

	#endregion

	#region Construction

	public StringConstantsView(StringConstantsViewModel viewModel)
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
		StringConstantViewModel	viewModel	= new();
		StringConstantView		view		= new(viewModel);
		object?			result				= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			_viewModel.Insert(viewModel.StringEntry);
		}
	}

	async void OnEditString(object sender, EventArgs eventArgs)
	{
		StringEntry				stringEntry	= new(_viewModel.SelectedItem!);
		StringConstantViewModel	viewModel		= new(stringEntry);
		StringConstantView		view			= new(viewModel);
		object?					result			= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			_viewModel.ReplaceSelected(viewModel.StringEntry);
		}
	}

	async void OnDeleteString(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			_viewModel.Delete();
			StringsDataGrid.ScrollTo(_viewModel.SelectedItem!, ScrollToPosition.Center, _animateScrollToSelection);
		}
	}

	async public void OnOK(object sender, EventArgs eventArgs)
	{
		Dictionary<string, object?> navigationParameter = new()
		{
			{ "NavigationCommand",	"Do Nothing" },
			{ "NavigationObject",	null }
		};
		await Shell.Current.GoToAsync("../", true, navigationParameter);
	}

	async public void OnCancel(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync("../", true);
	}

	#endregion

	#region Methods



	#endregion
}