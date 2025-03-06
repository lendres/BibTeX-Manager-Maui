using BibTeXLibrary;
using BibTexManager.ViewModels;

namespace BibTexManager.Views;

public partial class EditRawBibEntryForm : ContentPage
{
	private BibEntryViewModel _viewModel;

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
			{ "NavigationCommand", _viewModel.SaveCommand },
			{ "NavigationObject", _viewModel.BibEntry! }
		};
		await Shell.Current.GoToAsync("../", true, navigationParameter);
	}

	async public void OnCancel(object sender, EventArgs eventArgs)
	{
		// Navigate back with a result.
		Dictionary<string, object> navigationParameter = new()
		{
			{ "NavigationCommand", "Cancel" },
			{ "Result", _viewModel.BibEntry! }
		};
		await Shell.Current.GoToAsync("../", true, navigationParameter);
	}

	async void OnPaste(object sender, EventArgs eventArgs)
	{
		string? pastedText	= await Clipboard.Default.GetTextAsync();
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
}