using BibTeXLibrary;
using BibtexManager.Project;
using HtmlAgilityPack;

namespace BibtexManager;

/// <summary>
/// 
/// </summary>
public class SpeConferenceImporter : BulkImporter, IBulkImporter
{
	#region Fields

	private readonly string[]							_conferencePageUrls			= [];

	private int											_currentReferenceItem		= 0;

	private readonly List<ConferenceReferenceItem>		_conferenceItems			= new List<ConferenceReferenceItem>();
	private readonly List<string>						_articleLinks				= new List<string>();

	private readonly string								_outputPath					= "";


	#endregion

	#region Construction

	/// <summary>
	/// Constructor.
	/// </summary>
	public SpeConferenceImporter(string importPath)
	{
		_conferencePageUrls = File.ReadAllLines(importPath);
		_outputPath			= DigitalProduction.IO.Path.GetFullPathWithoutExtension(importPath) + "-output.xlsx";
	}

	/// <summary>
	/// Constructor.
	/// </summary>
	public SpeConferenceImporter(string[] conferencePageUrls, string? outputPath = null)
	{
		_conferencePageUrls = conferencePageUrls;
		if (!string.IsNullOrEmpty(outputPath))
		{
			_outputPath = outputPath;
		}
	}

	#endregion

	#region Properties

	#endregion

	#region Interface Methods

	/// <summary>
	/// Search for and download the Bibtex entry for an SPE paper.
	/// </summary>
	/// <param name="searchTerms">Terms to search the web for the paper.</param>
	protected override BibEntry? Import(string url)
	{
		string bibTexString = SpeImportUtilities.DownloadSpeBibtex(this.HttpClient, url).Result;

		if (!String.IsNullOrEmpty(bibTexString))
		{
			return ParseSingleEntryText(bibTexString);
		}

		return null;
	}

	/// <summary>
	/// Bulk SPE paper search and import.
	/// </summary>
	/// <param name="path">The path to a file that contains a list of search strings.</param>
	public override IEnumerable<ImportResult> BulkImport()
	{
		_currentReferenceItem = 0;

		GenerateConferenceLinks(_conferencePageUrls);

		foreach (ImportResult importResult in BulkImport(_articleLinks.ToArray()))
		{
			yield return importResult;
		}

		// Write the results.
		if (!string.IsNullOrEmpty(_outputPath))
		{
			WriteBulkImportResults(_outputPath, ConferenceReferenceItem.Headers);
		}
	}

	private void GenerateConferenceLinks(string[] conferencePageUrls)
	{
		// Loop over each HTML page provided.
		int pageNumber = 0;
		foreach (string conferencePageUrl in conferencePageUrls)
		{
			pageNumber++;
			List<HtmlNode> sessionSections = GetSessionSections(conferencePageUrl);

			// Loop over each session.
			foreach (HtmlNode sessionSection in sessionSections)
			{
				// Extract the session name from a header element.
				string sessionName = sessionSection.Descendants("h4").FirstOrDefault()?.InnerText.Trim() ?? "";

				// Find each paper by finding the links to articles.
				foreach (string articleLink in GetArticleLinks(sessionSection))
				{
					string articleAddress = SpeImportUtilities.Website + articleLink;

					_conferenceItems.Add(
						new ConferenceReferenceItem()
						{
							Number          = pageNumber,
							ConferencePage  = conferencePageUrl,
							Session         = sessionName,
							Url             = articleAddress
						}
					);

					_articleLinks.Add(articleAddress);
				}
			}
		}
	}

	protected override void FormatAndSaveResult(string searchString, ImportResult importResult)
	{
		ConferenceReferenceItem referenceItem = _conferenceItems[_currentReferenceItem++];

		if (importResult.BibEntry != null)
		{
			referenceItem.Reference	= importResult.BibEntry.Key;
			referenceItem.Title		= importResult.BibEntry.Title.TrimStart('{').TrimEnd('}');
		}

		SaveResult(referenceItem.ToStringArray());
	}

	private List<HtmlNode> GetSessionSections(string url)
	{
		HtmlNode articleList = GetArticleListSection(url);
		IEnumerable<HtmlNode> sessionSections = articleList.Descendants("section");
		return sessionSections.ToList();
	}

	private HtmlNode GetArticleListSection(string url)
	{
		HtmlDocument htmlDocument				= new HtmlWeb().Load(url);
		IEnumerable<HtmlNode> articleSection	= htmlDocument.DocumentNode.Descendants("div")
			.Select(div => div)
			.Where(u => u.GetAttributeValue("id", null) == "ArticleList");

		return articleSection.ToList<HtmlNode>()[0];
	}

	private List<string> GetArticleLinks(HtmlNode sessionSection)
	{
		IEnumerable<HtmlNode> articleLinks = sessionSection.Descendants("a")
			.Select(a => a)
			.Where(u => u.GetAttributeValue("class", null) == "viewArticleLink");

		List<string> links = new List<string>();

		foreach (HtmlNode link in articleLinks)
		{
			links.Add(link.GetAttributeValue("href", null));
		}

		return links;
	}

	//<a href="/SPEDC/proceedings/24DC/1-24DC/D011S002R002/542896" class="viewArticleLink" data-feathr-click-track="true" data-feathr-link-aids="5d7a72b3d3708ff60b15b26c">View Article<span class="screenreader-text">titled, Blind Trust: Challenges in Data Sharing for Oil and Gas Well Construction</span></a>

	#endregion

} // End class.