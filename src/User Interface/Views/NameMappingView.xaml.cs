using BibTeXLibrary;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;

namespace BibTeXManager.Views;

public partial class NameMappingView : ContentPage
{
	public NameMappingView(NameMappingViewModel viewModel)
	{
		InitializeComponent();
		ViewModel		= viewModel;
		BindingContext	= viewModel;
	}

	private NameMappingViewModel ViewModel { get; set; }

	#region Button Events

	private async void OnNewEntry(object sender, EventArgs eventArgs)
	{
		BibEntryMapViewModel	viewModel	= new();
		BibEntryMapView			view		= new(viewModel);
		object? result = await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			ViewModel.Insert(viewModel.BibEntryMap);
		}
	}

	private async void OnEditEntry(object sender, EventArgs eventArgs)
	{
		BibEntryMap				bibEntryMap	= new(ViewModel.SelectedItem!);
		BibEntryMapViewModel	viewModel	= new(bibEntryMap);
		BibEntryMapView			view		= new(viewModel);
		object? result = await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			ViewModel.ReplaceSelected(viewModel.BibEntryMap);
		}
	}

	async private void OnSave(object? sender, EventArgs eventArgs)
	{
		ViewModel.Save();
		Dictionary<string, object?> navigationParameter = new()
		{
			{ "NavigationCommand",  "Do Nothing" },
			{ "NavigationObject",   null }
		};
		await Shell.Current.GoToAsync("../", true, navigationParameter);
	}

	private async void OnCancel(object sender, EventArgs eventArgs)
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
}