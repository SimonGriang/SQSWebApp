using System.Collections.Generic;
using System.Linq;
using WebApp.Models;

namespace WebApp.Data
{
    public class LanguageRepository
    {
        private readonly WebAppContext _context;

        public LanguageRepository(WebAppContext context)
        {
            _context = context;
        }

        public void AddLanguage(Language language)
        {
            _context.Language.Add(language);
            _context.SaveChanges();
        }
        
        public void RemoveLanguage(int id)
        {
            var language = _context.Language.FirstOrDefault(l => l.ID == id);
            if (language != null)
            {
                _context.Language.Remove(language);
                _context.SaveChanges();
            }
        }

        public Language? GetLanguage(int id)
        {
            return _context.Language.FirstOrDefault(l => l.ID == id);
        }

        public List<Language> GetAllLanguages()
        {
            return _context.Language.ToList();
        }

        public bool LanguageExists(int id)
        {
            return _context.Language.Any(l => l.ID == id);
        }
    }
}