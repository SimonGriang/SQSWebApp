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
    /// <summary>
    /// Represents a unit test class for the LanguageSeeding class.
    /// </summary>
    [TestClass]
    [TestCategory("UnitTests")]
    public class LanguageSeedingTest
    {
        private Mock<ITranslationService> _translationServiceMock = new Mock<ITranslationService>();
        private WebAppContext _context;
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes the test class before each test method is executed.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            // Mock for the TranslationService
            _translationServiceMock = new Mock<ITranslationService>();

            // Setup for the TranslationService Mock
            var languages = new List<Language>
            {
                new Language { Name = "German", Abbreviation = "de", IsTargetLanguage = true, IsOriginLanguage = false },
                new Language { Name = "English", Abbreviation = "en-US", IsTargetLanguage = true, IsOriginLanguage = false },
                new Language { Name = "Detect Language", Abbreviation = "DL", IsTargetLanguage = false, IsOriginLanguage = true },
                new Language { Name = "English", Abbreviation = "en", IsTargetLanguage = false, IsOriginLanguage = true }
            };
            _translationServiceMock.Setup(x => x.getDeeplLanguages()).ReturnsAsync(languages);

            // Configure DI container
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<WebAppContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));
            serviceCollection.AddSingleton<ITranslationService>(_translationServiceMock.Object);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            // Get WebAppContext from the ServiceProvider
            _context = _serviceProvider.GetRequiredService<WebAppContext>();

            // Clean up the database
            _context.Language.RemoveRange(_context.Language);
            _context.SaveChanges();
        }

        /// <summary>
        /// Tests the Initialize method of the LanguageSeeding class to ensure that all languages are correctly seeded.
        /// </summary>
        [TestMethod]
        public void Initialize_ShouldSeedLanguages()
        {
            // Act
            LanguageSeeding.Initialize(_serviceProvider);

            // Assert
            Assert.AreEqual(6, _context.Language.Count()); // Check if all languages were added correctly
        }

        /// <summary>
        /// Tests the Initialize method of the LanguageSeeding class to ensure that no additional languages are seeded if they already exist.
        /// </summary>
        [TestMethod]
        public void Initialize_ShouldNotSeedLanguagesIfTheyAlreadyExist()
        {
            // Arrange
            _context.Language.AddRange(new List<Language>
            {
                new Language { Name = "German", Abbreviation = "de", IsTargetLanguage = true, IsOriginLanguage = false },
                new Language { Name = "English", Abbreviation = "en-US", IsTargetLanguage = true, IsOriginLanguage = false },
                new Language { Name = "Detect Language", Abbreviation = "DL", IsTargetLanguage = false, IsOriginLanguage = true },
                new Language { Name = "English", Abbreviation = "en", IsTargetLanguage = false, IsOriginLanguage = true }
            });
            _context.SaveChanges();

            // Act
            LanguageSeeding.Initialize(_serviceProvider);

            // Assert
            Assert.AreEqual(4, _context.Language.Count()); // Check if no additional languages were added
        }

        /// <summary>
        /// Tests the Initialize method of the LanguageSeeding class to ensure that missing languages are correctly seeded if they partially exist.
        /// </summary>
        [TestMethod]
        public void Initialize_ShouldSeedLanguagesIfTheyPartiallyExist()
        {
            // Arrange
            _context.Language.AddRange(new List<Language>
            {
                new Language { Name = "German", Abbreviation = "de", IsTargetLanguage = true, IsOriginLanguage = false },
                new Language { Name = "English", Abbreviation = "en-US", IsTargetLanguage = true, IsOriginLanguage = false }
            });
            _context.SaveChanges();

            // Act
            LanguageSeeding.Initialize(_serviceProvider);

            // Assert
            Assert.AreEqual(6, _context.Language.Count()); // Check if the missing languages were added
        }
    }
}
