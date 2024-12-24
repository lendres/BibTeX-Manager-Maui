using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Validation;

namespace BibTexManager.ViewModels;

public partial class MainViewModel : ObservableObject
{
	#region Fields

	[ObservableProperty]
	private ValidatableObject<string>			_xmlInputFile					= new();

	[ObservableProperty]
	private ValidatableObject<string>			_xsltFile						= new();

	[ObservableProperty]
	private ValidatableObject<string>			_xsltArguments					= new();

	[ObservableProperty]
	private ValidatableObject<string>			_outputFile						= new();

	[ObservableProperty]
	private ValidatableObject<string>			_outputDirectory				= new();

	[ObservableProperty]
	private bool								_runPostprocessing				= false;

	[ObservableProperty]
	private ValidatableObject<string>			_postprocessor					= new();

	[ObservableProperty]
	private bool								_isSubmittable;

	#endregion

	#region Construction

	public MainViewModel()
    {
		InitializeValues();
		AddValidations();
		ValidateSubmittable();
	}

	private void InitializeValues()
	{
		XmlInputFile.Value		= Preferences.XmlInputFile;
		XsltFile.Value			= Preferences.XsltFile;
		XsltArguments.Value		= Preferences.XsltArguments;
		OutputFileFullPath		= Preferences.OutputFile;
		RunPostprocessing		= Preferences.RunPostprocessor;
		Postprocessor.Value		= Preferences.Postprocessor;
	}

	private void AddValidations()
	{
		XmlInputFile.Validations.Add(new IsNotNullOrEmptyRule	{ ValidationMessage = "A file name is required." });
		XmlInputFile.Validations.Add(new FileExistsRule			{ ValidationMessage = "The file does not exist." });
		XmlInputFile.Validate();

		XsltFile.Validations.Add(new IsNotNullOrEmptyRule	{ ValidationMessage = "A file name is required." });
		XsltFile.Validations.Add(new FileExistsRule			{ ValidationMessage = "The file does not exist." });
		XsltFile.Validate();

		OutputFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		OutputFile.Validate();

		OutputDirectory.Validations.Add(new IsNotNullOrEmptyRule	{ ValidationMessage = "A directory is required." });
		OutputDirectory.Validations.Add(new DirectoryExistsRule		{ ValidationMessage = "The directory does not exist." });
		OutputDirectory.Validate();

		OnRunPostprocessingChanged(RunPostprocessing);
	}

	#endregion

	#region Properties

	public string OutputFileFullPath
	{
		get => Path.Combine(OutputDirectory.Value!.Trim(), OutputFile.Value!.Trim());

		set
		{
			OutputFile.Value		= Path.GetFileName(value);
			OutputDirectory.Value	= Path.GetDirectoryName(value);
		}
	}

	#endregion

	#region Validation

	[RelayCommand]
	private void ValidateXmlInputFile()
	{
		XmlInputFile.Validate();
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateXsltFile()
	{
		XsltFile.Validate();
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateOutputFile()
	{
		OutputFile.Validate();
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateOutputDirectory()
	{
		OutputDirectory.Validate();
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidatePostprocessor()
	{
		Postprocessor.Validate();
        ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable =
		XmlInputFile.IsValid &&
		XsltFile.IsValid &&
		OutputFile.IsValid &&
		OutputDirectory.IsValid &&
		Postprocessor.IsValid;

	#endregion

	#region On Properties Changed

	partial void OnRunPostprocessingChanged(bool value)
	{
		if (value)
		{ 
			Postprocessor.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
			Postprocessor.Validations.Add(new FileExistsRule { ValidationMessage = "The file does not exist." });
			ValidatePostprocessor();
		}
		else
		{
			Postprocessor.Validations.Clear();
			ValidatePostprocessor();
		}
	}

	#endregion

	#region Methods and Commands

	[RelayCommand]
	private void ClearXsltArguments()
	{
		XsltArguments.Value = "";
	}

	private void SaveSettings()
	{
		Preferences.XmlInputFile		= XmlInputFile.Value!.Trim();
		Preferences.XsltFile			= XsltFile.Value!.Trim();
		Preferences.XsltArguments		= XsltArguments.Value!.Trim();
		Preferences.OutputFile			= OutputFileFullPath;
		Preferences.RunPostprocessor	= RunPostprocessing;
		Preferences.Postprocessor		= Postprocessor.Value!.Trim();
	}

	#endregion

} // End class.