using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Services;
using Maui.DataGrid;

namespace BibTeXManager.Views;

public class BibliographyPartDataGridView<TViewModel, TPart> : BibliographyPartView<TViewModel> where TViewModel : BibiographyPartDataGridBaseViewModel<TPart> where TPart : class
{
	#region Fields

	private readonly IDialogService _dialogService;

	#endregion

	#region Construction

	public BibliographyPartDataGridView(TViewModel viewModel) :
		base(viewModel)
	{
		_dialogService = MauiProgram.Services.GetRequiredService<IDialogService>();
	}

	#endregion

	#region Properties

	protected bool AnimateScrollToSelection { get; } = false;

	protected DataGrid DataGrid { get; set; } = null!;

	#endregion

	#region Edit

	public bool RequireSearchString()
	{
		return ViewModel.RequireSearchString;
	}

	public bool Find(string searchString)
	{
		return ViewModel.Find(searchString);
	}

	public void SelectNextFoundItem()
	{
		ViewModel.SelectNextFoundItem();
		DataGrid.ScrollTo(ViewModel.SelectedItem!, ScrollToPosition.Center, AnimateScrollToSelection);
	}

	public void OnScrollToSelection(object sender, EventArgs eventArgs)
	{
		if (ViewModel.SelectedItem != null)
		{
			DataGrid.ScrollTo(ViewModel.SelectedItem, ScrollToPosition.Center, AnimateScrollToSelection);
		}
	}

	public void Insert(TPart entry)
	{
		ViewModel.Insert(entry);
		DataGrid.ScrollTo(ViewModel.SelectedItem!, ScrollToPosition.Center, AnimateScrollToSelection);
	}

	public void ReplaceSelected(TPart entry)
	{
		ViewModel.ReplaceSelected(entry);
		DataGrid.ScrollTo(ViewModel.SelectedItem!, ScrollToPosition.Center, AnimateScrollToSelection);
	}

	public async void OnDeleteEntry(object sender, EventArgs eventArgs)
	{
		bool result = await _dialogService.HostingPage!.DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");
		if (result)
		{
			ViewModel.Delete();
			DataGrid.ScrollTo(ViewModel.SelectedItem!, ScrollToPosition.Center, AnimateScrollToSelection);
		}
	}

	#endregion
}