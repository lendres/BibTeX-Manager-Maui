using DigitalProduction.Maui.Enums;
namespace BibTeXManager.Views;

public interface IBibliographyPartDataGridView
{
	void OnNewEntry(object sender, EventArgs eventArgs);

	void OnEditEntry(object sender, EventArgs eventArgs);

	void OnDeleteEntry(object sender, EventArgs eventArgs);

	SearchResult Find(string searchString);

	SearchResult SelectNextFoundItem();

	void OnScrollToSelection(object sender, EventArgs eventArgs);
}