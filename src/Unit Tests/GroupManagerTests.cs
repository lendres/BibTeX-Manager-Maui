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

	[Theory]
	[InlineData("")]
	[InlineData("   ")]
	public void GetAvailableQualityFilesWithEmptyPathReturnsEmpty(string fieldQualityProcessingFile)
	{
		List<string> files = GroupManager.GetAvailableQualityFiles();

		Assert.Empty(files);
	}

	[Fact]
	public void GetAvailableQualityFilesWithMissingDirectoryReturnsEmpty()
	{
		string fieldQualityProcessingFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "Field Quality Processing.qlty");

		List<string> files = GroupManager.GetAvailableQualityFiles();

		Assert.Empty(files);
	}

	[Fact]
	public void GetAvailableQualityFilesReturnsMatchingFilesWithoutExtensions()
	{
		string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
		Directory.CreateDirectory(directory);

		try
		{
			File.WriteAllText(Path.Combine(directory, "Beta.qlty"), string.Empty);
			File.WriteAllText(Path.Combine(directory, "Alpha.qlty"), string.Empty);
			File.WriteAllText(Path.Combine(directory, "Field Quality Processing.qlty"), string.Empty);
			File.WriteAllText(Path.Combine(directory, "Ignored.txt"), string.Empty);

			List<string> files = GroupManager.GetAvailableQualityFiles(Path.Combine(directory, "Field Quality Processing.qlty"));

			Assert.Equal(["Alpha", "Beta"], files);
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
					<xi:include href="LaTeX Field Quality Processing.qlty" />
				</fieldprocessorgroups>
			</qualityprocessor>
			""";

		File.WriteAllText(path, xml);

		try
		{
			GroupManager? groupManager = GroupManager.Deserialize(path);

			Assert.NotNull(groupManager);
			Assert.Single(groupManager.Includes);
			Assert.Equal("LaTeX Field Quality Processing.qlty", groupManager.Includes[0].Href);
		}
		finally
		{
			File.Delete(path);
		}
	}
}