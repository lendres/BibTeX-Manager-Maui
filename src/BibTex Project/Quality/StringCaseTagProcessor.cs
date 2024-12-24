using DigitalProduction.Strings;
using System.Xml.Serialization;

namespace BibtexManager
{
	/// <summary>
	/// Changes the case of text.
	/// </summary>
	public class StringCaseTagProcessor : TagProcessor
	{
		#region Fields

		private StringCase          _toCase             = StringCase.TitleCase;
		private string              _culture            = "en-US";

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public StringCaseTagProcessor()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// The case to convert the text to.
		/// </summary>
		[XmlAttribute("tocase")]
		public StringCase StringCase { get => _toCase; set => _toCase = value; }

		/// <summary>
		/// The culture to use when converting text.
		/// </summary>
		[XmlAttribute("culture")]
		public string Culture { get => _culture; set => _culture = value; }
		
		#endregion

		#region Methods

		/// <summary>
		/// Gets the replacement string for the input (original) string.
		/// </summary>
		/// <param name="correction">Correction information.</param>
		protected override void ProcessPatternMatch(Correction correction)
		{
			correction.ReplacementText	= Format.ChangeCase(correction.MatchedText, _toCase, _culture);

			bool replaceText			= _toCase != StringCase.None && correction.ReplacementText != correction.MatchedText;

			correction.ReplaceText		= replaceText;
			correction.PromptUser		= replaceText;
		}

		#endregion

	} // End class.
}