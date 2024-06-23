using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    /// <summary>
    /// Represents a language used in the application.
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Gets or sets the ID of the language.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the language.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation of the language.
        /// </summary>
        public string? Abbreviation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the language is a target language.
        /// </summary>
        public bool IsTargetLanguage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the language is an origin language.
        /// </summary>
        public bool IsOriginLanguage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class.
        /// </summary>
        public Language()
        {
            Name = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class with the specified name and abbreviation.
        /// </summary>
        /// <param name="name">The name of the language.</param>
        /// <param name="code">The abbreviation of the language.</param>
        public Language(string name, string code)
        {
            Name = name;
            Abbreviation = code;
        }
    }
}
