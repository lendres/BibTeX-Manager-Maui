namespace BibTexManager;

public interface IBibTexFilePicker
{
	Task<string> BrowseForBibliographyFile();
	Task<string> BrowseForProjectFile();
}
