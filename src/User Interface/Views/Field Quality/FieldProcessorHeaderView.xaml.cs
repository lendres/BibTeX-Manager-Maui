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
		_viewModel!.SaveFieldProcessorCallback?.Invoke(_viewModel.FieldProcessor!);
		// We have to add an empty to prevent the file from being reloaded. Otherwise, the previous file will be reloaded and the changes will be lost.
		await Shell.Current.GoToAsync("../", true, new Dictionary<string, object>
		{
			{ "FieldQualityProcessingFile", string.Empty }
		});
	}

	async private void OnCancel(object sender, EventArgs eventArgs)
	{
		// We have to add an empty to prevent the file from being reloaded. Otherwise, the previous file will be reloaded and the changes will be lost.
		await Shell.Current.GoToAsync("../", true, new Dictionary<string, object>
		{
			{ "FieldQualityProcessingFile", string.Empty }
		});
	}
}