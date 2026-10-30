using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;

namespace BibTeXManager.Views;

public partial class FieldProcessorGroupView : ContentPage
{
    #region Fields

    private readonly FieldProcessorGroupViewModel _viewModel;

    #endregion

    #region Construction

    public FieldProcessorGroupView(FieldProcessorGroupViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    #endregion

    #region Methods

	private async void OnNewProcessor(object sender, EventArgs eventArgs)
	{
		AddFieldProcessorViewModel	viewModel	= new();
		AddFieldProcessorView		view		= new(viewModel);
		IPopupResult<bool> result = await Shell.Current.ShowPopupAsync<bool>(view, new PopupOptions { CanBeDismissedByTappingOutsideOfPopup = false, Shape = null, Shadow = null });

		if (!result.WasDismissedByTappingOutsideOfPopup && result.Result)
		{
			string viewName = viewModel.SelectedType + "View";
			await Shell.Current.GoToAsync(viewName, true, new Dictionary<string, object?>
			{
				{ "SaveFieldProcessorCallback", new Action<FieldProcessor>(_viewModel.AddProcessor) }
			});
		}
	}

	private async void OnEditProcessor(object sender, EventArgs eventArgs)
	{
		string viewName = _viewModel!.SelectedProcessorViewModel!.FieldProcessor.XsiType + "View";
		await Shell.Current.GoToAsync(viewName, true, new Dictionary<string, object?>
		{
			{ "SaveFieldProcessorCallback", new Action<FieldProcessor>(_viewModel.UpdateProcessor) },
			{ "FieldProcessor", _viewModel!.SelectedProcessorViewModel!.FieldProcessor }
		});
	}

	private async void OnDeleteProcessor(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			_viewModel.DeleteProcessor();
		}
	}

    private async void OnSaveAndReturn(object? sender, EventArgs eventArgs)
    {
        _viewModel.Save();
        await Shell.Current.GoToAsync("../");
    }

    private async void OnSave(object? sender, EventArgs eventArgs)
    {
        _viewModel.Save();
    }

    private async void OnCancel(object? sender, EventArgs eventArgs)
    {
        await Shell.Current.GoToAsync("../");
    }

    #endregion
}