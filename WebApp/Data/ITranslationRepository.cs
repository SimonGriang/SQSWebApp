using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.Data
{
    /// <summary>
    /// Represents a repository for managing translations.
    /// </summary>
    public interface ITranslationRepository
    {
        /// <summary>
        /// Adds a new translation to the repository.
        /// </summary>
        /// <param name="translation">The translation to add.</param>
        void AddTranslation(Translation translation);

        /// <summary>
        /// Retrieves all translations from the repository.
        /// </summary>
        /// <returns>A list of all translations.</returns>
        List<Translation> GetAllTranslations();

        /// <summary>
        /// Checks if a translation with the specified ID exists in the repository.
        /// </summary>
        /// <param name="id">The ID of the translation.</param>
        /// <returns>True if the translation exists, false otherwise.</returns>
        bool TranslationExists(int id);

        /// <summary>
        /// Retrieves a translation from the repository by its ID.
        /// </summary>
        /// <param name="id">The ID of the translation.</param>
        /// <returns>The translation with the specified ID, or null if not found.</returns>
        Translation? GetTranslationById(int id);

        /// <summary>
        /// Deletes a translation from the repository by its ID.
        /// </summary>
        /// <param name="id">The ID of the translation to delete.</param>
        void DeleteTranslation(int id);
    }
}
