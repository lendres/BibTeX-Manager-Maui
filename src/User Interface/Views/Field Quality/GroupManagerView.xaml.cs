using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;

namespace BibTeXManager.Views;

public partial class GroupManagerView : ContentPage
{
	private readonly GroupManagerViewModel _viewModel;

	public GroupManagerView(GroupManagerViewModel viewModel)
	{
		InitializeComponent();

		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	private async void OnSaveAndReturn(object? sender, EventArgs eventArgs)
	{
		_viewModel.Save();

		Dictionary<string, object?> navigationParameter = new()
		{
			{ "NavigationCommand", "Do Nothing" },
			{ "NavigationObject", null }
		};

		await Shell.Current.GoToAsync("../", true, navigationParameter);
	}

	private void OnSave(object? sender, EventArgs eventArgs)
	{
		_viewModel.Save();
	}

	private async void OnCancel(object? sender, EventArgs eventArgs)
	{
		Dictionary<string, object?> navigationParameter = new()
		{
			{ "NavigationCommand", "Do Nothing" },
			{ "NavigationObject", null }
		};

		await Shell.Current.GoToAsync("../", true, navigationParameter);
	}

	private async void OnNewFieldProcessingGroup(object sender, EventArgs eventArgs)
	{
		GetNameViewModel	viewModel	= new(_viewModel.AvailableIncludeNames);
		GetNameView			view		= new(viewModel);
		IPopupResult<bool>				result		= await Shell.Current.ShowPopupAsync<bool>(view, new PopupOptions { CanBeDismissedByTappingOutsideOfPopup = false, Shape = null, Shadow = null });

		if (!result.WasDismissedByTappingOutsideOfPopup && result.Result)
		{
			_viewModel.NewFieldProcessingGroup(viewModel.Name);
		}
	}

	private async void OnRenameFieldProcessingGroup(object sender, EventArgs eventArgs)
	{
		GetNameViewModel	viewModel	= new(_viewModel.SelectedFieldProcessingGroup!.Name, _viewModel.AvailableIncludeNames);
		GetNameView			view		= new(viewModel);
		IPopupResult<bool>				result		= await Shell.Current.ShowPopupAsync<bool>(view, new PopupOptions { CanBeDismissedByTappingOutsideOfPopup = false, Shape = null, Shadow = null });

		if (!result.WasDismissedByTappingOutsideOfPopup && result.Result)
		{
			_viewModel.RenameFieldProcessingGroup(viewModel.Name);
		}
	}

	private async void OnEditFieldProcessingGroup(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync(nameof(FieldProcessorGroupView), true, new Dictionary<string, object>
		{
			{ "FieldQualityProcessingFile", _viewModel.GetSelectedQualityFilePath() }
		});
	}

	private async void OnDeleteFieldProcessingGroup(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlertAsync("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			_viewModel.DeleteFieldProcessingGroup();
		}
	}
}