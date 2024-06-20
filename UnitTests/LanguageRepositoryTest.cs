using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Tests
{
    [TestClass]
    public class LanguageRepositoryTests
    {
        private DbContextOptions<WebAppContext> _options;

        [TestInitialize]
        public void Initialize()
        {
            _options = new DbContextOptionsBuilder<WebAppContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new WebAppContext(_options))
            {
                context.Language.AddRange(new List<Language>
                {
                    new Language { ID = 1, Name = "German", Abbreviation = "de" },
                    new Language { ID = 2, Name = "English", Abbreviation = "en", isOriginLanguage = false },
                    new Language { ID = 3, Name = "Spanish", isTargetLanguage = false },
                    new Language { ID = 4, Name = "French", Abbreviation = "fr", isOriginLanguage = false, isTargetLanguage = false },
                    new Language { ID = 5, Name = "Italian" },
                    new Language { ID = 6, Name = "Russian", Abbreviation = "ru", isOriginLanguage = true },
                    new Language { ID = 7, Name = "Chinese", Abbreviation = "zh", isTargetLanguage = true },
                    new Language { ID = 8, Name = "Japanese", Abbreviation = "ja", isOriginLanguage = true, isTargetLanguage = true },
                    new Language { ID = 9, Name = "Korean", isOriginLanguage = true },
                    new Language { ID = 10, Name = "Arabic", isTargetLanguage = true }
                });
                context.SaveChanges();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var context = new WebAppContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public void AddLanguage_ShouldAddLanguageToDatabase()
        {
            // Arrange
            using (var context = new WebAppContext(_options))
            {
                var repository = new LanguageRepository(context);
                var newLanguages = new List<Language>
                {
                    new Language { ID = 11, Name = "Portuguese", Abbreviation = "pt" },
                    new Language { ID = 12, Name = "Dutch", Abbreviation = "nl", isOriginLanguage = false },
                    new Language { ID = 13, Name = "Greek", isTargetLanguage = false },
                    new Language { ID = 14, Name = "Swedish", Abbreviation = "sv", isOriginLanguage = false, isTargetLanguage = false },
                    new Language { ID = 15, Name = "Danish" },
                    new Language { ID = 16, Name = "Finnish", Abbreviation = "fi", isOriginLanguage = true },
                    new Language { ID = 17, Name = "Norwegian", Abbreviation = "no", isTargetLanguage = true },
                    new Language { ID = 18, Name = "Polish", Abbreviation = "pl", isOriginLanguage = true, isTargetLanguage = true },
                    new Language { ID = 19, Name = "Hungarian", isOriginLanguage = true },
                    new Language { ID = 20, Name = "Czech", isTargetLanguage = true }
                };

                foreach (var newLanguage in newLanguages)
                {
                    // Act
                    repository.AddLanguage(newLanguage);

                    // Assert
                    Assert.IsTrue(repository.LanguageExists(newLanguage.ID));
                }
            }
        }

        [TestMethod]
        public void RemoveLanguage_ShouldRemoveLanguageFromDatabase()
        {
            // Arrange
            using (var context = new WebAppContext(_options))
            {
                var repository = new LanguageRepository(context);
                var languageIdsToRemove = new List<int> { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
        
                foreach (var languageId in languageIdsToRemove)
                {
                    // Act
                    repository.RemoveLanguage(languageId);
        
                    // Assert
                    Assert.IsFalse(repository.LanguageExists(languageId));
                }
            }
        }

        [TestMethod]
        public void GetLanguage_ShouldReturnLanguageIfExists()
        {
            // Arrange
            using (var context = new WebAppContext(_options))
            {
                var repository = new LanguageRepository(context);
                var languageId = 2;

                // Act
                var language = repository.GetLanguage(languageId);

                // Assert
                Assert.IsNotNull(language);
                Assert.AreEqual(languageId, language!.ID);
            }
        }

        [TestMethod]
        public void GetAllLanguages_ShouldReturnAllLanguages()
        {
            // Arrange
            using (var context = new WebAppContext(_options))
            {
                var repository = new LanguageRepository(context);
                var expectedLanguages = new List<Language>
                {
                    new Language { ID = 1, Name = "German", Abbreviation = "de" },
                    new Language { ID = 2, Name = "English", Abbreviation = "en", isOriginLanguage = false },
                    new Language { ID = 3, Name = "Spanish", isTargetLanguage = false },
                    new Language { ID = 4, Name = "French", Abbreviation = "fr", isOriginLanguage = false, isTargetLanguage = false },
                    new Language { ID = 5, Name = "Italian" },
                    new Language { ID = 6, Name = "Russian", Abbreviation = "ru", isOriginLanguage = true },
                    new Language { ID = 7, Name = "Chinese", Abbreviation = "zh", isTargetLanguage = true },
                    new Language { ID = 8, Name = "Japanese", Abbreviation = "ja", isOriginLanguage = true, isTargetLanguage = true },
                    new Language { ID = 9, Name = "Korean", isOriginLanguage = true },
                    new Language { ID = 10, Name = "Arabic", isTargetLanguage = true }
                };
        
                // Act
                var actualLanguages = repository.GetAllLanguages();
        
                // Assert
                Assert.AreEqual(expectedLanguages.Count, actualLanguages.Count);
        
                for (int i = 0; i < expectedLanguages.Count; i++)
                {
                    Assert.AreEqual(expectedLanguages[i].ID, actualLanguages[i].ID);
                    Assert.AreEqual(expectedLanguages[i].Name, actualLanguages[i].Name);
                    Assert.AreEqual(expectedLanguages[i].Abbreviation, actualLanguages[i].Abbreviation);
                    Assert.AreEqual(expectedLanguages[i].isOriginLanguage, actualLanguages[i].isOriginLanguage);
                    Assert.AreEqual(expectedLanguages[i].isTargetLanguage, actualLanguages[i].isTargetLanguage);
                }
            }
        }

        [TestMethod]
        public void LanguageExists_ShouldReturnTrueIfExists()
        {
            // Arrange
            using (var context = new WebAppContext(_options))
            {
                var repository = new LanguageRepository(context);
                var existingLanguageId = 1;

                // Act
                var languageExists = repository.LanguageExists(existingLanguageId);

                // Assert
                Assert.IsTrue(languageExists);
            }
        }

        [TestMethod]
        public void LanguageExists_ShouldReturnFalseIfNotExists()
        {
            // Arrange
            using (var context = new WebAppContext(_options))
            {
                var repository = new LanguageRepository(context);
                var nonExistingLanguageId = 99;

                // Act
                var languageExists = repository.LanguageExists(nonExistingLanguageId);

                // Assert
                Assert.IsFalse(languageExists);
            }
        }

        [TestMethod]
        public void LanguageExistsByAbbreviation_ShouldReturnTrueIfExists()
        {
            // Arrange
            using (var context = new WebAppContext(_options))
            {
                var repository = new LanguageRepository(context);
                var existingLanguageId = "de";

                // Act
                var languageExists = repository.LanguageExistsByAbbreviation(existingLanguageId);

                // Assert
                Assert.IsTrue(languageExists);
            }
        }

        [TestMethod]
        public void LanguageExistsByAbbreviation_ShouldReturnFalseIfNotExists()
        {
            // Arrange
            using (var context = new WebAppContext(_options))
            {
                var repository = new LanguageRepository(context);
                var nonExistingLanguageId = "xx";

                // Act
                var languageExists = repository.LanguageExistsByAbbreviation(nonExistingLanguageId);

                // Assert
                Assert.IsFalse(languageExists);
            }
        }
    }
}
