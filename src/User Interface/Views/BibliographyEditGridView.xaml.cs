using BibTeXLibrary;
using BibTeXManager.ViewModels;
using CommunityToolkit.Maui.Views;
using System.ComponentModel;

namespace BibTeXManager.Views;

public partial class BibliographyEditGridView : ContentView
{
	#region Fields

	private readonly BibliographyEditGridViewModel	_viewModel;
	private bool									_updatingSelectedItem;
	private readonly bool							_animateScrollToSelection = false;

	#endregion

	#region Construction

	public BibliographyEditGridView()
	{
		InitializeComponent();

		_viewModel		=  MauiProgram.Services.GetRequiredService<BibliographyEditGridViewModel>(); ;
		BindingContext	= _viewModel;

		Loaded += OnLoaded;
	}

	#endregion

	#region Properties

	public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
		nameof(SelectedItem),
		typeof(object),
		typeof(BibliographyEditGridView),
		null,
		BindingMode.TwoWay,
		propertyChanged: OnSelectedItemChanged);

	public object? SelectedItem
	{
		get => GetValue(SelectedItemProperty);
		set => SetValue(SelectedItemProperty, value);
	}

	private void OnLoaded(object? sender, EventArgs eventArgs)
	{
		if (_viewModel is INotifyPropertyChanged notifyPropertyChanged)
		{
			notifyPropertyChanged.PropertyChanged += OnViewModelPropertyChanged;
		}

		SelectedItem = _viewModel.SelectedItem;
	}

	private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
	{
		BibliographyEditGridView bibliographyEditGrid = (BibliographyEditGridView)bindable;

		if (bibliographyEditGrid._updatingSelectedItem)
		{
			return;
		}

		if (!Equals(bibliographyEditGrid._viewModel.SelectedItem, newValue))
		{
			bibliographyEditGrid._updatingSelectedItem		= true;
			bibliographyEditGrid._viewModel.SelectedItem	= (BibEntry)newValue;
			bibliographyEditGrid._updatingSelectedItem		= false;
		}
	}

	private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
	{
		if (eventArgs.PropertyName != nameof(BibliographyEditGridViewModel.SelectedItem))
		{
			return;
		}

		if (!Equals(SelectedItem, _viewModel.SelectedItem))
		{
			_updatingSelectedItem	= true;
			SelectedItem			= _viewModel.SelectedItem;
			_updatingSelectedItem	= false;
		}
	}

	#endregion

	#region File

	public async void New()
	{
		await _viewModel.New();
	}

	public async void Open()
	{
		await _viewModel.Open();
	}

	public async void Close()
	{
		await _viewModel.Close();
	}

	#endregion

	#region Edit

	public bool RequireSearchString()
	{
		return _viewModel.RequireSearchString;
	}

	public bool Find(string searchString)
	{
		return _viewModel.Find(searchString);
	}

	public void SelectNextFoundItem()
	{
		_viewModel.SelectNextFoundItem();
		BibliographyDataGrid.ScrollTo(_viewModel.SelectedItem!, ScrollToPosition.Center, _animateScrollToSelection);
	}

	public void OnScrollToSelection(object sender, EventArgs eventArgs)
	{
		if (_viewModel.SelectedItem != null)
		{
			BibliographyDataGrid.ScrollTo(_viewModel.SelectedItem, ScrollToPosition.Center, _animateScrollToSelection);
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
		TemplateSelectionViewModel viewModel = new(_viewModel.Project.BibEntryInitialization.TemplateNames);
		TemplateSelectionView view = new(viewModel);
		object? result = await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			BibEntry entry = BibEntry.NewBibEntryFromTemplate(_viewModel.Project.BibEntryInitialization, viewModel.Template);

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
			{ "BibEntry", _viewModel.SelectedItem! }
		});
	}

	public async void OnDeleteBibEntry(object sender, EventArgs eventArgs)
	{
//bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");
bool result = true;
		if (result)
		{
			_viewModel.Delete();
			BibliographyDataGrid.ScrollTo(_viewModel.SelectedItem!, ScrollToPosition.Center, _animateScrollToSelection);
		}
	}

	#endregion

	#region Methods

	public void Insert(BibEntry bibEntry)
	{
		_viewModel.Insert(bibEntry);
		BibliographyDataGrid.ScrollTo(_viewModel.SelectedItem!, ScrollToPosition.Center, _animateScrollToSelection);
	}

	public void ReplaceSelected(BibEntry bibEntry)
	{
		_viewModel.ReplaceSelected(bibEntry);
		BibliographyDataGrid.ScrollTo(_viewModel.SelectedItem!, ScrollToPosition.Center, _animateScrollToSelection);
	}

	#endregion
}