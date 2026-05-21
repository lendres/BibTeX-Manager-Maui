namespace BibTeXManager.Views;

public interface IBibliographyPartDataGridView
{
	void OnNewEntry(object sender, EventArgs eventArgs);

	void OnEditEntry(object sender, EventArgs eventArgs);

	void OnDeleteEntry(object sender, EventArgs eventArgs);

	void SelectNextFoundItem();

	void OnScrollToSelection(object sender, EventArgs eventArgs);
}