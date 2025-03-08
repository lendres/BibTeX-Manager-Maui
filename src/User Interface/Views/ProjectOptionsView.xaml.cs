using BibTexManager.ViewModels;
using DigitalProduction.Maui.Views;
using DocumentFormat.OpenXml.Spreadsheet;
using System;

namespace BibTexManager.Views;

public partial class ProjectOptionsView : PopupView
{
	ProjectOptionsViewModel _viewModel;

	public ProjectOptionsView(ProjectOptionsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	async void OnBrowseForInputFile(object sender, EventArgs eventArgs)
	{
		try
		{
			PickOptions pickOptions = new() { PickerTitle="Select a Bibliography File", FileTypes=CreateBibliographyFilePickerFileType() };
			FileResult? result      = await BrowseForFile(pickOptions);

			if (result != null)
			{
				BibliographyFileEntry.Text = result.FullPath;
			}
		}
		catch (Exception exception)
		{
			BibliographyFileEntry.Text = string.Empty;
		}
	}

	public static FilePickerFileType CreateBibliographyFilePickerFileType()
	{
		return new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
		{
			{ DevicePlatform.iOS, new[] { "public.plain-text", "public.text" }	},
			{ DevicePlatform.macOS, new[] { "public.plain-text", "public.text" } },
			{ DevicePlatform.Android, new[] { "text/plain" } },
			{ DevicePlatform.WinUI, new[] { ".bib", ".txt", ".text" } },
		});
	}

	public static async Task<FileResult?> BrowseForFile(PickOptions options)
	{
		try
		{
			return await FilePicker.PickAsync(options);
		}
		catch
		{
			// The user canceled or something went wrong.
		}

		return null;
	}

	protected override void OnSaveButtonClicked(object? sender, EventArgs eventArgs)
	{
		_viewModel.Save();
		base.OnSaveButtonClicked(sender, eventArgs);
	}
}