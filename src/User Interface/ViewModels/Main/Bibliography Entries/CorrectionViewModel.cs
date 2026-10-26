using BibTeXManager;
using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Controls;

namespace BibTeXManager.ViewModels;

public partial class CorrectionViewModel : ObservableObject
{
	#region Construction

	public CorrectionViewModel(FieldProcessingData tagProcessingData)
	{
		FieldProcessingData = tagProcessingData;
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string				Title { get; set; }					= "Replace Text?";

	[ObservableProperty]
	public partial FieldProcessingData	FieldProcessingData { get; set; }

	[ObservableProperty]
	public partial string				ReplacementText  { get; set; }		= "";

	public bool							ReplaceText
	{
		get => FieldProcessingData.Correction.ReplaceText;
		set => FieldProcessingData.Correction.ReplaceText = value;
	}

	#endregion

	#region Events

	partial void OnTitleChanged(string value)
	{
		ReplacementText = value;
	}
	
	partial void OnFieldProcessingDataChanged(FieldProcessingData value)
	{
		ReplacementText = value.Correction.ReplacementText;
	}

	#endregion

	#region Methods

	public void SetResult(MessageBoxYesNoToAllResult dialogResult)
	{
		switch (dialogResult)
		{
			case MessageBoxYesNoToAllResult.YesToAll:
				FieldProcessingData.AcceptAll = true;
				FieldProcessingData.Correction.ReplaceText        = true;
				FieldProcessingData.Correction.ReplacementText	= ReplacementText;
				break;

			case MessageBoxYesNoToAllResult.Yes:
				FieldProcessingData.Correction.ReplaceText		= true;
				FieldProcessingData.Correction.ReplacementText	= ReplacementText;
				break;

			case MessageBoxYesNoToAllResult.No:
				FieldProcessingData.Correction.ReplaceText		= false;
				break;

			case MessageBoxYesNoToAllResult.Cancel:
				break;

			default:
				throw new Exception("Bad value.");
		}
	}

	#endregion
}