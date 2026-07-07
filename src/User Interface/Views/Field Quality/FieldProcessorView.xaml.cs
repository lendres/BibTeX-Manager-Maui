using BibTeXManager.ViewModels;

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
			_viewModel = value;
			BindingContext = value;
		}
	}

	async private void OnSave(object? sender, EventArgs eventArgs)
	{
		//_viewModel.Save();
		Dictionary<string, object?> navigationParameter = new()
		{
			{ "NavigationCommand",  "Do Nothing" },
			{ "NavigationObject",   null }
		};
		await Shell.Current.GoToAsync("../", true); //, navigationParameter);
	}

	async private void OnCancel(object sender, EventArgs eventArgs)
	{
		// Navigate back with a result.
		Dictionary<string, object?> navigationParameter = new()
		{
			{ "NavigationCommand",  "Do Nothing" },
			{ "NavigationObject",   null }
		};
		await Shell.Current.GoToAsync("../", true); //, navigationParameter);
	}
}