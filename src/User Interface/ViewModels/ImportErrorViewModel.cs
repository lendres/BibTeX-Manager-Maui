using BibTeXManager;
using BibTeXManager.Importing;
using BibTeXManager.Project;
using BibTeXManager.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Controls;
using DocumentFormat.OpenXml.Bibliography;
using System.Speech.Recognition;

namespace BibTeXManager.ViewModels;

public partial class ImportErrorViewModel : ObservableObject
{
	#region Fields

	private readonly IBulkImporter   _importer;
	private readonly ImportResult	_importResult;

	#endregion

	#region Construction

	public ImportErrorViewModel(IBulkImporter importer, ImportResult importResult)
	{
		_importer       = importer;
		_importResult	= importResult;

		switch (importResult.Result)
		{
			case ResultType.NotFound:
				Title = "Item Not Found";

				Message = "The item was not found during the search." + Environment.NewLine + Environment.NewLine;
				if (!string.IsNullOrEmpty(importResult.Message))
				{
					Message += importResult.Message + Environment.NewLine + Environment.NewLine;
				}
				Message += "Do you wish to try again?";
				break;

			case ResultType.Error:
				Title = "Import Error";

				Message = "An error occured during the search." + Environment.NewLine + Environment.NewLine +
						importResult.Message + Environment.NewLine + Environment.NewLine +
						"Do you wish to try again?";
				break;

			default:
				throw new Exception("Unknown import result type: " + importResult.Result);
		}
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string				Title { get; set; }

	[ObservableProperty]
	public partial string				Message { get; set; }

	#endregion

	#region Events


	#endregion

	#region Methods

	public void SetResult(ImportErrorHandlingType result)
	{
		_importer.Continue = result;
	}

	#endregion
}