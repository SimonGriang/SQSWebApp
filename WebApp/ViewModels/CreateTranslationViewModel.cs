using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels
{
    /// <summary>
    /// Represents the view model for creating a translation.
    /// </summary>
    public class CreateTranslationViewModel
    {
        /// <summary>
        /// Gets or sets the list of target languages.
        /// </summary>
        public List<Language>? targetLanguages { get; set; }

        /// <summary>
        /// Gets or sets the list of origin languages.
        /// </summary>
        public List<Language>? originLanguages { get; set; }

        /// <summary>
        /// Gets or sets the translation.
        /// </summary>
        [Required(ErrorMessage = "Translation ist erforderlich")]
        public Translation? Translation { get; set; }

        /// <summary>
        /// Gets or sets the ID of the language from which the translation is made.
        /// </summary>
        [Required(ErrorMessage = "LanguageFrom darf nicht 0 sein")]
        public int LanguageFrom { get; set; }

        /// <summary>
        /// Gets or sets the ID of the target language for the translation.
        /// </summary>
        [Required(ErrorMessage = "LanguageTo darf nicht 0 sein")]
        public int LanguageTo { get; set; }

        /// <summary>
        /// Gets or sets the English language ID.
        /// </summary>
        public int? English { get; set; }

        /// <summary>
        /// Gets or sets the English (US) language ID.
        /// </summary>
        public int? EnglishUS { get; set; }

        /// <summary>
        /// Gets or sets the English (GB) language ID.
        /// </summary>
        public int? EnglishGB { get; set; }

        /// <summary>
        /// Gets or sets the German language ID.
        /// </summary>
        public int? German { get; set; }

        /// <summary>
        /// Gets or sets the ID for detecting the language.
        /// </summary>
        public int? DetectLanguage { get; set; }

        /// <summary>
        /// Custom validation method for validating the LanguageTo property.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>The validation result.</returns>
        public ValidationResult ValidateLanguageTo(ValidationContext validationContext)
        {
            if (targetLanguages == null || !targetLanguages.Exists(l => true))
            {
                return new ValidationResult("Keine Sprachen vorhanden, Validierung übersprungen.");
            }

            if (LanguageTo <= 0 || !targetLanguages.Exists(l => l.ID == LanguageTo))
            {
                return new ValidationResult("Bitte wählen Sie eine gültige Zielsprache aus.");
            }

            return ValidationResult.Success!;
        }

        /// <summary>
        /// Custom validation method for validating the originLanguages property.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>The validation result.</returns>
        public ValidationResult ValidateOriginLanguages(ValidationContext validationContext)
        {
            if (originLanguages == null || !originLanguages.Exists(l => true))
            {
                return new ValidationResult("Keine Sprachen vorhanden, Validierung übersprungen.");
            }

            if (LanguageFrom <= 0 || !originLanguages.Exists(l => l.ID == LanguageFrom))
            {
                return new ValidationResult("Bitte wählen Sie eine gültige Ausgangssprache aus.");
            }

            return ValidationResult.Success!;
        }
    }
}
