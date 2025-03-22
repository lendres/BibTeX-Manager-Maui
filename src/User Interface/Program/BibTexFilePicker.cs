namespace BibTexManager;

class BibTexFilePicker : IBibTexFilePicker
{
	#region File Browsing

	public async Task<string> BrowseForProjectFile()
	{
		PickOptions pickOptions = new() { PickerTitle="Select a Bibliography File", FileTypes=CreateBibliographyProjectFileType() };
		return await BrowseForFile(pickOptions);
	}

	public async Task<string> BrowseForBibliographyFile()
	{
		PickOptions pickOptions = new() { PickerTitle="Select a Bibliography File", FileTypes=CreateBibliographyFilePickerFileType() };
		return await BrowseForFile(pickOptions);
	}

	public async Task<string> BrowseForTagOrderFile()
	{
		PickOptions pickOptions = new() { PickerTitle="Select a Tag Order File", FileTypes=CreateTagOrderFilePickerFileType() };
		return await BrowseForFile(pickOptions);
	}

	public async Task<string> BrowseForTagQualityFile()
	{
		PickOptions pickOptions = new() { PickerTitle="Select a Tag Quality File", FileTypes=CreateTagQualityFilePickerFileType() };
		return await BrowseForFile(pickOptions);
	}

	public async Task<string> BrowseForNameRemappingFile()
	{
		PickOptions pickOptions = new() { PickerTitle="Select a Name Remapping File", FileTypes=CreateNameRemappingFilePickerFileType() };
		return await BrowseForFile(pickOptions);
	}

	#endregion

	#region Creating File Types

	public FilePickerFileType CreateBibliographyProjectFileType()
	{
		return new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
		{
			{ DevicePlatform.WinUI, new[] { ".bibproj" } }
		});
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

	private static FilePickerFileType CreateTagOrderFilePickerFileType()
	{
		return new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
		{
			{ DevicePlatform.iOS, new[] { "public.xml", "public.plain-text", "public.text" } },
			{ DevicePlatform.macOS, new[] { "public.xml", "public.plain-text", "public.text" } },
			{ DevicePlatform.Android, new[] { "text/xml", "text/plain" } },
			{ DevicePlatform.WinUI, new[] { ".tagord", ".xml", ".txt", ".text" } },
		});
	}

	private static FilePickerFileType CreateTagQualityFilePickerFileType()
	{
		return new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
		{
			{ DevicePlatform.iOS, new[] { "public.xml", "public.plain-text", "public.text" } },
			{ DevicePlatform.macOS, new[] { "public.xml", "public.plain-text", "public.text" } },
			{ DevicePlatform.Android, new[] { "text/xml", "text/plain" } },
			{ DevicePlatform.WinUI, new[] { ".qlty", ".xml", ".txt", ".text" } },
		});
	}

	private static FilePickerFileType CreateNameRemappingFilePickerFileType()
	{
		return new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
		{
			{ DevicePlatform.iOS, new[] { "public.xml", "public.plain-text", "public.text" } },
			{ DevicePlatform.macOS, new[] { "public.xml", "public.plain-text", "public.text" } },
			{ DevicePlatform.Android, new[] { "text/xml", "text/plain" } },
			{ DevicePlatform.WinUI, new[] { ".bibmap", ".xml", ".txt", ".text" } },
		});
	}

	#endregion

	#region Helper Methods

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

	#endregion
}