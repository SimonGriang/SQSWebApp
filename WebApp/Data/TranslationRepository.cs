using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.ViewModels;
using WebApp.Data;


namespace WebApp.Data
{
    public class TranslationRepository : ITranslationRepository
    {
        private readonly WebAppContext _context;

        public TranslationRepository(WebAppContext context)
        {
            _context = context;
        }

        public void AddTranslation(Translation translation)
        {
            _context.Translation.Add(translation);
            _context.SaveChanges();
        }   

        public List<Translation> GetAllTranslations()
        {
            return _context.Translation.Include(t => t.OriginalLanguage).Include(t=>t.TranslatedLanguage).ToList();
        }

        public bool TranslationExists(int id)
        {
            return _context.Translation.Any(e => e.ID == id);
        }

        public Translation? GetTranslationById(int id)
        {
            return _context.Translation.Include(t => t.OriginalLanguage).Include(t => t.TranslatedLanguage).SingleOrDefault(t => t.ID == id);
        }

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