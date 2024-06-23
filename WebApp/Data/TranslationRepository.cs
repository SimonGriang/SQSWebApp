using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.ViewModels;
using WebApp.Data;


namespace WebApp.Data
{
    /// <summary>
    /// Represents a repository for managing translations.
    /// </summary>
    public class TranslationRepository : ITranslationRepository
    {
        private readonly WebAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public TranslationRepository(WebAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new translation to the repository.
        /// </summary>
        /// <param name="translation">The translation to add.</param>
        public void AddTranslation(Translation translation)
        {
            _context.Translation.Add(translation);
            _context.SaveChanges();
        }   

        /// <summary>
        /// Retrieves all translations from the repository.
        /// </summary>
        /// <returns>A list of all translations.</returns>
        public List<Translation> GetAllTranslations()
        {
            return _context.Translation.Include(t => t.OriginalLanguage).Include(t=>t.TranslatedLanguage).ToList();
        }

        /// <summary>
        /// Checks if a translation with the specified ID exists in the repository.
        /// </summary>
        /// <param name="id">The ID of the translation.</param>
        /// <returns>True if the translation exists, otherwise false.</returns>
        public bool TranslationExists(int id)
        {
            return _context.Translation.Any(e => e.ID == id);
        }

        /// <summary>
        /// Retrieves a translation from the repository by its ID.
        /// </summary>
        /// <param name="id">The ID of the translation.</param>
        /// <returns>The translation with the specified ID, or null if not found.</returns>
        public Translation? GetTranslationById(int id)
        {
            return _context.Translation.Include(t => t.OriginalLanguage).Include(t => t.TranslatedLanguage).SingleOrDefault(t => t.ID == id);
        }

        /// <summary>
        /// Deletes a translation from the repository by its ID.
        /// </summary>
        /// <param name="id">The ID of the translation to delete.</param>
        public void DeleteTranslation(int id)
        {
            var translation = _context.Translation.FirstOrDefault(t => t.ID == id);
            if (translation != null)
            {
                _context.Translation.Remove(translation);
                _context.SaveChanges();
            }
        }
    }
}