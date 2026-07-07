using BibTeXManager.ViewModels;
using BibTeXManager.Views;
using BibTeXManager.Views.Field_Quality;

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
		Routing.RegisterRoute(nameof(GroupManagerView), typeof(GroupManagerView));
		Routing.RegisterRoute(nameof(FieldProcessorGroupEditorView), typeof(FieldProcessorGroupEditorView));
		Routing.RegisterRoute(nameof(StringReplacementFieldProcessorView), typeof(StringReplacementFieldProcessorView));
	}
}