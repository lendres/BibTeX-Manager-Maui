namespace BibTeXManager.ViewModels;

public partial class FieldNameMap
{
	#region Construction

	public FieldNameMap()
	{
	}

	public FieldNameMap(string fromName, string toName)
	{
		From	= fromName;
		To		= toName;
	}

	#endregion

	#region Properties

	public string From { get; set; } = "";

	public string To { get; set; } = "";

	#endregion
}