using BibTeXManager.Views;

namespace BibTeXManager;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(EditRawBibEntryForm), typeof(EditRawBibEntryForm));
		Routing.RegisterRoute(nameof(SettingsView), typeof(SettingsView));
		Routing.RegisterRoute(nameof(NameMappingView), typeof(NameMappingView));
		Routing.RegisterRoute(nameof(TemplatesEditView), typeof(TemplatesEditView));
	}
}