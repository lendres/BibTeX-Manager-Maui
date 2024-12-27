using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Views;
using DigitalProduction.Controls;
using DigitalProduction.ViewModels;
using DigitalProduction.Views;
using BibTexManager.ViewModels;
using BibTexManager.Views;
using Microsoft.Maui.Controls;

namespace BibTexManager.Views;

public partial class MainPage : DigitalProductionMainPage
{


	#region Construction

	public MainPage()
	{
		InitializeComponent();
	}

	#endregion

	#region Button Events

	async void OnNew(object sender, EventArgs eventArgs)
	{
		//TranslationMatrix translationMatrix = TranslationMatrix.CreateNewTranslationMatrix(TranslationMatrixNewName, InputProcessor, InputFile);

		await Shell.Current.GoToAsync(nameof(EditRawBibEntryForm), true, new Dictionary<string, object>
		{
			{"AddMode",  true},
		});

		//ConfigurationsViewModel? configurationsViewModel = BindingContext as ConfigurationsViewModel;
		//System.Diagnostics.Debug.Assert(configurationsViewModel != null);

		//ConfigurationViewModel	viewModel	= new(Interface.ConfigurationList?.ConfigurationNames ?? []);
		//ConfigurationView		view		= new(viewModel);
		//object?					result		= await Shell.Current.ShowPopupAsync(view);

		//if (result is bool boolResult && boolResult)
		//{
		//	configurationsViewModel?.Insert(viewModel.Configuration);
		//}
	}

	#endregion
}