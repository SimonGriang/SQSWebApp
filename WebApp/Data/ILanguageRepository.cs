using WebApp.Models;

public interface ILanguageRepository
{
    void AddLanguage(Language language);
    void RemoveLanguage(int id);
    Language? GetLanguage(int id);
    List<Language> GetAllLanguages();
    bool LanguageExists(int id);
    bool LanguageExistsByAbbreviation(string abbreviation);
    Language? returnLanguageByAbbreviation(string abbreviation);
}