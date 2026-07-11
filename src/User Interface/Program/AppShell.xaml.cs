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
		Routing.RegisterRoute(nameof(GroupManagerView), typeof(GroupManagerView));
		Routing.RegisterRoute(nameof(FieldProcessorGroupView), typeof(FieldProcessorGroupView));
		Routing.RegisterRoute(nameof(QuoteFieldProcessorView), typeof(QuoteFieldProcessorView));
		Routing.RegisterRoute(nameof(StringReplacementFieldProcessorView), typeof(StringReplacementFieldProcessorView));
		Routing.RegisterRoute(nameof(RemoveEnclosingBracesFieldProcessorView), typeof(RemoveEnclosingBracesFieldProcessorView));
		Routing.RegisterRoute(nameof(SentenceEndingSpacesFieldProcessorView), typeof(SentenceEndingSpacesFieldProcessorView));
		Routing.RegisterRoute(nameof(StringCaseFieldProcessorView), typeof(StringCaseFieldProcessorView));
		Routing.RegisterRoute(nameof(StringReplacementFieldProcessorView), typeof(StringReplacementFieldProcessorView));
	}
}