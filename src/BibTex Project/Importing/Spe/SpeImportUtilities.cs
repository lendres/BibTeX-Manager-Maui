using DocumentFormat.OpenXml.Office2016.Excel;
using Google.Apis.CustomSearchAPI.v1.Data;
using System.Net.Http;

namespace BibTeXManager;

/// <summary>
/// 
/// </summary>
public static class SpeImportUtilities
{
	#region Fields

	private static string		_website          = "onepetro.org";

	#endregion

	#region Construction

	#endregion

	#region Properties

	public static string Website { get => _website; set => _website=value; }

	#endregion

	#region Methods

	/// <summary>
	/// Search for and download the Bibtex entry for an SPE paper.
	/// </summary>
	/// <param name="searchTerms">Terms to search the web for the paper.</param>
	public static IEnumerable<string?> ArticleSearch(HttpClient client, string searchTerms)
	{
		for (int i = 0; i < 2; i++)
		{
			//IList<Result> results   = DigitalProduction.Http.CustomSearch.SiteSearch(searchTerms, website);
			IList<Result> results   = DigitalProduction.Http.CustomSearch.SiteSearch(searchTerms, _website, i*11);

			// No results were returned.
			if (results == null)
			{
				yield break;
			}

			foreach (Result result in results)
			{
				string		webSiteUrl		= result.Link;
				WebPageType	webPageType		= SpeWebPageType(webSiteUrl);
				string?		bibTexString	= null;

				switch (webPageType)
				{
					case WebPageType.ArticlePage:
						bibTexString = DownloadSpeBibtex(client, webSiteUrl).Result;
						break;

					case WebPageType.ConferencePage:
						bibTexString = SearchConferencePageForArticle(client, webSiteUrl, searchTerms);
						break;

					case WebPageType.Unknown:
						// No nothing and keep searching.
						break;
				}

				// If we found the correct entry, return it, otherwise we keep searching.
				if (bibTexString != null)
				{
					yield return bibTexString;
				}

			}
		}
		yield return null;
	}

	private static WebPageType SpeWebPageType(string result)
	{
		// Look for resutls that contain the specified website.
		if (!result.Contains(_website))
		{
			return WebPageType.Unknown;
		}

		string lastPathElement = result.Split('/').Last();

		if (result.Contains("conference"))
		{
			return WebPageType.ConferencePage;
		}

		// Did we find a OnePetro reference to a document or something else?  The documents end in a number.
		if (int.TryParse(lastPathElement, out _))
		{
			return WebPageType.ArticlePage;
		}

		return WebPageType.Unknown;
	}

	async public static Task<string> DownloadSpeBibtex(HttpClient client, string articleUrl)
	{
		// Extract the last path element.  For an SPE article, this should be the document ID.
		string docuementId = articleUrl.Split('/').Last();
		string downloadUrl = "https://onepetro.org/Citation/Download?resourceId=" + docuementId + "&resourceType=3&citationFormat=2";

		// Attempt to download the bitex entry.
		HttpResponseMessage	response		= await client.GetAsync(downloadUrl);
		if (!response.IsSuccessStatusCode)
		{
			string message = "Failed to download Bibtex entry from " + _website + ": " + response.ReasonPhrase;
			//message +=  Environment.NewLine + "Check that you are logged into SPE.";
			throw new HttpRequestException();
		}

		HttpContent			content			= response.Content;
		string				responseString	= await content.ReadAsStringAsync();
		responseString						= DigitalProduction.Strings.Format.TrimStart(responseString, "\r\n");
		
		return responseString;
	}

	private static string? SearchConferencePageForArticle(HttpClient client, string url, string searchTerms)
	{
		List<string[]> links = DigitalProduction.Http.HttpGet.GetAllLinksOnPage(url);

		foreach (string[] link in links)
		{
			if (DigitalProduction.Strings.Format.Similarity(link[0], searchTerms) > 0.9)
			{
				return DownloadSpeBibtex(client, link[1]).Result;
			}
		}

		return null;
	}

	#endregion

} // End class.