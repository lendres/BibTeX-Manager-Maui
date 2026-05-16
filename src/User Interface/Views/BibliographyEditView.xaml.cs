using BibTeXLibrary;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;
using System.ComponentModel;

namespace BibTeXManager.Views;

public partial class BibliographyEditView : BibliographyPartDataGridView<BibliographyEditViewModel, BibEntry>
{
	#region Construction

	public BibliographyEditView() :
		base(MauiProgram.Services.GetRequiredService<BibliographyEditViewModel>())
	{
		InitializeComponent();
		_mainGrid.BindingContext	= ViewModel;
		DataGrid					= _dataGrid;

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

	#endregion

	#region Button Events

	public async void OnNewBibEntry(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync(nameof(EditRawBibEntryForm), true, new Dictionary<string, object>
		{
			{ "AddMode",  true }
		});
	}

	public async void OnNewBibEntryFromTemplate(object sender, EventArgs eventArgs)
	{
		TemplateSelectionViewModel	viewModel	= new(ViewModel.Project.BibEntryInitialization.TemplateNames);
		TemplateSelectionView		view		= new(viewModel);
		object? result = await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			BibEntry entry = BibEntry.NewBibEntryFromTemplate(ViewModel.Project.BibEntryInitialization, viewModel.Template);

			await Shell.Current.GoToAsync(nameof(EditRawBibEntryForm), true, new Dictionary<string, object>
			{
				{ "AddMode",  true },
				{ "BibEntry", entry }
			});
		}
	}

	public async void OnEditBibEntry(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync(nameof(EditRawBibEntryForm), true, new Dictionary<string, object>
		{
			{ "AddMode",  false },
			{ "BibEntry", ViewModel.SelectedItem! }
		});
	}

	#endregion
}