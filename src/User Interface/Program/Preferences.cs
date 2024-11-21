namespace BibTexManager;

/// <summary>
/// Registry access and setting storage.
/// </summary>
public static class Preferences
{
	#region Settings Properties

	/// <summary>
	/// XML file location.
	/// </summary>
	public static string XmlInputFile
	{
		get => Microsoft.Maui.Storage.Preferences.Default.Get("XML File", "");
		set => Microsoft.Maui.Storage.Preferences.Default.Set("XML File", value);
	}

	/// <summary>
	/// XSLT file location.
	/// </summary>
	public static string XsltFile
	{
		get => Microsoft.Maui.Storage.Preferences.Default.Get("XSLT File", "");
		set => Microsoft.Maui.Storage.Preferences.Default.Set("XSLT File", value);
	}

	/// <summary>
	/// Xslt arguments.
	/// </summary>
	public static string XsltArguments
	{
		get => Microsoft.Maui.Storage.Preferences.Default.Get("XSLT Arguments", "");
		set => Microsoft.Maui.Storage.Preferences.Default.Set("XSLT Arguments", value);
	}

	/// <summary>
	/// Output file name.
	/// </summary>
	public static string OutputFile
	{
		get => Microsoft.Maui.Storage.Preferences.Default.Get("Output File", "");
		set => Microsoft.Maui.Storage.Preferences.Default.Set("Output File", value);
	}

	/// <summary>
	/// Run postprocessing.
	/// </summary>
	public static bool RunPostprocessor
	{
		get => Microsoft.Maui.Storage.Preferences.Default.Get("Run Postprocessor", false);
		set => Microsoft.Maui.Storage.Preferences.Default.Set("Run Postprocessor", value);
	}

	/// <summary>
	/// Postprocess.
	/// </summary>
	public static string Postprocessor
	{
		get => Microsoft.Maui.Storage.Preferences.Default.Get("Postprocessor", "");
		set => Microsoft.Maui.Storage.Preferences.Default.Set("Postprocessor", value);
	}

	#endregion
}