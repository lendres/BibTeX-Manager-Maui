using BibtexManager;
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
		object?				result		= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			//_viewModel.NewTemplate(viewModel.Name);
		}
	}

	private async void OnRenameFieldProcessingGroup(object sender, EventArgs eventArgs)
	{
		GetNameViewModel	viewModel	= new(_viewModel.SelectedInclude!.IncludeName, _viewModel.AvailableIncludeNames);
		GetNameView			view		= new(viewModel);
		object?				result		= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			//_viewModel.Rename(_viewModel.SelectedIncludeName!, viewModel.Name);
		}
	}

	private async void OnEditFieldProcessingGroup(object sender, EventArgs eventArgs)
	{
		GetNameViewModel	viewModel	= new(_viewModel.SelectedInclude!.IncludeName, _viewModel.AvailableIncludeNames);
		GetNameView			view		= new(viewModel);
		object?				result		= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			//_viewModel.Rename(_viewModel.SelectedIncludeName!, viewModel.Name);
		}
	}

	private async void OnDeleteFieldProcessingGroup(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			//_viewModel.Delete(_viewModel.SelectedIncludeName!);
		}
	}
}