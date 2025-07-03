using BibTeXManager.Importing;
using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class ImportErrorView : PopupView
{
	#region Fields

	private readonly ImportErrorViewModel _viewModel;

	#endregion

	#region Construction

	public ImportErrorView(ImportErrorViewModel viewModel)
	{
		BindingContext	= viewModel;
		_viewModel		= viewModel;
		InitializeComponent();
	}

	#endregion

	#region Events

	async void OnTryAgainButtonClicked(object? sender, EventArgs eventArgs)
	{
		await HandleResult(ImportErrorHandlingType.TryAgain);
	}

	async void OnSkipButtonClicked(object? sender, EventArgs eventArgs)
	{
		await HandleResult(ImportErrorHandlingType.Skip);
	}

	async new void OnCancelButtonClicked(object? sender, EventArgs eventArgs)
	{
		await HandleResult(ImportErrorHandlingType.Cancel);
	}

	private async Task HandleResult(ImportErrorHandlingType result)
	{
		_viewModel.SetResult(result);
		CancellationTokenSource cancelationTokenSource = new(TimeSpan.FromSeconds(5));
		await CloseAsync(result, cancelationTokenSource.Token);
	}

	#endregion
}