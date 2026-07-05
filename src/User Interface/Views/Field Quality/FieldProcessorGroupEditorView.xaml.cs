using BibTeXManager.ViewModels;

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