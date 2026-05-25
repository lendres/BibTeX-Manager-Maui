using BibTeXManager;

using System.Xml.Serialization;

namespace BibTeXManagerUnitTests;

public class BibEntryMapTests
{
	#region Basic Tests

	[Fact]
	public void DefaultConstructorCreatesEmptyValues()
	{
		BibEntryMap bibEntryMap = new();

		Assert.Equal("", bibEntryMap.Name);
		Assert.Equal("", bibEntryMap.ToType);
		Assert.NotNull(bibEntryMap.FieldNameMaps);
		Assert.Empty(bibEntryMap.FieldNameMaps);
	}

	[Fact]
	public void PropertiesCanBeSet()
	{
		BibEntryMap bibEntryMap = new()
		{
			Name = "article",
			ToType = "inproceedings",
		};

		bibEntryMap.FieldNameMaps.Add("journal", "booktitle");
		bibEntryMap.FieldNameMaps.Add("volume", "number");

		Assert.Equal("article", bibEntryMap.Name);
		Assert.Equal("inproceedings", bibEntryMap.ToType);
		Assert.Equal("booktitle", bibEntryMap.FieldNameMaps["journal"]);
		Assert.Equal("number", bibEntryMap.FieldNameMaps["volume"]);
	}

	#endregion

	#region Serialization Tests

	[Fact]
	public void SerializeIncludesAttributesAndFieldNameMaps()
	{
		BibEntryMap bibEntryMap = CreateTestBibEntryMap();

		string xml = SerializeObjectToString(bibEntryMap);

		Assert.Contains("name=\"article\"", xml);
		Assert.Contains("totype=\"inproceedings\"", xml);
		Assert.Contains("fieldmaps", xml);
		Assert.Contains("journal", xml);
		Assert.Contains("booktitle", xml);
		Assert.Contains("volume", xml);
		Assert.Contains("number", xml);
	}

	[Fact]
	public void DeserializeReadsAttributesAndFieldNameMaps()
	{
		BibEntryMap bibEntryMap = CreateTestBibEntryMap();
		string xml = SerializeObjectToString(bibEntryMap);

		BibEntryMap deserializedBibEntryMap = DeserializeObjectFromString<BibEntryMap>(xml);

		Assert.Equal("article", deserializedBibEntryMap.Name);
		Assert.Equal("inproceedings", deserializedBibEntryMap.ToType);
		Assert.Equal(2, deserializedBibEntryMap.FieldNameMaps.Count);
		Assert.Equal("booktitle", deserializedBibEntryMap.FieldNameMaps["journal"]);
		Assert.Equal("number", deserializedBibEntryMap.FieldNameMaps["volume"]);
	}

	#endregion

	#region Deserialization Tests

	[Fact]
	public void DeserializeEmptyXmlCreatesDefaultFieldNameMaps()
	{
		string xml =
			"""
			<?xml version="1.0" encoding="utf-16"?>
			<bibliographyentrymap name="article" totype="inproceedings" />
			""";

		BibEntryMap bibEntryMap = DeserializeObjectFromString<BibEntryMap>(xml);

		Assert.Equal("article", bibEntryMap.Name);
		Assert.Equal("inproceedings", bibEntryMap.ToType);
		Assert.NotNull(bibEntryMap.FieldNameMaps);
		Assert.Empty(bibEntryMap.FieldNameMaps);
	}

	[Fact]
	public void DeserializeFromRawXmlString()
	{
		string xml =
			"""
			<?xml version="1.0" encoding="utf-16"?>
			<bibliographyentrymap name="article" totype="inproceedings">
				<fieldmaps>
					<item key="journal">
						<value>booktitle</value>
					</item>
					<item key="volume">
						<value>number</value>
					</item>
				</fieldmaps>
			</bibliographyentrymap>
			""";

		BibEntryMap bibEntryMap = DeserializeObjectFromString<BibEntryMap>(xml);

		Assert.Equal("article", bibEntryMap.Name);
		Assert.Equal("inproceedings", bibEntryMap.ToType);
		Assert.Equal(2, bibEntryMap.FieldNameMaps.Count);
		Assert.Equal("booktitle", bibEntryMap.FieldNameMaps["journal"]);
		Assert.Equal("number", bibEntryMap.FieldNameMaps["volume"]);
	}

	#endregion

	#region Helper Functions

	private static BibEntryMap CreateTestBibEntryMap()
	{
		BibEntryMap bibEntryMap = new()
		{
			Name = "article",
			ToType = "inproceedings",
		};

		bibEntryMap.FieldNameMaps.Add("journal", "booktitle");
		bibEntryMap.FieldNameMaps.Add("volume", "number");

		return bibEntryMap;
	}

	private static string SerializeObjectToString<T>(T value)
	{
		XmlSerializer serializer = new(typeof(T));

		using StringWriter stringWriter = new();
		serializer.Serialize(stringWriter, value);

		return stringWriter.ToString();
	}

	private static T DeserializeObjectFromString<T>(string xml)
	{
		XmlSerializer serializer = new(typeof(T));
		using StringReader stringReader = new(xml);
		return (T)serializer.Deserialize(stringReader)!;
	}

	#endregion
}