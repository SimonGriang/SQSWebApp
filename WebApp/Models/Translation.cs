using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    /// <summary>
    /// Represents a translation entity.
    /// </summary>
    public class Translation
    {
        /// <summary>
        /// Gets or sets the ID of the translation.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the original text to be translated.
        /// </summary>
        [Required(ErrorMessage = "OriginalText is required")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "OriginalText must be between 1 and 500 characters")]
        public string? OriginalText { get; set; }

        /// <summary>
        /// Gets or sets the translated text.
        /// </summary>
        public string? TranslatedText { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the translation was made.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? Translated_at { get; set; }

        /// <summary>
        /// Gets or sets the original language of the text.
        /// </summary>
        public Language? OriginalLanguage { get; set; }

        /// <summary>
        /// Gets or sets the translated language of the text.
        /// </summary>
        public Language? TranslatedLanguage { get; set; }
    }
}
