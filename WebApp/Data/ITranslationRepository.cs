using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.Data
{
    public interface ITranslationRepository
    {
        void AddTranslation(Translation translation);
        List<Translation> GetAllTranslations();
        bool TranslationExists(int id);
        Translation? GetTranslationById(int id);
        void UpdateTranslation(Translation translation);
        void DeleteTranslation(int id);
    }
}
