using BibTexManager.Views;

namespace BibTexManager;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(EditRawBibEntryForm), typeof(EditRawBibEntryForm));
	}
}