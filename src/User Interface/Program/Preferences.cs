using DigitalProduction.Maui.Services;
using DigitalProduction.Xml.Serialization;
using System.Runtime.CompilerServices;

namespace BibTeXManager;

/// <summary>
/// Registry access and setting storage.
/// </summary>
public static class Preferences
{
    #region Fields

    private static readonly IRecentPathsManagerService _recentPathsManagerService = 
		DigitalProduction.Maui.Services.ServiceProvider.GetService<IRecentPathsManagerService>();

    #endregion

    #region Bibliography Settings

	/// <summary>
	/// The settings for writing the bibliography file.
	/// </summary>
	public static ProjectSettings ProjectSettings
	{
		get => GetInstanceOrDefault<ProjectSettings>();
		set => SetInstance<ProjectSettings>(value);
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
		get => GetValueOrDefault(false);
		set => SetValue(value);
	}

    #endregion

    private static T GetInstanceOrDefault<T>([CallerMemberName] string propertyName = "") where T : new()

	{
		string serializedSettings = GetValueOrDefault("", propertyName);
		if (string.IsNullOrEmpty(serializedSettings))
		{
			return new T();
		}
		else
		{
			return Serialization.DeserializeObjectFromString<T>(serializedSettings)!;
		}
    }

    private static void SetInstance<T>(T? value, [CallerMemberName] string propertyName = "")
    {
        string serializedSettings = Serialization.SerializeObjectToString(value);
		SetValue(serializedSettings, propertyName);
    }

    private static string GetValueOrDefault(string defaultValue, [CallerMemberName] string propertyName = "")
    {
        return GetValueOrDefault<string>(defaultValue, propertyName);
    }

    private static void SetValue(string value, [CallerMemberName] string propertyName = "")
    {
        SetValue<string>(value, propertyName);
    }

    private static bool GetValueOrDefault(bool defaultValue, [CallerMemberName] string propertyName = "")
    {
        return GetValueOrDefault<bool>(defaultValue, propertyName);
    }

    private static void SetValue(bool value, [CallerMemberName] string propertyName = "")
    {
        SetValue<bool>(value, propertyName);
    }

    private static T GetValueOrDefault<T>(T defaultValue, string propertyName)
    {
        return Microsoft.Maui.Storage.Preferences.Default.Get(propertyName, defaultValue);
    }

    private static void SetValue<T>(T? value, string propertyName)
    {
        Microsoft.Maui.Storage.Preferences.Default.Set(propertyName, value);
    }
}