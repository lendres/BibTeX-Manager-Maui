using DigitalProduction.Maui.Validation;

namespace BibTexManager.Validation;

/// <summary>
/// File must exist validation rule.
/// </summary>
/// <remarks>
/// Based on:
/// https://github.com/dotnet-architecture/eshop-mobile-client
/// Copyright (c) 2020 .NET Application Architecture - Reference Apps
/// </remarks>
public class RelativePathExistsRule : FileExistsBase
{
	public string	MainPath { get; set; }				= string.Empty;
	public bool		UsingRelativePaths { get; set; }	= false;

	public override bool Check(string? value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}
		return FileExists(ConvertToAbsolutePath(value));
	}

	/// <summary>
	/// Convert a path to absolute path if the relative path option is in use.
	/// </summary>
	/// <param name="path">Path to convert.</param>
	private string ConvertToAbsolutePath(string path)
	{
		if (UsingRelativePaths)
		{
			path = DigitalProduction.IO.Path.ConvertToAbsolutePath(path, Path.GetDirectoryName(MainPath) ?? "");
		}
		return path;
	}
}