using CommunityToolkit.Mvvm.ComponentModel;

namespace BibTexManager.ViewModels;

public partial class ProgramOptionsViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public ProgramOptionsViewModel()
	{
		Initialize();
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial bool				OpenLastProjectAtStartUp { get; set; }			= false;

	[ObservableProperty]
	public partial bool				IsSubmittable { get; set; }						= true;

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