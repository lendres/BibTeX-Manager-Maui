namespace BibTeXManager;

class BibTeXFilePicker : IBibTeXFilePicker
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

	public async Task<string> BrowseForFieldOrderFile()
	{
		PickOptions pickOptions = new() { PickerTitle="Select a Field Order File", FileTypes=CreateFieldOrderFilePickerFileType() };
		return await BrowseForFile(pickOptions);
	}

	public async Task<string> BrowseForFieldQualityFile()
	{
		PickOptions pickOptions = new() { PickerTitle= "Select a Field Quality File", FileTypes=CreateFieldQualityFilePickerFileType() };
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

	public FilePickerFileType CreateBibliographyFilePickerFileType()
	{
		return new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
		{
			{ DevicePlatform.iOS, new[] { "public.plain-text", "public.text" }	},
			{ DevicePlatform.macOS, new[] { "public.plain-text", "public.text" } },
			{ DevicePlatform.Android, new[] { "text/plain" } },
			{ DevicePlatform.WinUI, new[] { ".bib", ".txt", ".text" } },
		});
	}

	public FilePickerFileType CreateFieldOrderFilePickerFileType()
	{
		return new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
		{
			{ DevicePlatform.iOS, new[] { "public.xml", "public.plain-text", "public.text" } },
			{ DevicePlatform.macOS, new[] { "public.xml", "public.plain-text", "public.text" } },
			{ DevicePlatform.Android, new[] { "text/xml", "text/plain" } },
			{ DevicePlatform.WinUI, new[] { ".tagord", ".xml", ".txt", ".text" } },
		});
	}

	public FilePickerFileType CreateFieldQualityFilePickerFileType()
	{
		return new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
		{
			{ DevicePlatform.iOS, new[] { "public.xml", "public.plain-text", "public.text" } },
			{ DevicePlatform.macOS, new[] { "public.xml", "public.plain-text", "public.text" } },
			{ DevicePlatform.Android, new[] { "text/xml", "text/plain" } },
			{ DevicePlatform.WinUI, new[] { ".qlty", ".xml", ".txt", ".text" } },
		});
	}

	public FilePickerFileType CreateNameRemappingFilePickerFileType()
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

	private static async Task<string> BrowseForFile(PickOptions options)
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