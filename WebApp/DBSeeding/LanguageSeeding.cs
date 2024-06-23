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
    /// <summary>
    /// Class responsible for seeding the Language table in the database.
    /// </summary>
    public static class LanguageSeeding
    {
        /// <summary>
        /// Initializes the Language table with predefined languages.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to access required services.</param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new WebAppContext(
            serviceProvider.GetRequiredService<DbContextOptions<WebAppContext>>()))
            {
                ITranslationService translationService = serviceProvider.GetRequiredService<ITranslationService>();
                Task<List<Language>> deeplLanguages = translationService.getDeeplLanguages();
                List<Language> allLanguages = deeplLanguages.GetAwaiter().GetResult();
                allLanguages.Add(new Language
                {
                    Name = "Detect Language",
                    Abbreviation = "DL",
                    IsOriginLanguage = true,
                    IsTargetLanguage = false
                });

                allLanguages.Add(new Language
                {
                    Name = "English",
                    Abbreviation = "en",
                    IsOriginLanguage = true,
                    IsTargetLanguage = false
                });

                List<Language> languagesToAdd = new List<Language>();

                foreach (var language in allLanguages)
                {
                    if (!context.Language.Any(l => l.Name == language.Name && l.Abbreviation == language.Abbreviation))
                    {
                        languagesToAdd.Add(language);
                    }
                }
                
                if (languagesToAdd.Count > 0)
                {
                    context.Language.AddRange(languagesToAdd);
                    context.SaveChanges();
                }
            }
        }
    }
}
