using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXManager.ViewModels;

public partial class FieldNameMap
{
	#region Construction

	public FieldNameMap()
	{
	}

	public FieldNameMap(string fromName, string toName)
	{
		FromName = fromName;
		ToName = toName;
	}

	#endregion

	#region Properties

	public string FromName { get; set; } = "";

	public string ToName { get; set; } = "";

	#endregion
}