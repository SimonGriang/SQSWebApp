using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Language
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Abbreviation { get; set; }

        public bool isTargetLanguage { get; set; } = false;
        public bool isOriginLanguage { get; set; } = false;

        public Language()
        {
            Name = string.Empty;
        }

        public Language(string name, string code)
        {
            Name = name;
            Abbreviation = code;
        }
    }
}
