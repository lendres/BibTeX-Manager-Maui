namespace BibTeXManager.Views;

using BibTeXManager.ViewModels;
using DigitalProduction.Maui.Views;

public partial class TemplateSelectionView : PopupView
{
	public TemplateSelectionView(TemplateSelectionViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		TemplatePicker.SelectedIndex = 0;
	}
}