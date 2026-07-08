using BibTeXManager.ViewModels;

namespace BibTeXManager.Views;

public partial class FieldProcessorHeaderView : ContentView
{
	private FieldProcessorViewModel?		_viewModel;

	public FieldProcessorHeaderView()
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
}