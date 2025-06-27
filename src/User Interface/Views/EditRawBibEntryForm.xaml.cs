using BibTeXLibrary;
using BibtexManager;
using BibTexManager.ViewModels;
using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;

namespace BibTexManager.Views;

public partial class EditRawBibEntryForm : ContentPage
{
	private readonly BibEntryViewModel _viewModel;

	public EditRawBibEntryForm(BibEntryViewModel viewModel)
	{
		InitializeComponent();
		_viewModel		= viewModel;
		BindingContext	= viewModel;
	}

	async public void OnSave(object sender, EventArgs eventArgs)
	{
		// Navigate back with a result.
		Dictionary<string, object> navigationParameter = new()
		{
			{ "NavigationCommand",	_viewModel.SaveCommand },
			{ "NavigationObject",	_viewModel.BibEntry! }
		};
		await Shell.Current.GoToAsync("../", true, navigationParameter);
	}

	async public void OnCancel(object sender, EventArgs eventArgs)
	{
		// Navigate back with a result.
		Dictionary<string, object> navigationParameter = new()
		{
			{ "NavigationCommand",	"Cancel" },
			{ "Result",				_viewModel.BibEntry! }
		};
		await Shell.Current.GoToAsync("../", true, navigationParameter);
	}

	async void OnPaste(object sender, EventArgs eventArgs)
	{
		await Paste();
	}

	async void OnPasteAndCheck(object sender, EventArgs eventArgs)
	{
		await Paste();
		await CheckQuality();
	}

	async void OnCheckQuality(object sender, EventArgs eventArgs)
	{
		await CheckQuality();
	}

	private async Task Paste()
	{
		string? pastedText  = await Clipboard.Default.GetTextAsync();
		if (pastedText == null)
		{
			return;
		}

		int start			= BibEntryEditor.CursorPosition;
		int length			= BibEntryEditor.SelectionLength;
		string originalText	= BibEntryEditor.Text ?? "";
		string newText;

		if (length > 0)
		{
			// Replace the selected text.
			newText = originalText.Remove(start, length).Insert(start, pastedText);
		}
		else
		{
			// Insert at the cursor position.
			newText = originalText.Insert(start, pastedText);
		}

		BibEntryEditor.Text = newText;

		// Move cursor to the end of inserted text.
		BibEntryEditor.CursorPosition = start + pastedText.Length;
		BibEntryEditor.SelectionLength = 0;
	}

	private async Task CheckQuality()
	{
		bool breakNext = false;

		MessageBoxYesNoToAllResult lastDialogResult = MessageBoxYesNoToAllResult.Cancel;

		foreach (TagProcessingData tagProcessingData in _viewModel.CheckQuality())
		{
			// If the processing was cancelled, we break.  We have to loop back around here to give the
			// processing a chance to finish (it was yielded).  Now exit before processing another entry.
			if (breakNext)
			{
				break;
			}

			CorrectionViewModel	viewModel = new(tagProcessingData);

			if (lastDialogResult == MessageBoxYesNoToAllResult.YesToAll)
			{
				viewModel.SetResult(MessageBoxYesNoToAllResult.YesToAll);
				continue;
			}

			CorrectionView		view		= new(viewModel);
			object?				result		= await Shell.Current.ShowPopupAsync(view);

			if (result is MessageBoxYesNoToAllResult messageBoxResult)
			{
				lastDialogResult	= messageBoxResult;
				breakNext			= messageBoxResult == MessageBoxYesNoToAllResult.Cancel;
			}
		}
	}
}