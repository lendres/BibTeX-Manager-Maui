using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui;
using BibTeXLibrary;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;

namespace BibTeXManager.Views;

public partial class StringsEditView : BibliographyPartDataGridView<StringsEditViewModel, StringEntry>
{
	#region Fields
	#endregion

	#region Construction

	public StringsEditView() :
		base(MauiProgram.Services.GetRequiredService<StringsEditViewModel>())
	{
		InitializeComponent();
		_mainGrid.BindingContext	= ViewModel;
		DataGrid					= _dataGrid;
	}

	#endregion

	#region Properties
	#endregion

	#region Button Events

	public override async void OnNewEntry(object sender, EventArgs eventArgs)
	{
		StringEditViewModel	viewModel	= new();
		StringEditView		view		= new(viewModel);
		IPopupResult<bool>				result		= await Shell.Current.ShowPopupAsync<bool>(view, new PopupOptions { CanBeDismissedByTappingOutsideOfPopup = false, Shape = null, Shadow = null });

		if (!result.WasDismissedByTappingOutsideOfPopup && result.Result)
		{
			ViewModel.Insert(viewModel.StringEntry);
		}
	}

	public override async void OnEditEntry(object sender, EventArgs eventArgs)
	{
		StringEntry			stringEntry	= new(ViewModel.SelectedItem!);
		StringEditViewModel	viewModel	= new(stringEntry);
		StringEditView		view		= new(viewModel);
		IPopupResult<bool>				result		= await Shell.Current.ShowPopupAsync<bool>(view, new PopupOptions { CanBeDismissedByTappingOutsideOfPopup = false, Shape = null, Shadow = null });

		if (!result.WasDismissedByTappingOutsideOfPopup && result.Result)
		{
			ViewModel.ReplaceSelected(viewModel.StringEntry);
		}
	}

	#endregion
}