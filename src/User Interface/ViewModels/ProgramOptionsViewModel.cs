using BibTeXLibrary;
using BibtexManager;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Translation.Validation;
using DigitalProduction.Validation;

namespace BibTexManager.ViewModels;

public partial class ProgramOptionsViewModel : ObservableObject
{
	#region Fields

	[ObservableProperty]
	private bool				_openLastProjectAtStartUp			= false;

	#endregion

	#region Construction

	public ProgramOptionsViewModel()
	{
		Initialize();
	}

	#endregion

	private void Initialize()
	{
		OpenLastProjectAtStartUp = Preferences.LoadLastProjectAtStartUp;
	}

	public void Save()
	{
		Preferences.LoadLastProjectAtStartUp = OpenLastProjectAtStartUp;
	}
}