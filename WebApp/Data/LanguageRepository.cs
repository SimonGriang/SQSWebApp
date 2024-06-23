using System.Collections.Generic;
using System.Linq;
using WebApp.Models;

namespace WebApp.Data
{
    /// <summary>
    /// Represents a repository for managing languages in the application.
    /// </summary>
    public class LanguageRepository : ILanguageRepository
    {
        private readonly WebAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public LanguageRepository(WebAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new language to the repository.
        /// </summary>
        /// <param name="language">The language to add.</param>
        public void AddLanguage(Language language)
        {
            _context.Language.Add(language);
            _context.SaveChanges();
        }
        
        /// <summary>
        /// Removes a language from the repository by its ID.
        /// </summary>
        /// <param name="id">The ID of the language to remove.</param>
        public void RemoveLanguage(int id)
        {
            var language = _context.Language.FirstOrDefault(l => l.ID == id);
            if (language != null)
            {
                _context.Language.Remove(language);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves a language from the repository by its ID.
        /// </summary>
        /// <param name="id">The ID of the language to retrieve.</param>
        /// <returns>The language with the specified ID, or null if not found.</returns>
        public Language? GetLanguage(int id)
        {
            return _context.Language.FirstOrDefault(l => l.ID == id);
        }

        /// <summary>
        /// Retrieves all languages from the repository.
        /// </summary>
        /// <returns>A list of all languages.</returns>
        public List<Language> GetAllLanguages()
        {
            return _context.Language.ToList();
        }

        /// <summary>
        /// Checks if a language with the specified ID exists in the repository.
        /// </summary>
        /// <param name="id">The ID of the language to check.</param>
        /// <returns>True if the language exists, false otherwise.</returns>
        public bool LanguageExists(int id)
        {
            return _context.Language.Any(l => l.ID == id);
        }

        /// <summary>
        /// Checks if a language with the specified abbreviation exists in the repository.
        /// </summary>
        /// <param name="abbreviation">The abbreviation of the language to check.</param>
        /// <returns>True if the language exists, false otherwise.</returns>
        public bool LanguageExistsByAbbreviation(string abbreviation)
        {
            return _context.Language.Any(l => l.Abbreviation == abbreviation);
        }

        /// <summary>
        /// Retrieves a language from the repository by its abbreviation.
        /// </summary>
        /// <param name="abbreviation">The abbreviation of the language to retrieve.</param>
        /// <returns>The language with the specified abbreviation, or null if not found.</returns>
        public Language? returnLanguageByAbbreviation(string abbreviation)
        {
            return _context.Language.FirstOrDefault(l => l.Abbreviation == abbreviation);
        }
    }
}