using BibtexManager;
using BibtexManager.Project;
using DigitalProduction.Http;

namespace BibtexManagerUnitTests;

public class BulkSpeImportTests
{
	public BulkSpeImportTests()
	{
		CustomSearchKey? customSearchKey = CustomSearchKey.Deserialize(@"..\..\..\customsearchkey.xml");
		Assert.NotNull(customSearchKey);
		CustomSearch.SetCxAndKey(customSearchKey);
	}

	/// <summary>
	/// 
	/// </summary>
	[Fact]
	public void ConferenceImportTest()
	{
		string search1					= "https://onepetro.org/SPEDC/24DC/conference/2-24DC";
		SpeConferenceImporter importer	= new([search1]);
		string resultString				= "Search: " + search1 + Environment.NewLine + Environment.NewLine;

		foreach (ImportResult result in importer.BulkImport())
		{
			resultString += result.BibEntry?.ToString();
		}

		//Assert.Equal(Statistics.Covariance(xValues, yValues), 2.9167, 0.0001, errorMessage);
	}

} // End class.