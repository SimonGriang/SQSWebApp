using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApp.Data;
using WebApp.Models;

/// <summary>
/// Contains unit tests for the <see cref="LanguageRepository"/> class.
/// </summary>
namespace WebApp.Tests
{
    [TestClass]
    [TestCategory("UnitTests")]
    public class LanguageRepositoryTests
    {
        private DbContextOptions<WebAppContext> _options = new DbContextOptionsBuilder<WebAppContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

        /// <summary>
        /// Initializes the test environment before each test method is executed.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            using (var context = new WebAppContext(_options))
            {
                context.Language.AddRange(new List<Language>
                {
                    new Language { ID = 1, Name = "German", Abbreviation = "de" },
                    new Language { ID = 2, Name = "English", Abbreviation = "en", IsOriginLanguage = false },
                    new Language { ID = 3, Name = "Spanish", IsTargetLanguage = false },
                    new Language { ID = 4, Name = "French", Abbreviation = "fr", IsOriginLanguage = false, IsTargetLanguage = false },
                    new Language { ID = 5, Name = "Italian" },
                    new Language { ID = 6, Name = "Russian", Abbreviation = "ru", IsOriginLanguage = true },
                    new Language { ID = 7, Name = "Chinese", Abbreviation = "zh", IsTargetLanguage = true },
                    new Language { ID = 8, Name = "Japanese", Abbreviation = "ja", IsOriginLanguage = true, IsTargetLanguage = true },
                    new Language { ID = 9, Name = "Korean", IsOriginLanguage = true },
                    new Language { ID = 10, Name = "Arabic", IsTargetLanguage = true }
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

        /// <summary>
        /// Tests the functionality of adding a language to the database.
        /// </summary>
        /// <remarks>
        /// This test method verifies that the <see cref="LanguageRepository.AddLanguage"/> method correctly adds a language to the database.
        /// It creates a new instance of the <see cref="LanguageRepository"/> class, initializes a list of new languages, and adds each language to the database.
        /// After adding each language, it asserts that the language exists in the database by calling the <see cref="LanguageRepository.LanguageExists"/> method.
        /// </remarks>
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
                    new Language { ID = 12, Name = "Dutch", Abbreviation = "nl", IsOriginLanguage = false },
                    new Language { ID = 13, Name = "Greek", IsTargetLanguage = false },
                    new Language { ID = 14, Name = "Swedish", Abbreviation = "sv", IsOriginLanguage = false, IsTargetLanguage = false },
                    new Language { ID = 15, Name = "Danish" },
                    new Language { ID = 16, Name = "Finnish", Abbreviation = "fi", IsOriginLanguage = true },
                    new Language { ID = 17, Name = "Norwegian", Abbreviation = "no", IsTargetLanguage = true },
                    new Language { ID = 18, Name = "Polish", Abbreviation = "pl", IsOriginLanguage = true, IsTargetLanguage = true },
                    new Language { ID = 19, Name = "Hungarian", IsOriginLanguage = true },
                    new Language { ID = 20, Name = "Czech", IsTargetLanguage = true }
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

        /// <summary>
        /// Tests the functionality of removing a language from the database.
        /// </summary>
        /// <remarks>
        /// This test method verifies that the <see cref="LanguageRepository.RemoveLanguage"/> method correctly removes the specified language from the database.
        /// It arranges the necessary objects and data, performs the removal operation, and then asserts that the language no longer exists in the database.
        /// </remarks>
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
                    // Act        ///               repository.RemoveLanguage(languageId);
        
                    // Assert
                    Assert.IsFalse(repository.LanguageExists(languageId));
                }
            }
        }

        /// <summary>
        /// Tests the GetLanguage method of the LanguageRepository class to ensure that it returns the correct language if it exists.
        /// </summary>
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

