using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace BibtexManager;

public class XInclude
{
	[XmlAttribute("href")]
	public string Href { get; set; } = string.Empty;
}


public class GroupManager
{

	#region Fields

	#endregion

	#region Properties

	[XmlArray("fieldprocessorgroups"), XmlArrayItem("include", Namespace = "http://www.w3.org/2001/XInclude")]
	public ObservableCollection<XInclude> Includes { get; set; } = new();

	#endregion

}