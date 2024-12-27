namespace BibTexManager.Views;

public partial class EditRawBibEntryForm : ContentPage
{
	public EditRawBibEntryForm()
	{
		InitializeComponent();
	}

	async public void OnSave(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync("../");
	}

	async public void OnCancel(object sender, EventArgs eventArgs)
	{
		await Shell.Current.GoToAsync("../");
	}
}