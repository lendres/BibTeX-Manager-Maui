using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Services;
using Maui.DataGrid;
using System.ComponentModel;

namespace BibTeXManager.Views;

public abstract class BibliographyPartDataGridView<TViewModel, TPart> : BibliographyPartView<TViewModel>, IBibliographyPartDataGridView
	where TViewModel : BibiographyPartDataGridBaseViewModel<TPart> where TPart : class
{
	#region Fields

	private readonly IDialogService _dialogService;

	#endregion

	#region Construction

	public BibliographyPartDataGridView(TViewModel viewModel) :
		base(viewModel)
	{
		_dialogService = MauiProgram.Services.GetRequiredService<IDialogService>();

		Loaded += OnLoaded;
	}

	#endregion

	#region Properties

	public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
		nameof(SelectedItem),
		typeof(object),
		typeof(BibliographyEditView),
		null,
		BindingMode.OneWayToSource);

	public object? SelectedItem
	{
		get => GetValue(SelectedItemProperty);
		private set => SetValue(SelectedItemProperty, value);
	}

	private void OnLoaded(object? sender, EventArgs eventArgs)
	{
		ViewModel.PropertyChanged += OnViewModelPropertyChanged;

		SelectedItem = ViewModel.SelectedItem;
	}

	private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
	{
		if (eventArgs.PropertyName == nameof(BibliographyEditViewModel.SelectedItem))
		{
			SelectedItem = ViewModel.SelectedItem;
		}
	}

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

	public abstract void OnNewEntry(object sender, EventArgs eventArgs);

	public abstract void OnEditEntry(object sender, EventArgs eventArgs);

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