using WebApp.Models;

namespace WebApp.Data
{
    /// <summary>
    /// Represents a repository for managing languages.
    /// </summary>
    public interface ILanguageRepository
    {
        /// <summary>
        /// Adds a new language to the repository.
        /// </summary>
        /// <param name="language">The language to add.</param>
        void AddLanguage(Language language);

        /// <summary>
        /// Removes a language from the repository based on its ID.
        /// </summary>
        /// <param name="id">The ID of the language to remove.</param>
        void RemoveLanguage(int id);

        /// <summary>
        /// Retrieves a language from the repository based on its ID.
        /// </summary>
        /// <param name="id">The ID of the language to retrieve.</param>
        /// <returns>The language with the specified ID, or null if not found.</returns>
        Language? GetLanguage(int id);

        /// <summary>
        /// Retrieves all languages from the repository.
        /// </summary>
        /// <returns>A list of all languages in the repository.</returns>
        List<Language> GetAllLanguages();

        /// <summary>
        /// Checks if a language with the specified ID exists in the repository.
        /// </summary>
        /// <param name="id">The ID of the language to check.</param>
        /// <returns>True if the language exists, false otherwise.</returns>
        bool LanguageExists(int id);

        /// <summary>
        /// Checks if a language with the specified abbreviation exists in the repository.
        /// </summary>
        /// <param name="abbreviation">The abbreviation of the language to check.</param>
        /// <returns>True if the language exists, false otherwise.</returns>
        bool LanguageExistsByAbbreviation(string abbreviation);

        /// <summary>
        /// Retrieves a language from the repository based on its abbreviation.
        /// </summary>
        /// <param name="abbreviation">The abbreviation of the language to retrieve.</param>
        /// <returns>The language with the specified abbreviation, or null if not found.</returns>
        Language? returnLanguageByAbbreviation(string abbreviation);
    }
}