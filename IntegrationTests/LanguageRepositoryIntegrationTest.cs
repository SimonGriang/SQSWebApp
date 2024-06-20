using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Tests
{
    [TestClass]
    [TestCategory("IntegrationTest")]
    public class LanuguageRepositoryIntegrationTest
    {
        private static WebAppContext _context = new WebAppContext(new DbContextOptionsBuilder<WebAppContext>().UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=mypassword").Options);
        private static ILanguageRepository _repository = new LanguageRepository(_context);

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            // Add test data to the database
            Language testOrigin = new Language { Name = "TestOrigin", Abbreviation = "testOrigin" };
            Language testTarget = new Language { Name = "TestTarget", Abbreviation = "testTarget" };

            _repository.AddLanguage(testOrigin);
            _repository.AddLanguage(testTarget);
        }

                [TestMethod]
        public void AddLanguage_ShouldAddLanguageToDatabase()
        {
            // Arrange
            var newLanguages = new List<Language>
            {
                new Language { ID = 1111, Name = "Portuguese", Abbreviation = "pt" },
                new Language { ID = 1112, Name = "Dutch", Abbreviation = "nl", isOriginLanguage = false },
                new Language { ID = 1113, Name = "Greek", isTargetLanguage = false },
                new Language { ID = 1114, Name = "Swedish", Abbreviation = "sv", isOriginLanguage = false, isTargetLanguage = false },
                new Language { ID = 1115, Name = "Danish" },
                new Language { ID = 1116, Name = "Finnish", Abbreviation = "fi", isOriginLanguage = true },
                new Language { ID = 1117, Name = "Norwegian", Abbreviation = "no", isTargetLanguage = true },
                new Language { ID = 1118, Name = "Polish", Abbreviation = "pl", isOriginLanguage = true, isTargetLanguage = true },
                new Language { ID = 1119, Name = "Hungarian", isOriginLanguage = true },
                new Language { ID = 1120, Name = "Czech", isTargetLanguage = true }
            };

            foreach (var newLanguage in newLanguages)
            {
                // Act
                _repository.AddLanguage(newLanguage);

                // Assert
                Assert.IsTrue(_repository.LanguageExists(newLanguage.ID));
            }
        }

        [TestMethod]
        public void RemoveLanguage_ShouldRemoveLanguageFromDatabase()
        {
            // Arrange
            var languageIdsToRemove = new List<int> { 1111, 1112, 1113, 1114, 1115, 1116, 1117, 1118, 1119, 1120 };
    
            foreach (var languageId in languageIdsToRemove)
            {
                // Act
                _repository.RemoveLanguage(languageId);
    
                // Assert
                Assert.IsFalse(_repository.LanguageExists(languageId));
            }
        }

        [TestMethod]
        public void GetLanguage_ShouldReturnLanguageIfExists()
        {
            // Arrange
            var testLanguage = _repository.returnLanguageByAbbreviation("testOrigin");
            var languageId = testLanguage
            !.ID;

            // Act
            var language = _repository.GetLanguage(languageId);

            // Assert
            Assert.IsNotNull(language);
            Assert.AreEqual(languageId, language!.ID);
        }

        [TestMethod]
        public void GetAllLanguages_ShouldReturnAllLanguages()
        {
            // Act
            var actualLanguages = _repository.GetAllLanguages();
        
            // Assert
            Assert.IsTrue(actualLanguages.Count > 1);
        }

        [TestMethod]
        public void LanguageExists_ShouldReturnTrueIfExists()
        {
            // Arrange
            var existingLanguageId = _context.Language.First().ID;

            // Act
            var languageExists = _repository.LanguageExists(existingLanguageId);

            // Assert
            Assert.IsTrue(languageExists);
        }

        [TestMethod]
        public void LanguageExists_ShouldReturnFalseIfNotExists()
        {
            // Arrange
            var nonExistingLanguageId = 0;

            // Act
            var languageExists = _repository.LanguageExists(nonExistingLanguageId);

            // Assert
            Assert.IsFalse(languageExists);
        }

        [TestMethod]
        public void LanguageExistsByAbbreviation_ShouldReturnTrueIfExists()
        {
            // Arrange
            var existingLanguageAbbreviation = "testOrigin";

            // Act
            var languageExists = _repository.LanguageExistsByAbbreviation(existingLanguageAbbreviation);

            // Assert
            Assert.IsTrue(languageExists);
        }

        [TestMethod]
        public void LanguageExistsByAbbreviation_ShouldReturnFalseIfNotExists()
        {
            // Arrange
            var nonExistingLanguageId = "xx";

            // Act
            var languageExists = _repository.LanguageExistsByAbbreviation(nonExistingLanguageId);

            // Assert
            Assert.IsFalse(languageExists);
        }

        [TestMethod]
        public void ReturnLanguageByAbbreviation_ShouldReturnCorrectLanguage()
        {
            // Act
            var result = _repository.returnLanguageByAbbreviation("testOrigin");

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetLanguage_ShouldReturnCorrectLanguage()
        {
            // Arrange
            var languageId = 1130;
            var expectedLanguage = new Language { ID = 1130, Abbreviation = "testOrigin", Name = "testOrigin" };

            // Act
            _repository.AddLanguage(expectedLanguage);
            var result = _repository.GetLanguage(languageId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLanguage.ID, result.ID);
            Assert.AreEqual(expectedLanguage.Abbreviation, result.Abbreviation);
            Assert.AreEqual(expectedLanguage.Name, result.Name);
        }

                [ClassCleanup]
        public static void Cleanup()
        {
            // Clean up test data from the database
            _context.Language.RemoveRange(_context.Language);
            _context.SaveChanges();
        }
    }
}