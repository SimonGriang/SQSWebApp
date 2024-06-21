using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.DBSeeding;
using WebApp.Models;
using WebApp.Services;
using WebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Tests
{
    [TestClass]
    [TestCategory("UnitTests")]
    public class LanguageSeedingTest
    {
        private Mock<ITranslationService> _translationServiceMock = new Mock<ITranslationService>();
        private WebAppContext _context;
        private IServiceProvider _serviceProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            // Mock für den TranslationService
            _translationServiceMock = new Mock<ITranslationService>();

            // Setup für den TranslationService Mock
            var languages = new List<Language>
            {
                new Language { Name = "German", Abbreviation = "de", isTargetLanguage = true, isOriginLanguage = false },
                new Language { Name = "English", Abbreviation = "en-US", isTargetLanguage = true, isOriginLanguage = false },
                new Language { Name = "Detect Language", Abbreviation = "DL", isTargetLanguage = false, isOriginLanguage = true },
                new Language { Name = "English", Abbreviation = "en", isTargetLanguage = false, isOriginLanguage = true }
            };
            _translationServiceMock.Setup(x => x.getDeeplLanguages()).ReturnsAsync(languages);

            // DI-Container konfigurieren
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<WebAppContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));
            serviceCollection.AddSingleton<ITranslationService>(_translationServiceMock.Object);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            // Erhalte den WebAppContext aus dem ServiceProvider
            _context = _serviceProvider.GetRequiredService<WebAppContext>();

            // Datenbank bereinigen
            _context.Language.RemoveRange(_context.Language);
            _context.SaveChanges();
        }

        [TestMethod]
        public void Initialize_ShouldSeedLanguages()
        {
            // Act
            LanguageSeeding.Initialize(_serviceProvider);

            // Assert
            Assert.AreEqual(6, _context.Language.Count()); // Überprüfe, ob alle Sprachen korrekt hinzugefügt wurden
        }

        [TestMethod]
        public void Initialize_ShouldNotSeedLanguagesIfTheyAlreadyExist()
        {
            // Arrange
            _context.Language.AddRange(new List<Language>
            {
                new Language { Name = "German", Abbreviation = "de", isTargetLanguage = true, isOriginLanguage = false },
                new Language { Name = "English", Abbreviation = "en-US", isTargetLanguage = true, isOriginLanguage = false },
                new Language { Name = "Detect Language", Abbreviation = "DL", isTargetLanguage = false, isOriginLanguage = true },
                new Language { Name = "English", Abbreviation = "en", isTargetLanguage = false, isOriginLanguage = true }
            });
            _context.SaveChanges();

            // Act
            LanguageSeeding.Initialize(_serviceProvider);

            // Assert
            Assert.AreEqual(4, _context.Language.Count()); // Überprüfe, ob keine weiteren Sprachen hinzugefügt wurden
        }

        [TestMethod]
        public void Initialize_ShouldSeedLanguagesIfTheyPartiallyExist()
        {
            // Arrange
            _context.Language.AddRange(new List<Language>
            {
                new Language { Name = "German", Abbreviation = "de", isTargetLanguage = true, isOriginLanguage = false },
                new Language { Name = "English", Abbreviation = "en-US", isTargetLanguage = true, isOriginLanguage = false }
            });
            _context.SaveChanges();

            // Act
            LanguageSeeding.Initialize(_serviceProvider);

            // Assert
            Assert.AreEqual(6, _context.Language.Count()); // Überprüfe, ob die fehlenden Sprachen hinzugefügt wurden
        }
    }
}
