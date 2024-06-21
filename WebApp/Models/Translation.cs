using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Translation
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "OriginalText is required")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "OriginalText must be between 1 and 500 characters")]
        public string? OriginalText { get; set; }

        public string? TranslatedText { get; set; }

        [DataType(DataType.Date)]
        public DateTime? translated_at { get; set; }


        public Language? OriginalLanguage { get; set; }
        public Language? TranslatedLanguage { get; set; }
    }
}
