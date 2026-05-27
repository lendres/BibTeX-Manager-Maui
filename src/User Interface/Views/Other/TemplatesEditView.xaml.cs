using BibTeXLibrary;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;

namespace BibTeXManager.Views;

public partial class TemplatesEditView : ContentPage
{
	readonly TemplatesEditViewModel _viewModel;

	readonly IBibTeXFilePicker			_filePicker		= IPlatformApplication.Current!.Services.GetRequiredService<IBibTeXFilePicker>();

	public TemplatesEditView(TemplatesEditViewModel viewModel)
	{
		InitializeComponent();
		_viewModel		= viewModel;
		BindingContext	= viewModel;

		if (_templatePicker.SelectedIndex < 0 && _templatePicker.Items.Count > -1)
		{
			_templatePicker.SelectedIndex = 0;
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

	#region Button Events

	private async void OnNewNameMap(object sender, EventArgs eventArgs)
	{
		NameMapViewModel	viewModel	= new(_viewModel.Initializer.TypeNames);
		NameMapView			view		= new(viewModel);
		object?				result		= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			_viewModel.Insert(viewModel.FieldNameMap);
		}
	}

	private async void OnEditNameMap(object sender, EventArgs eventArgs)
	{
		NameMap				nameMap		= new(_viewModel.SelectedItem!);
		NameMapViewModel	viewModel	= new(nameMap, _viewModel.Initializer.TypeNames);
		NameMapView			view		= new(viewModel);
		object? result = await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			_viewModel.ReplaceSelected(viewModel.FieldNameMap);
		}
	}

	async void OnDeleteNameMap(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			_viewModel.Delete();
		}
	}

	#endregion
}