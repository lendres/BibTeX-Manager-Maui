using CommunityToolkit.Mvvm.ComponentModel;

namespace BibTeXManager.ViewModels;

public partial class AddFieldProcessorViewModel : ObservableObject
{
	#region Construction

	public AddFieldProcessorViewModel()
	{
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string SelectedType { get; set; } = string.Empty;

	public List<string> ProcessorTypes
	{
		get
		{
			List<Type> processorTypes = DigitalProduction.Reflection.Assembly.GetConcreteSubclassTypesOf(typeof(FieldProcessor));
			return processorTypes.Select(type => type.Name).ToList();
		}
	}

	#endregion
}