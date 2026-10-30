using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui;
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
		
		if (_bibliographyEntryMapPicker.SelectedIndex < 0 && _bibliographyEntryMapPicker.Items.Count > -1)
		{
			_bibliographyEntryMapPicker.SelectedIndex = 0;
		}
	}

	private NameMappingViewModel ViewModel { get; set; }

	#region Button Events

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

	private async void OnNewBibliographyEntryMap(object sender, EventArgs eventArgs)
	{
		GetNameViewModel	viewModel	= new(ViewModel.BibliographyEntryTypes!);
		GetNameView			view		= new(viewModel);
		IPopupResult<bool>				result		= await Shell.Current.ShowPopupAsync<bool>(view, new PopupOptions { CanBeDismissedByTappingOutsideOfPopup = false, Shape = null, Shadow = null });

		if (!result.WasDismissedByTappingOutsideOfPopup && result.Result)
		{
			ViewModel.NewBibliographyEntryMap(viewModel.Name);
		}
	}

	private async void OnRenameBibliographyEntryMap(object sender, EventArgs eventArgs)
	{
		GetNameViewModel	viewModel	= new(ViewModel.SelectedType!, ViewModel.BibliographyEntryTypes!);
		GetNameView			view		= new(viewModel);
		IPopupResult<bool>				result		= await Shell.Current.ShowPopupAsync<bool>(view, new PopupOptions { CanBeDismissedByTappingOutsideOfPopup = false, Shape = null, Shadow = null });

		if (!result.WasDismissedByTappingOutsideOfPopup && result.Result)
		{
			ViewModel.RenameBibliographyEntryMap(ViewModel.SelectedType!, viewModel.Name);
		}
	}

	private async void OnDeleteBibliographyEntryMap(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlertAsync("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			ViewModel.DeleteBibliographyEntryMap(ViewModel.SelectedType!);
		}
	}

	private async void OnNewFieldMap(object sender, EventArgs eventArgs)
	{
		NameMapViewModel	viewModel	= new(ViewModel.SelectedBibliographyEntryMap!.InUseTypes);
		NameMapView			view		= new(viewModel);
		IPopupResult<bool>				result		= await Shell.Current.ShowPopupAsync<bool>(view, new PopupOptions { CanBeDismissedByTappingOutsideOfPopup = false, Shape = null, Shadow = null });

		if (!result.WasDismissedByTappingOutsideOfPopup && result.Result)
		{
			ViewModel.Insert(viewModel.NameMap);
		}
	}

	private async void OnEditFieldMap(object sender, EventArgs eventArgs)
	{
		NameMap				nameMap			= new(ViewModel.SelectedItem!);
		NameMapViewModel	viewModel		= new(nameMap, ViewModel.SelectedBibliographyEntryMap!.InUseTypes);
		NameMapView			view			= new(viewModel);
		IPopupResult<bool>				result			= await Shell.Current.ShowPopupAsync<bool>(view, new PopupOptions { CanBeDismissedByTappingOutsideOfPopup = false, Shape = null, Shadow = null });

		if (!result.WasDismissedByTappingOutsideOfPopup && result.Result)
		{
			ViewModel.ReplaceSelected(viewModel.NameMap);
		}
	}

	async void OnDeleteFieldMap(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlertAsync("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			ViewModel.Delete();
		}
	}

	#endregion
}