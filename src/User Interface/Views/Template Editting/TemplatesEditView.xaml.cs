using BibTeXLibrary;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.ComponentModel;

namespace BibTeXManager.Views;

public partial class TemplatesEditView : ContentPage
{
	#region Fields

	readonly TemplatesEditViewModel		_viewModel;

	#endregion

	#region Construction

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

	#endregion

	#region Page Button Events

	async private void OnSaveAndReturn(object? sender, EventArgs eventArgs)
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

	#endregion

	#region Name Map Button Events

	private async void OnNewNameMap(object sender, EventArgs eventArgs)
	{
		TypeToTemplateViewModel viewModel	= new(_viewModel.Initializer.TypeNames);
		TypeToTemplateView		view		= new(viewModel);
		object?					result		= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			_viewModel.Insert(viewModel.NameMap);
		}
	}

	private async void OnEditNameMap(object sender, EventArgs eventArgs)
	{
		NameMap					nameMap		= new(_viewModel.SelectedItem!);
		TypeToTemplateViewModel	viewModel	= new(nameMap, _viewModel.Initializer.TypeNames);
		TypeToTemplateView		view		= new(viewModel);
		object? result = await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			_viewModel.ReplaceSelected(viewModel.NameMap);
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

	#region Template Editting Button Events

	private void ButtonPressed(object? sender, EventArgs e)
	{
		_viewModel.BeginButtonPress();
	}

	private void EntryFocused(object? sender, FocusEventArgs e)
	{
		if (sender is not Entry entry)
		{
			return;
		}

		_viewModel.SelectedField = entry.BindingContext as ObservableString;
	}

	private void EntryUnfocused(object? sender, FocusEventArgs e)
	{
		if (_viewModel.ShouldIgnoreUnfocus())
		{
			return;
		}

		_viewModel.SelectedField = null;
	}

	#endregion
}