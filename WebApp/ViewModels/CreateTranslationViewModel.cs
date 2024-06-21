using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class CreateTranslationViewModel
    {
        public List<Language>? targetLanguages { get; set; }

        public List<Language>? originLanguages { get; set; }

        [Required(ErrorMessage = "Translation ist erforderlich")]
        public Translation? Translation { get; set; }

        [Required(ErrorMessage = "LanguageFrom darf nicht 0 sein")]
        public int? LanguageFrom { get; set; }

        [Required(ErrorMessage = "LanguageTo darf nicht 0 sein")]
        public int? LanguageTo { get; set; }

        public int? English { get; set; }
        public int? EnglishUS { get; set; }
        public int? EnglishGB { get; set; }
        public int? German { get; set; }
        public int? DetectLanguage { get; set; }

        // Benutzerdefinierte Validierungsmethode für LanguageTo
        public ValidationResult ValidateLanguageTo(ValidationContext validationContext)
        {
            if (targetLanguages == null || !targetLanguages.Any())
            {
                return ValidationResult.Success!; // Wenn keine Sprachen vorhanden sind, überspringe die Validierung
            }

            if (LanguageTo <= 0 || targetLanguages.Count(l => l.ID == LanguageTo) > 0)
            {
                return new ValidationResult("Bitte wählen Sie eine gültige Zielsprache aus.");
            }
            return ValidationResult.Success!;
        }

        public ValidationResult ValidateOriginLanguages(ValidationContext validationContext)
        {
            if (originLanguages == null || !originLanguages.Exists(l => l.isOriginLanguage))
            {
                return ValidationResult.Success!; // Wenn keine Sprachen vorhanden sind, überspringe die Validierung
            }

            if (LanguageFrom <= 0 || originLanguages.Count(l => l.ID == LanguageFrom) > 0)
            {
                return new ValidationResult("Bitte wählen Sie eine gültige Ausgangssprache aus.");
            }
            return ValidationResult.Success!;
        }
    }
}
