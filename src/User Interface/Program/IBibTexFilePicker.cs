namespace BibTexManager;

public interface IBibTexFilePicker
{
	Task<string> BrowseForProjectFile();
	Task<string> BrowseForBibliographyFile();
	Task<string> BrowseForTagOrderFile();

	FilePickerFileType CreateBibliographyProjectFileType();
}
