using BibTeXManager.ViewModels;
using DigitalProduction.Maui.ComponentModel;

namespace BibTeXManager.Views;

public partial class FieldProcessorView : ContentView
{
	private FieldProcessorViewModel?		_viewModel;

	public FieldProcessorView()
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

	async private void OnSave(object? sender, EventArgs eventArgs)
	{
		_viewModel!.AddFieldProcessorViewModelCallback?.Invoke(_viewModel);
		await Shell.Current.GoToAsync("../", true, new Dictionary<string, object>
		{
			{ "FieldQualityProcessingFile", string.Empty }
		});
	}

	async private void OnCancel(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync("../", true, new Dictionary<string, object>
		{
			{ "FieldQualityProcessingFile", string.Empty }
		});
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