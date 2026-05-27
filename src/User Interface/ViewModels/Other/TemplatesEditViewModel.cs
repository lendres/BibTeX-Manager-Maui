using BibTeXLibrary;
using BibTeXManager.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Enums;
using DigitalProduction.Maui.ViewModels;
using System.Collections.ObjectModel;

namespace BibTeXManager.ViewModels;

public partial class TemplatesEditViewModel : DataGridBaseViewModel<NameMap>
{
	#region Fields
	#endregion

	#region Construction

	public TemplatesEditViewModel()
	{
		Initializer = BibTeXProject.Instance!.BibEntryInitialization;
		Initialize();
		AddValidations();
		SetModified(false);
	 }

	#endregion

	#region Properties

	public BibEntryInitialization				Initializer { get; set; }

	[ObservableProperty]
	public partial InitializationPartType		ActiveInitializationPart { get; set; }

	[ObservableProperty]
	public partial List<string>?				TemplateNames { get; set; }

	[ObservableProperty]
	public partial string?						SelectedTemplate { get; set; }

	[ObservableProperty]
	public partial List<string>?				TemplateFieldNames { get; set; }

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }

	#endregion

	#region Initialization

	private void Initialize()
	{
		Items = new ObservableCollection<NameMap>(Initializer.NameMaps);
		TemplateNames = Initializer.TemplateNames;
	}

	#endregion

	#region Validation

	private void AddValidations()
	{
		//AuxiliaryFile.Validations.Add(new ConditionalIsNotNullOrEmptyRule { IsRequired = () => Settings.UseAuxiliaryFile, ValidationMessage = "A file name is required." });
		//AuxiliaryFile.Validations.Add(new ConditionalFileExistsRule { IsRequired = () => Settings.UseAuxiliaryFile, ValidationMessage = "The file does not exist." });
		//ValidateAuxiliaryFile();
	}

	[RelayCommand]
	private void ValidateAuxiliaryFile()
	{
		//if (AuxiliaryFile.Validate())
		//{
		//	Settings.AuxiliaryFile = AuxiliaryFile.Value!;
		//}
		ValidateSubmittable();
	}
	public bool ValidateSubmittable() => IsSubmittable =
		Modified;


	#endregion

	#region Events


	#endregion

	#region Commands

	[RelayCommand]
	private void SelectedTemplateChanged()
	{
		if (SelectedTemplate == null)
		{
			SelectedTemplate = null;
			TemplateFieldNames = null;
			return;
		}

		//SelectedBibliographyEntryMap = NameMapper.Maps.TryGetValue(SelectedTemplate, out BibliographyEntryMap? map) ? map : null;
		//if (SelectedBibliographyEntryMap != null)
		//{
		//	Items = NameMapper.Maps[SelectedTemplate!].FieldNameMaps;
		//	ToType.Value = SelectedBibliographyEntryMap.ToType;
		//}
		//else
		//{
		//	// DataGrid selected item.
		//	SelectedItem = null;
		//}

	}

	#endregion

	#region Methods

	private void SetModified(bool modified)
	{
		Modified = modified;
		ValidateSubmittable();
	}

	public void Save()
	{
	}

	public override SearchResult Find(string search)
	{
		throw new NotImplementedException();
	}

	#endregion
}