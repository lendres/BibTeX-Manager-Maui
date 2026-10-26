using BibTeXManager.ViewModels;
using DigitalProduction.Maui.ComponentModel;

namespace BibTeXManager.Views;

public partial class FieldProcessorFieldsView : ContentView
{
	private FieldProcessorViewModel?		_viewModel;

	public FieldProcessorFieldsView()
	{
		InitializeComponent();

	}

	public FieldProcessorViewModel ViewModel
	{
		get => _viewModel ?? throw new InvalidOperationException("ViewModel is not set");
		set
		{
			_viewModel		= value;
			BindingContext	= value;
		}
	}

	#region Fields

	private void ButtonPressed(object? sender, EventArgs e)
	{
		_viewModel!.BeginButtonPress();
	}

	private void EntryFocused(object? sender, FocusEventArgs e)
	{
		if (sender is not Entry entry)
		{
			return;
		}

		_viewModel!.SelectedField = entry.BindingContext as ObservableString;
	}

	private void EntryUnfocused(object? sender, FocusEventArgs e)
	{
		if (_viewModel!.ShouldIgnoreUnfocus())
		{
			return;
		}

		_viewModel.SelectedField = null;
	}

	#endregion
}