using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Models;
using System;
using System.Linq;
using Polly;
using WebApp.Data;
using WebApp.Services;

namespace WebApp.DBSeeding
{
    public class LanguageSeeding
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new WebAppContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<WebAppContext>>()))
            {
                // Look for any languages.
                if (context.Language.Any())
                {
                    return;   // DB has been seeded
                }

                List<Language> languages = new List<Language>();
                TranslationService translationService = new TranslationService(context);
                Task<List<Language>> allLanguages = translationService.getDeeplLanguages();
                languages = allLanguages.GetAwaiter().GetResult();

                // Add languages to the database
                context.Language.AddRange(languages);

                // Add additional languages
                Language detectLanguage = new Language
                {
                    Name = "Detect Language",
                    Abbreviation = "DL",
                    isOriginLanguage = true,
                    isTargetLanguage = false
                };

                Language englishLanguage = new Language
                {
                    Name = "English",
                    Abbreviation = "en",
                    isOriginLanguage = true,
                    isTargetLanguage = false
                };

                context.Language.AddRange(detectLanguage, englishLanguage);

                context.SaveChanges();
            }
        }
    }
}
