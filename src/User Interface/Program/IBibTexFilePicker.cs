namespace BibTexManager;

public interface IBibTexFilePicker
{
	Task<string> BrowseForProjectFile();
	Task<string> BrowseForBibliographyFile();
	Task<string> BrowseForTagOrderFile();
	Task<string> BrowseForTagQualityFile();
	Task<string> BrowseForNameRemappingFile();

	FilePickerFileType CreateBibliographyProjectFileType();
}