using BibtexManager;
using DigitalProduction.Xml.Serialization;
using System.Xml.Serialization;

namespace BibTeXManagerUnitTests;

[XmlRoot("qualityprocessor")]
public class GroupManagerTests
{
	[Fact]
	public void IncludesDefaultIsEmpty()
	{
		GroupManager groupManager = new();

		Assert.Empty(groupManager.Includes);
	}

	[Fact]
	public void XIncludeHrefDefaultIsEmpty()
	{
		XInclude include = new();

		Assert.Equal(string.Empty, include.Href);
	}

	[Fact]
	public void GetAvailableQualityFilesWithEmptyPathReturnsEmpty()
	{
		CreateTempGroupManager(out string directory, out GroupManager groupManager);
		List<string> files = groupManager.GetAvailableQualityFiles();
		Assert.Empty(files);
		Directory.Delete(directory, true);
	}

	[Fact]
	public void GetAvailableQualityFilesWithMissingDirectoryReturnsEmpty()
	{
		CreateTempGroupManager(out string directory, out GroupManager groupManager);
		Directory.Delete(directory, true);
		List<string> files = groupManager.GetAvailableQualityFiles();
		Assert.Empty(files);
	}

	[Fact]
	public void GetAvailableQualityFilesReturnsMatchingFilesWithoutExtensions()
	{
		CreateTempGroupManager(out string directory, out GroupManager groupManager);

		try
		{
			File.WriteAllText(Path.Combine(directory, "Beta"+GroupManager.FieldQualityProcessingGroupExtension), string.Empty);
			File.WriteAllText(Path.Combine(directory, "Alpha"+GroupManager.FieldQualityProcessingGroupExtension), string.Empty);
			File.WriteAllText(Path.Combine(directory, "Ignored.txt"), string.Empty);

			List<string> files = groupManager.GetAvailableQualityFiles();

			Assert.Equal(["Alpha"+GroupManager.FieldQualityProcessingGroupExtension, "Beta"+GroupManager.FieldQualityProcessingGroupExtension], files);
		}
		finally
		{
			Directory.Delete(directory, true);
		}
	}

	[Fact]
	public void DeserializeWithoutIncludingReadsIncludes()
	{
		string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.xml");
		string xml =
			"""
			<?xml version="1.0" encoding="utf-8"?>
			<qualityprocessor xmlns:xi="http://www.w3.org/2001/XInclude">
				<fieldprocessorgroups>
					<xi:include href="LaTeX Field Quality Processing.fqpg" />
				</fieldprocessorgroups>
			</qualityprocessor>
			""";

		File.WriteAllText(path, xml);

		try
		{
			GroupManager? groupManager = GroupManager.Deserialize(path);

			Assert.NotNull(groupManager);
			Assert.Single(groupManager.Includes);
			Assert.Equal("LaTeX Field Quality Processing"+GroupManager.FieldQualityProcessingGroupExtension, groupManager.Includes[0].Href);
		}
		finally
		{
			File.Delete(path);
		}
	}

	#region Helper Methods

	private static void CreateTempGroupManager(out string directory, out GroupManager groupManager)
	{
		directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
		Directory.CreateDirectory(directory);

		string path = Path.Combine(directory, "Field Quality Processing" + GroupManager.FieldQualityManagerExtension);

		File.WriteAllText(path,
			"""
			<?xml version="1.0" encoding="utf-8"?>
			<qualityprocessor>
				<fieldprocessorgroups />
			</qualityprocessor>
			""");

		groupManager = GroupManager.Deserialize(path)!;
	}

	#endregion
}