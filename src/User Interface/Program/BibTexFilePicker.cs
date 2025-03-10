using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTexManager;

class BibTexFilePicker : IBibTexFilePicker
{

	public async Task<string> BrowseForBibliographyFile()
	{
		PickOptions pickOptions = new() { PickerTitle="Select a Bibliography File", FileTypes=CreateBibliographyFilePickerFileType() };
		return await BrowseForFile(pickOptions);
	}

	public async Task<string> BrowseForProjectFile()
	{
		PickOptions pickOptions = new() { PickerTitle="Select a Bibliography File", FileTypes=CreateBibliographyProjectFileType() };
		return await BrowseForFile(pickOptions);
	}

	private async Task<string> BrowseForFile(PickOptions options)
	{
		try
		{
			FileResult? result = await FilePicker.PickAsync(options);
			if (result != null)
			{
				return result.FullPath;
			}
			return string.Empty;
		}
		catch
		{
			// The user canceled or something went wrong.
			return string.Empty;
		}
	}

	private static FilePickerFileType CreateBibliographyFilePickerFileType()
	{
		return new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
		{
			{ DevicePlatform.iOS, new[] { "public.plain-text", "public.text" }	},
			{ DevicePlatform.macOS, new[] { "public.plain-text", "public.text" } },
			{ DevicePlatform.Android, new[] { "text/plain" } },
			{ DevicePlatform.WinUI, new[] { ".bib", ".txt", ".text" } },
		});
	}

	public FilePickerFileType CreateBibliographyProjectFileType()
	{
		return new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
		{
			{ DevicePlatform.WinUI, new[] { ".bibproj" } }
		});
	}

}