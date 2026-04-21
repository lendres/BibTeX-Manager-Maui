using BibTeXLibrary;
using DigitalProduction.Maui.Services;
using DigitalProduction.Xml;
using DigitalProduction.Xml.Serialization;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace BibTeXManager;

/// <summary>
/// Registry access and setting storage.
/// </summary>
public static class Preferences
{
    #region Fields

    private static readonly IRecentPathsManagerService      _recentPathsManagerService = 
		DigitalProduction.Maui.Services.ServiceProvider.GetService<IRecentPathsManagerService>();

    #endregion

    #region Bibliography Settings

	/// <summary>
	/// The settings for writing the bibliography file.
	/// </summary>
	public static ProjectSettings ProjectSettings
	{
		get
		{
			string serializedSettings = GetValueOrDefault<string>("");
			if (string.IsNullOrEmpty(serializedSettings))
			{
				return new ProjectSettings();
			}
			else
			{
				return Serialization.DeserializeObjectFromString<ProjectSettings>(serializedSettings)!;
			}
		}

		set
		{
			string serializedSettings = Serialization.SerializeObjectToString(value);
			SetValue<string>(serializedSettings);
		}
	}

    #endregion

    #region Program Settings

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

    #endregion

    private static void SetValue<T>(T? value, [CallerMemberName] string propertyName = "")
    {
        Microsoft.Maui.Storage.Preferences.Default.Set(propertyName, value);
    }

    private static T GetValueOrDefault<T>(T defaultValue, [CallerMemberName] string propertyName = "")
    {
        return Microsoft.Maui.Storage.Preferences.Default.Get(propertyName, defaultValue);
    }
}