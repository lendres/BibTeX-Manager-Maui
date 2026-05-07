namespace BibTeXManager;

public interface IBibTeXFilePicker
{
	Task<string> BrowseForProjectFile();
	Task<string> BrowseForBibliographyFile();
	Task<string> BrowseForFieldOrderFile();
	Task<string> BrowseForFieldQualityFile();
	Task<string> BrowseForNameRemappingFile();

	FilePickerFileType CreateBibliographyProjectFileType();
	FilePickerFileType CreateBibliographyFilePickerFileType();
	FilePickerFileType CreateFieldOrderFilePickerFileType();
	FilePickerFileType CreateFieldQualityFilePickerFileType();
	FilePickerFileType CreateNameRemappingFilePickerFileType();
}