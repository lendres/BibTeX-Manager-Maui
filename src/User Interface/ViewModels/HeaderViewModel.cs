using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Strings;
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
	public partial string Header { get; set; } = "This is the header";

	#endregion

	#region Methods

	public async Task New()
	{
	}

	public async Task Open()
	{
		if (Project.Bibliography != null)
		{
			Header = Project.Bibliography.Header;
		}
	}

	public async Task Close()
	{
		Header = string.Empty;
	}

	partial void OnHeaderChanged(string value)
	{
		if (!Project.Bibliography.Header.EqualsIgnoringLineEndings(value))
		{
			Project.Bibliography.Header = value;
		}
	}

	#endregion
}