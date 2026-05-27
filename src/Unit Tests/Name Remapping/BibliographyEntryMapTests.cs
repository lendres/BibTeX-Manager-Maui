using BibTeXManager;

using System.Xml.Serialization;

namespace BibTeXManagerUnitTests;

public class BibEntryMapTests
{
	#region Basic Tests

	[Fact]
	public void DefaultConstructorCreatesEmptyValues()
	{
		BibliographyEntryMap bibEntryMap = new();

		Assert.Equal("", bibEntryMap.Name);
		Assert.Equal("", bibEntryMap.ToType);
		Assert.NotNull(bibEntryMap.FieldNameMaps);
		Assert.Empty(bibEntryMap.FieldNameMaps);
	}

	[Fact]
	public void PropertiesCanBeSet()
	{
		BibliographyEntryMap bibEntryMap = new()
		{
			Name = "article",
			ToType = "inproceedings",
		};

		bibEntryMap.FieldNameMaps.Add(new FieldNameMap("journal", "booktitle"));
		bibEntryMap.FieldNameMaps.Add(new FieldNameMap("volume", "number"));

		Assert.Equal("article", bibEntryMap.Name);
		Assert.Equal("inproceedings", bibEntryMap.ToType);
		Assert.Equal("booktitle", bibEntryMap.FieldNameMaps[0].To);
		Assert.Equal("number", bibEntryMap.FieldNameMaps[1].To);
	}

	#endregion

	#region Serialization Tests

	[Fact]
	public void SerializeIncludesAttributesAndFieldNameMaps()
	{
		BibliographyEntryMap bibEntryMap = CreateTestBibEntryMap();

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
		BibliographyEntryMap bibEntryMap = CreateTestBibEntryMap();
		string xml = SerializeObjectToString(bibEntryMap);

		BibliographyEntryMap deserializedBibEntryMap = DeserializeObjectFromString<BibliographyEntryMap>(xml);

		Assert.Equal("article", deserializedBibEntryMap.Name);
		Assert.Equal("inproceedings", deserializedBibEntryMap.ToType);
		Assert.Equal(2, deserializedBibEntryMap.FieldNameMaps.Count);
		Assert.Equal("booktitle", deserializedBibEntryMap.FieldNameMaps[0].To);
		Assert.Equal("number", deserializedBibEntryMap.FieldNameMaps[1].To);
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

		BibliographyEntryMap bibEntryMap = DeserializeObjectFromString<BibliographyEntryMap>(xml);

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
					<fieldmap from="journal" to="booktitle"/>
					<fieldmap from="volume" to="number"/>
				</fieldmaps>
			</bibliographyentrymap>
			""";

		BibliographyEntryMap bibEntryMap = DeserializeObjectFromString<BibliographyEntryMap>(xml);

		Assert.Equal("article", bibEntryMap.Name);
		Assert.Equal("inproceedings", bibEntryMap.ToType);
		Assert.Equal(2, bibEntryMap.FieldNameMaps.Count);
		Assert.Equal("booktitle", bibEntryMap.FieldNameMaps[0].To);
		Assert.Equal("number", bibEntryMap.FieldNameMaps[1].To);
	}

	#endregion

	#region Helper Functions

	private static BibliographyEntryMap CreateTestBibEntryMap()
	{
		BibliographyEntryMap bibEntryMap = new()
		{
			Name = "article",
			ToType = "inproceedings",
		};

		bibEntryMap.FieldNameMaps.Add(new FieldNameMap("journal", "booktitle"));
		bibEntryMap.FieldNameMaps.Add(new FieldNameMap("volume", "number"));

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