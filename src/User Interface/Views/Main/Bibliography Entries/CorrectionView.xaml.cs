using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Controls;
using DigitalProduction.Maui.Views;

namespace BibTeXManager.Views;

public partial class CorrectionView : PopupView
{
	#region Fields

	private readonly CorrectionViewModel _viewModel;

	#endregion

	#region Construction

	public CorrectionView(CorrectionViewModel viewModel)
	{
		BindingContext	= viewModel;
		_viewModel		= viewModel;
		InitializeComponent();
	}

	#endregion

	#region Events

	async void OnYesButtonClicked(object? sender, EventArgs eventArgs)
	{
		await HandleResult(MessageBoxYesNoToAllResult.Yes);
	}

	async void OnYesToAllButtonClicked(object? sender, EventArgs eventArgs)
	{
		await HandleResult(MessageBoxYesNoToAllResult.YesToAll);
	}

	async void OnNoButtonClicked(object? sender, EventArgs eventArgs)
	{
		await HandleResult(MessageBoxYesNoToAllResult.No);
	}

	async new void OnCancelButtonClicked(object? sender, EventArgs eventArgs)
	{
		await HandleResult(MessageBoxYesNoToAllResult.Cancel);
	}

	private async Task HandleResult(MessageBoxYesNoToAllResult result)
	{
		_viewModel.SetResult(result);
		CancellationTokenSource cancelationTokenSource = new(TimeSpan.FromSeconds(5));
		await CloseAsync(result, cancelationTokenSource.Token);
	}

	#endregion
}