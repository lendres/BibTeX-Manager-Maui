using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;

namespace BibTeXManager.Views;

public partial class FieldProcessorGroupEditorView : ContentPage
{
    #region Fields

    private readonly FieldProcessorGroupEditorViewModel _viewModel;

    #endregion

    #region Construction

    public FieldProcessorGroupEditorView(FieldProcessorGroupEditorViewModel viewModel)
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
		object? result = await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			//_viewModel.AddProcessor();
			string viewName = viewModel.SelectedType + "View";
			await Shell.Current.GoToAsync(viewName, true, new Dictionary<string, object>
			{
				{ "AddFieldProcessorViewModelCallback", new Action<FieldProcessorViewModel>(_viewModel.AddProcessor) }
			});
		}
	}

	private async void OnEditProcessor(object sender, EventArgs eventArgs)
	{
		//GetNameViewModel	viewModel	= new(_viewModel.SelectedProcessor!, _viewModel.TemplateNames!.ToList());
		//GetNameView			view		= new(viewModel);
		//object?				result		= await Shell.Current.ShowPopupAsync(view);

		//if (result is bool boolResult && boolResult)
		//{
		//	_viewModel.RenameTemplate(_viewModel.SelectedTemplate!, viewModel.Name);
		//}
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