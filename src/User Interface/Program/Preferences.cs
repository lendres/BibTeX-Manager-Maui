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

    ///// <summary>
    ///// Use paths relative to the bibliography file.
    ///// </summary>
    //public static bool UsePathsRelativeToBibFile
    //{
    //    get => GetValueOrDefault<bool>(false);
    //    set => SetValue(value);
    //}

    ///// <summary>
    ///// Determines if the bibiography entry initialization file.
    ///// </summary>
    //public static bool UseBibEntryInitialization
    //{
    //    get => GetValueOrDefault<bool>(false);
    //    set => SetValue(value);
    //}

    ///// <summary>
    ///// The path to the bibiography entry initialization file.
    ///// </summary>
    //public static string BibEntryInitializationFile
    //{
    //    get => GetValueOrDefault<string>(string.Empty);
    //    set => SetValue(value);
    //}

    ///// <summary>
    ///// The path to the bibiography file.
    ///// </summary>
    //public static string BibliographyFile
    //{
    //    get => GetValueOrDefault<string>(string.Empty);
    //    set => SetValue(value);
    //}

    ///// <summary>
    ///// Replace tag values with string constants.
    ///// </summary>
    //public static bool UseAuxiliaryFile
    //{
    //    get => GetValueOrDefault<bool>(false);
    //    set => SetValue(value);
    //}

    ///// <summary>
    ///// Assessory files that contain things like strings.
    ///// </summary>
    //public static string AuxiliaryFile
    //{
    //    get => GetValueOrDefault<string>(string.Empty);
    //    set => SetValue(value);
    //}

    ///// <summary>
    ///// Replace tag values with string constants.
    ///// </summary>
    //public static bool UseStringConstants
    //{
    //    get => GetValueOrDefault<bool>(false);
    //    set => SetValue(value);
    //}

    ///// <summary>
    ///// Specifies if the tags should be processed to ensure their quality.
    ///// </summary>
    //public static bool UseTagQualityProcessing
    //{
    //    get => GetValueOrDefault<bool>(false);
    //    set => SetValue(value);
    //}

    ///// <summary>
    ///// The path to the quality processor file.
    ///// </summary>
    //public static string TagQualityProcessingFile
    //{
    //    get => GetValueOrDefault<string>(string.Empty);
    //    set => SetValue(value);
    //}

    ///// <summary>
    ///// Use BibEntry remapping.
    ///// </summary>
    //public static bool UseBibEntryRemapping
    //{
    //    get => GetValueOrDefault<bool>(false);
    //    set => SetValue(value);
    //}

    ///// <summary>
    ///// The path to the bibliography remapping file.
    ///// </summary>
    //public static string BibEntryRemappingFile
    //{
    //    get => GetValueOrDefault<string>(string.Empty);
    //    set => SetValue(value);
    //}

    /// <summary>
    /// The settings for writing the bibliography file.
    /// </summary>
    public static WriteSettings WriteSettings
    {
		get
		{
			string serializedSettings = GetValueOrDefault<string>("");
			if (string.IsNullOrEmpty(serializedSettings))
			{
				return new WriteSettings();
			}
			else
			{
				return Serialization.DeserializeObjectFromString<WriteSettings>(serializedSettings)!;
			}
		}

        set
        {
			string serializedSettings = Serialization.SerializeObjectToString(value);
			SetValue<string>(serializedSettings);
        }
    }

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

	///// <summary>
	///// The settings for writing the bibliography file.
	///// </summary>
	//public static bool AutoGenerateKeys
 //   {
 //       get => GetValueOrDefault<bool>(true);
 //       set => SetValue(value);
 //   }

 //   /// <summary>
 //   /// Copy the bibliography entry's cite key when the entry is added.
 //   /// </summary>
 //   public static bool CopyCiteKeyOnEntryAdd
 //   {
 //       get => GetValueOrDefault<bool>(true);
 //       set => SetValue(value);
 //   }

 //   /// <summary>
 //   /// Sort the bibliography.
 //   /// </summary>
 //   public static bool SortBibliography
 //   {
 //       get => GetValueOrDefault<bool>(true);
 //       set => SetValue(value);
 //   }

 //   /// <summary>
 //   /// Method to sort the bibliography by.
 //   /// </summary>
 //   public static SortBy BibliographySortMethod
 //   {
 //       get => GetValueOrDefault<SortBy>(SortBy.Key);
 //       set => SetValue(value);
 //   }

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