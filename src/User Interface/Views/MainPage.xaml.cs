using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Views;
using DigitalProduction.Controls;
using DigitalProduction.ViewModels;
using DigitalProduction.Views;
using BibTexManager.ViewModels;

namespace BibTexManager.Views;

public partial class MainPage : DigitalProductionMainPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	/// <summary>
	/// Override to set the height to something usable.
	/// </summary>
	protected override void OnAppearing()
	{
		base.OnAppearing();
	}

	async void OnHelp(object sender, EventArgs eventArgs)
	{
		System.Reflection.Assembly? entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
		System.Diagnostics.Debug.Assert(entryAssembly != null);
		string url = DigitalProduction.Reflection.Assembly.DocumentationAddress(entryAssembly);
		await Launcher.OpenAsync(url);
	}

	async void OnAbout(object sender, EventArgs eventArgs)
	{
		AboutView1 view = new(new AboutViewModel(true));
		_ = await Shell.Current.ShowPopupAsync(view);
	}



	//public async void OnBrowseForXmlile(object sender, EventArgs eventArgs)
	//{
	//	PickOptions pickOptions = new()
	//	{
	//		PickerTitle = "Select an XML File",
	//		FileTypes   = DigitalProduction.IO.FileTypes.Xml
	//	};
	//	FileResult? result = await BrowseForFile(pickOptions);
	//	if (result != null)
	//	{
	//		Entry.Text = result.FullPath;
	//	}
	//}


	public static async Task<FileResult?> BrowseForFile(PickOptions options)
	{
		try
		{
			return await FilePicker.PickAsync(options);
		}
		catch
		{
			//(Exception exception)
			//string message = exception.Message;
			// The user canceled or something went wrong.
		}
		return null;
	}

}