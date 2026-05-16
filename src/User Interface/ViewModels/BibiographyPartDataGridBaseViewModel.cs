using DigitalProduction.Maui.ViewModels;

namespace BibTeXManager.ViewModels;

public abstract class BibiographyPartDataGridBaseViewModel<T> : DataGridBaseViewModel<T> where T : class
{
	#region Properties

	public BibTeXProject Project { get => BibTeXProject.Instance ?? throw new NullReferenceException("Project is null."); }

	#endregion

	#region Methods

	public async Task New()
	{
		if (Project.Bibliography != null)
		{
			AddItems();
		}
	}

	public async Task Open()
	{
		Items?.Clear();
		AddItems();
	}

	public async Task Close()
	{
		Items?.Clear();
		Items = null;
	}

	protected abstract void AddItems();

	#endregion
}