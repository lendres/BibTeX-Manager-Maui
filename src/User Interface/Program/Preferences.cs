using DigitalProduction.Maui.Services;

namespace BibTeXManager;

/// <summary>
/// Registry access and setting storage.
/// </summary>
public static class Preferences
{
	private static readonly IRecentPathsManagerService      _recentPathsManagerService = 
		DigitalProduction.Maui.Services.ServiceProvider.GetService<IRecentPathsManagerService>();

	/// <summary>
	/// Load last project as start up.
	/// </summary>
	public static IRecentPathsManagerService RecentPathsManagerService
	{
		get => _recentPathsManagerService;
	}

	/// <summary>
	/// Load last project as start up.
	/// </summary>
	public static bool LoadLastProjectAtStartUp
	{
		get => Microsoft.Maui.Storage.Preferences.Default.Get("Load Last Project At Start Up", false);
		set => Microsoft.Maui.Storage.Preferences.Default.Set("Load Last Project At Start Up", value);
	}

	/// <summary>
	/// Google search endige cx identifier.
	/// </summary>
	public static string CustomSearchEngineIdentifier
	{
		get => Microsoft.Maui.Storage.Preferences.Default.Get("Custom Search Engine Identifier", "");
		set => Microsoft.Maui.Storage.Preferences.Default.Set("Custom Search Engine Identifier", value);
	}

	/// <summary>
	/// Google search engine API key.
	/// </summary>
	public static string SearchEngineApiKey
	{
		get => Microsoft.Maui.Storage.Preferences.Default.Get("Search Engine Api Key", "");
		set => Microsoft.Maui.Storage.Preferences.Default.Set("Search Engine Api Key", value);
	}
}