namespace BibTeXManager.ViewModels;

public interface IBibliographyPartViewModel
{
	BibTeXProject Project { get; }

	Task New();

	Task Open();

	Task Close();
}
