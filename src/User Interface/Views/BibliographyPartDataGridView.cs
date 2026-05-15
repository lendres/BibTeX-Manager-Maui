using BibTeXManager.ViewModels;
using Maui.DataGrid;

namespace BibTeXManager.Views;

public class BibliographyPartDataGridView<TViewModel, TPart> : ContentView where TViewModel : BibiographyPartDataGridBaseViewModel<TPart> where TPart : class
{
	#region Construction

	public BibliographyPartDataGridView(TViewModel viewModel)
	{
		ViewModel = viewModel;
	}

	#endregion

	#region Properties

	protected TViewModel ViewModel { get; set; }

	protected bool AnimateScrollToSelection { get; } = false;

	protected DataGrid DataGrid { get; set; } = null!;

	#endregion

	#region File

	public async void New()
	{
		await ViewModel.New();
	}

	public async void Open()
	{
		await ViewModel.Open();
	}

	public async void Close()
	{
		await ViewModel.Close();
	}

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
		//bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");
		bool result = true;
		if (result)
		{
			ViewModel.Delete();
			DataGrid.ScrollTo(ViewModel.SelectedItem!, ScrollToPosition.Center, AnimateScrollToSelection);
		}
	}

	#endregion
}