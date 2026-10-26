using BibTeXManager.ViewModels;

namespace BibTeXManager.Views;

public class BibliographyPartView<TViewModel> : ContentView, IBibliographyPartView where TViewModel : IBibliographyPartViewModel
{
	#region Construction

	public BibliographyPartView(TViewModel viewModel)
	{
		ViewModel = viewModel;
	}

	#endregion

	#region Properties

	protected TViewModel ViewModel { get; set; }

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
}