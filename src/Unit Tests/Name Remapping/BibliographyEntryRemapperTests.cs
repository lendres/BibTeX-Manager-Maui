using BibTeXLibrary;
using BibTeXManager;
using System.Xml.Serialization;

namespace BibTeXManagerUnitTests;

public class BibliographyEntryRemapperTests
{
	#region Basic Tests

	[Fact]
	public void DefaultConstructorCreatesEmptyMaps()
	{
		BibEntryRemapper remapper = new();

		Assert.NotNull(remapper.Maps);
		Assert.Empty(remapper.Maps);
	}

	[Fact]
	public void RemapEntryNamesRemapsTypeAndMappedFields()
	{
		BibEntryRemapper remapper = CreateTestRemapper();
		BibEntry entry = CreateArticleEntry();

		remapper.RemapEntryNames(entry);

		Assert.Equal("inproceedings", entry.Type);
		Assert.False(entry.ContainsFieldName("journal"));
		Assert.False(entry.ContainsFieldName("volume"));
		Assert.True(entry.ContainsFieldName("booktitle"));
		Assert.True(entry.ContainsFieldName("number"));
		Assert.Equal("Journal Name", entry["booktitle"]);
		Assert.Equal("12", entry["number"]);
		Assert.Equal("Paper Title", entry["title"]);
	}

	[Fact]
	public void RemapEntryNamesDoesNothingWhenMapDoesNotExist()
	{
		BibEntryRemapper remapper = CreateTestRemapper();
		BibEntry entry = CreateArticleEntry();

		entry.Type = "book";
		remapper.RemapEntryNames(entry);

		Assert.Equal("book", entry.Type);
		Assert.True(entry.ContainsFieldName("journal"));
		Assert.True(entry.ContainsFieldName("volume"));
		Assert.False(entry.ContainsFieldName("booktitle"));
		Assert.False(entry.ContainsFieldName("number"));
	}

	[Fact]
	public void RemapEntryNamesOnlyRenamesExistingFields()
	{
		BibEntryRemapper remapper = CreateTestRemapper();
		BibEntry entry = new() { Type = "article" };

		entry["journal"] = "Journal Name";

		remapper.RemapEntryNames(entry);

		Assert.Equal("inproceedings", entry.Type);
		Assert.False(entry.ContainsFieldName("journal"));
		Assert.True(entry.ContainsFieldName("booktitle"));
		Assert.False(entry.ContainsFieldName("number"));
		Assert.Equal("Journal Name", entry["booktitle"]);
	}

	[Fact]
	public void RemapEntryNamesMatchesEntryTypeIgnoringCase()
	{
		BibEntryRemapper remapper = CreateTestRemapper();
		BibEntry entry = CreateArticleEntry();

		entry.Type = "Article";
		remapper.RemapEntryNames(entry);

		Assert.Equal("inproceedings", entry.Type);
		Assert.True(entry.ContainsFieldName("booktitle"));
		Assert.True(entry.ContainsFieldName("number"));
	}

	#endregion

	#region Serialization and Deserialization Tests

	[Fact]
	public void SerializeAndDeserializePreservesMaps()
	{
		string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.xml");
		BibEntryRemapper remapper = CreateTestRemapper();

		try
		{
			remapper.Serialize(filePath);

			BibEntryRemapper deserializedRemapper = BibEntryRemapper.Deserialize(filePath)!;

			Assert.NotNull(deserializedRemapper);
			Assert.Single(deserializedRemapper.Maps);
			Assert.True(deserializedRemapper.Maps.ContainsKey("article"));
			Assert.Equal("article", deserializedRemapper.Maps["article"].Name);
			Assert.Equal("inproceedings", deserializedRemapper.Maps["article"].ToType);
			Assert.Equal("booktitle", deserializedRemapper.Maps["article"].FieldNameMaps["journal"]);
			Assert.Equal("number", deserializedRemapper.Maps["article"].FieldNameMaps["volume"]);
		}
		finally
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}
	}

	[Fact]
	public void DeserializeFromRawXmlString()
	{
		string xml =
			"""
			<?xml version="1.0" encoding="utf-16"?>
			<bibentryremapping>
				<maps>
					<item key="article">
						<value name="Article to InProceedings" totype="inproceedings">
							<fieldmaps>
								<item key="journal">
									<value>booktitle</value>
								</item>
								<item key="volume">
									<value>number</value>
								</item>
							</fieldmaps>
						</value>
					</item>
				</maps>
			</bibentryremapping>
			""";

		//<fieldnamemap from="journal" to="booktitle" />
		//<fieldnamemap from="volume" to="number" />

		BibEntryRemapper remapper = DeserializeObjectFromString<BibEntryRemapper>(xml);

		Assert.Single(remapper.Maps);
		Assert.True(remapper.Maps.ContainsKey("article"));
		Assert.Equal("Article to InProceedings", remapper.Maps["article"].Name);
		Assert.Equal("inproceedings", remapper.Maps["article"].ToType);
		Assert.Equal("booktitle", remapper.Maps["article"].FieldNameMaps["journal"]);
		Assert.Equal("number", remapper.Maps["article"].FieldNameMaps["volume"]);
	}

	#endregion

	#region Helper Functions

	private static BibEntryRemapper CreateTestRemapper()
	{
		BibEntryMap map = new()
		{
			Name = "article",
			ToType = "inproceedings",
		};

		map.FieldNameMaps.Add("journal", "booktitle");
		map.FieldNameMaps.Add("volume", "number");

		BibEntryRemapper remapper = new();
		remapper.Maps.Add("article", map);

		return remapper;
	}

	private static BibEntry CreateArticleEntry()
	{
		BibEntry entry = new() { Type = "article" };

		entry["journal"] = "Journal Name";
		entry["volume"] = "12";
		entry["title"] = "Paper Title";

		return entry;
	}

	private static T DeserializeObjectFromString<T>(string xml)
	{
		XmlSerializer serializer = new(typeof(T));
		using StringReader stringReader = new(xml);
		return (T)serializer.Deserialize(stringReader)!;
	}

	#endregion
}