using DigitalProduction.Maui.Services;

using DpmPreferences = DigitalProduction.Maui.Storage.Preferences;

namespace BibTeXManager;

/// <summary>
/// Registry access and setting storage.
/// </summary>
public static class Preferences
{
    #region Fields

    private static readonly IRecentPathsManagerService _recentPathsManagerService = 
		IPlatformApplication.Current!.Services.GetRequiredService<IRecentPathsManagerService>();

    #endregion

    #region Bibliography Settings

	/// <summary>
	/// The settings for writing the bibliography file.
	/// </summary>
	public static ProjectSettings ProjectSettings
	{
		get => DpmPreferences.GetInstance<ProjectSettings>();
		set => DpmPreferences.SetInstance<ProjectSettings>(value);
	}

    #endregion

    #region Program Settings

    /// <summary>
    /// Recent paths.
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
		get => DpmPreferences.Get(false);
		set => DpmPreferences.Set(value);
	}

    #endregion
}