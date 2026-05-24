using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXManager.ViewModels;

public partial class FieldNameMapViewModel : ObservableObject
{
	#region Construction

	public FieldNameMapViewModel()
	{
	}

	public FieldNameMapViewModel(string fromName, string toName)
	{
		FromName = fromName;
		ToName = toName;
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string FromName { get; set; } = "";

	[ObservableProperty]
	public partial string ToName { get; set; } = "";

	#endregion
}
