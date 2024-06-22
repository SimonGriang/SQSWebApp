
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.IntegrationTests
{
    [TestClass]
    [TestCategory("IntegrationTests")]
    public class TranslationRepositoryIntegrationTest
    {
        private static WebAppContext _context = new WebAppContext(new DbContextOptionsBuilder<WebAppContext>().UseNpgsql("Host=localhost;Port=5431;Database=postgresIntegration;Username=postgres;Password=mypassword").Options);
        private static ILanguageRepository _LanguageRepository = new LanguageRepository(_context);
        private static ITranslationRepository _TranslationRepository = new TranslationRepository(_context);

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            // Add test data to the database
            Language testOrigin = new Language { Name = "TestOrigin", Abbreviation = "testOrigin" };
            Language testTarget = new Language { Name = "TestTarget", Abbreviation = "testTarget" };

            _LanguageRepository.AddLanguage(testOrigin);
            _LanguageRepository.AddLanguage(testTarget);

            // Add test data to the database
            _TranslationRepository.AddTranslation(new Translation { 
                OriginalText = "This is a test",
                TranslatedText = "Das ist ein Test",
                OriginalLanguage = testOrigin,
                TranslatedLanguage = testTarget,
                translated_at = DateTime.UtcNow
            });
        }

        [TestMethod]
        public void AddTranslation_ShouldAddTranslationToDatabase()
        {
            // Arrange
            Language testOrigin = new Language { Name = "TestOriginAddTranslationTest", Abbreviation = "testOriginAddTransl" };
            Language testTarget = new Language { Name = "TestTargetAddTranslationTest", Abbreviation = "testTargetAddTransl" };

            _LanguageRepository.AddLanguage(testOrigin);
            _LanguageRepository.AddLanguage(testTarget);

            // Add test data to the database
            Translation translation = new Translation { 
                OriginalText = "This is a test to see if the translation is added to the database",
                TranslatedText = "Das ist ein Test um zu sehen ob die Übersetzung in die Datenbank hinzugefügt wird",
                OriginalLanguage = testOrigin,
                TranslatedLanguage = testTarget,
                translated_at = DateTime.UtcNow
            };
            // Act
            _TranslationRepository.AddTranslation(translation);

            // Assert
            Assert.IsTrue(_context.Translation.Contains(translation));
        }

        [TestMethod]
        public void GetAllTranslations_ShouldReturnAllTranslations()
        {
            // Act
            var translations = _TranslationRepository.GetAllTranslations();

            // Assert
            Assert.IsTrue(translations.Count > 0);
        }

        [TestMethod]
        public void TranslationExists_ShouldReturnTrueForExistingTranslation()
        {
            // Arrange
            var existingTranslationId = _context.Translation.First().ID;

            // Act
            var exists = _TranslationRepository.TranslationExists(existingTranslationId);

            // Assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void GetTranslationById_ShouldReturnCorrectTranslation()
        {
            // Arrange
            var existingTranslation = _context.Translation.First();

            // Act
            var translation = _TranslationRepository.GetTranslationById(existingTranslation.ID);

            // Assert
            Assert.AreEqual(existingTranslation, translation);
        }

        [TestMethod]
        public void DeleteTranslation_ShouldRemoveTranslationFromDatabase()
        {
            // Arrange
            var existingTranslationId = _context.Translation.First().ID;

            // Act
            _TranslationRepository.DeleteTranslation(existingTranslationId);

            // Assert
            Assert.IsFalse(_context.Translation.Any(t => t.ID == existingTranslationId));
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            // Clean up test data from the database
            _context.Language.RemoveRange(_context.Language);
            _context.Translation.RemoveRange(_context.Translation);
            _context.SaveChanges();
        }
    }
}