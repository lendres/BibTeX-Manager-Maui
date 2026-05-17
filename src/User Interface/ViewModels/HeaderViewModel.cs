using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXManager.ViewModels;

public partial class HeaderViewModel : ObservableObject, IBibliographyPartViewModel
{
	#region Properties

	public BibTeXProject Project { get => BibTeXProject.Instance ?? throw new NullReferenceException("Project is null."); }

	[ObservableProperty]
	public partial string Header { get; set; } = "";

	#endregion

	#region Methods

	public async Task New()
	{
	}

	public async Task Open()
	{
		if (Project.Bibliography != null)
		{
			Header = "";
			foreach (string line in Project.Bibliography.Header)
				Header += line + Environment.NewLine;
		}
	}

	public async Task Close()
	{
	}

	#endregion
}
