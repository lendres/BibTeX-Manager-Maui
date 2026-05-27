using BibTeXManager.ViewModels;

namespace BibTeXManager.Views;

public partial class TemplatesEditView : ContentPage
{
	readonly TemplatesEditViewModel _viewModel;

	readonly IBibTeXFilePicker			_filePicker		= IPlatformApplication.Current!.Services.GetRequiredService<IBibTeXFilePicker>();

	public TemplatesEditView(TemplatesEditViewModel viewModel)
	{
		InitializeComponent();
		_viewModel		= viewModel;
		BindingContext	= viewModel;
	}

	async private void OnSave(object? sender, EventArgs eventArgs)
	{
		_viewModel.Save();
		Dictionary<string, object?> navigationParameter = new()
		{
			{ "NavigationCommand",  "Do Nothing" },
			{ "NavigationObject",   null }
		};
		await Shell.Current.GoToAsync("../", true, navigationParameter);
	}

	async private void OnCancel(object sender, EventArgs eventArgs)
	{
		// Navigate back with a result.
		Dictionary<string, object?> navigationParameter = new()
		{
			{ "NavigationCommand",  "Do Nothing" },
			{ "NavigationObject",   null }
		};
		await Shell.Current.GoToAsync("../", true, navigationParameter);
	}
}