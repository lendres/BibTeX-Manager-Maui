using BibTeXManager.Views;

namespace BibTeXManager;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(EditRawBibEntryForm), typeof(EditRawBibEntryForm));
		Routing.RegisterRoute(nameof(StringConstantsView), typeof(StringConstantsView));
		Routing.RegisterRoute(nameof(SettingsView), typeof(SettingsView));
	}
}