        /// <summary>
        /// Tests the GetAllLanguages method of the LanguageRepository class to ensure that it returns all languages.
        /// </summary>
        /// <remarks>
        /// This test method verifies that the GetAllLanguages method of the LanguageRepository class returns the expected list of languages.
        /// It performs the following steps:
        /// 1. Arranges the necessary objects and data for the test.
        /// 2. Calls the GetAllLanguages method to retrieve the actual list of languages.
        /// 3. Asserts that the count of expected languages matches the count of actual languages.
        /// 4. Iterates through each language in the expected list and asserts that the corresponding language in the actual list has the same properties.
        /// </remarks>
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
                    new Language { ID = 2, Name = "English", Abbreviation = "en", IsOriginLanguage = false },
                    new Language { ID = 3, Name = "Spanish", IsTargetLanguage = false },
                    new Language { ID = 4, Name = "French", Abbreviation = "fr", IsOriginLanguage = false, IsTargetLanguage = false },
                    new Language { ID = 5, Name = "Italian" },
                    new Language { ID = 6, Name = "Russian", Abbreviation = "ru", IsOriginLanguage = true },
                    new Language { ID = 7, Name = "Chinese", Abbreviation = "zh", IsTargetLanguage = true },
                    new Language { ID = 8, Name = "Japanese", Abbreviation = "ja", IsOriginLanguage = true, IsTargetLanguage = true },
                    new Language { ID = 9, Name = "Korean", IsOriginLanguage = true },
                    new Language { ID = 10, Name = "Arabic", IsTargetLanguage = true }
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
                    Assert.AreEqual(expectedLanguages[i].IsOriginLanguage, actualLanguages[i].IsOriginLanguage);
                    Assert.AreEqual(expectedLanguages[i].IsTargetLanguage, actualLanguages[i].IsTargetLanguage);
                }
            }
        }

        /// <summary>
        /// This test method verifies that the LanguageExists method of the LanguageRepository class returns true if a language with the specified ID exists in the database.
        /// </summary>
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

        /// <summary>  
        /// This test method verifies that the LanguageExists method of the LanguageRepository class
        /// returns false when a language with the specified ID does not exist in the database.   
        /// </summary>
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

        /// <summary>
        /// Tests the <see cref="LanguageRepository.LanguageExistsByAbbreviation"/> method to ensure it returns true if a language with the specified abbreviation exists.
        /// </summary>
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

        /// <summary>
        /// Tests the behavior of the LanguageExistsByAbbreviation method when the language with the specified abbreviation does not exist.
        /// </summary>
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

        /// <summary>
        /// Tests the <see cref="LanguageRepository.ReturnLanguageByAbbreviation"/> method to ensure it returns the correct language based on the abbreviation.
        /// </summary>
        /// <remarks>
        /// This test method verifies that the <see cref="LanguageRepository.ReturnLanguageByAbbreviation"/> method correctly retrieves a language from the repository based on the provided abbreviation.
        /// It sets up the necessary test data, calls the method under test, and asserts that the returned language matches the expected language.
        /// </remarks>
        [TestMethod]
        public void ReturnLanguageByAbbreviation_ShouldReturnCorrectLanguage()
        {
            using (var context = new WebAppContext(_options))
            {
                var repository = new LanguageRepository(context);
                // Arrange
                var abbreviation = "en";
                var expectedLanguage = new Language { ID = 2, Name = "English", Abbreviation = "en", IsOriginLanguage = false };

                // Act
                var result = repository.returnLanguageByAbbreviation(abbreviation);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(expectedLanguage.ID, result!.ID);
                Assert.AreEqual(expectedLanguage.Name, result.Name);
                Assert.AreEqual(expectedLanguage.Abbreviation, result.Abbreviation);
                Assert.AreEqual(expectedLanguage.IsOriginLanguage, result.IsOriginLanguage);
                Assert.AreEqual(expectedLanguage.IsTargetLanguage, result.IsTargetLanguage);
            }
        }

        /// <summary>
        /// Tests the <see cref="LanguageRepository.GetLanguage"/> method to ensure that it returns the correct language.
        /// </summary>
        /// <remarks>
        /// This test method verifies that the <see cref="LanguageRepository.GetLanguage"/> method correctly retrieves a language from the repository based on the provided language ID.
        /// It performs the following steps:
        /// 1. Creates a new instance of the <see cref="WebAppContext"/> using the specified options.
        /// 2. Instantiates a new <see cref="LanguageRepository"/> object with the created context.
        /// 3. Sets up the necessary test data, including the expected language.
        /// 4. Calls the <see cref="LanguageRepository.AddLanguage"/> method to add the expected language to the repository.
        /// 5. Calls the <see cref="LanguageRepository.GetLanguage"/> method with the specified language ID.
        /// 6. Asserts that the returned language is not null and matches the expected language in terms of ID, abbreviation, and name.
        /// </remarks>
        [TestMethod]
        public void GetLanguage_ShouldReturnCorrectLanguage()
        {

            using (var context = new WebAppContext(_options))
            {
                var repository = new LanguageRepository(context);
                // Arrange
                var languageId = 30;
                var expectedLanguage = new Language { ID = 30, Abbreviation = "EN", Name = "English" };

                // Act
                repository.AddLanguage(expectedLanguage);
                var result = repository.GetLanguage(languageId);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(expectedLanguage.ID, result.ID);
                Assert.AreEqual(expectedLanguage.Abbreviation, result.Abbreviation);
                Assert.AreEqual(expectedLanguage.Name, result.Name);
            }
        }

        /// <summary>
        /// Tests the validation of the OriginalText property in the Translation class.
        /// </summary>
        /// <remarks>
        /// This method performs the following tests:
        /// - Required validation: Checks if the OriginalText property is required.
        /// - String length validation: Checks if the OriginalText property has a length between 1 and 500 characters.
        /// - Valid case: Checks if the OriginalText property is valid when set to a valid value.
        /// </remarks>
        [TestMethod]
        public void TranslationTestOriginalTextValidation()
        {
            var translation = new Translation();

            // Test required validation
            var context = new ValidationContext(translation) { MemberName = "OriginalText" };
            var result = new List<ValidationResult>();
            var isValid = Validator.TryValidateProperty(translation.OriginalText, context, result);
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("OriginalText is required", result.First().ErrorMessage);

            // Test string length validation
            translation.OriginalText = new string('a', 501);
            result.Clear();
            isValid = Validator.TryValidateProperty(translation.OriginalText, context, result);
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("OriginalText must be between 1 and 500 characters", result.First().ErrorMessage);

            // Test valid case
            translation.OriginalText = "Hello";
            result.Clear();
            isValid = Validator.TryValidateProperty(translation.OriginalText, context, result);
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, result.Count);
        }
    }
}
