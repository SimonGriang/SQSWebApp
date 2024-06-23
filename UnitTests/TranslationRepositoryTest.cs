using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Tests
{
    /// <summary>
    /// Represents a test class for the TranslationRepository class.
    /// </summary>
    [TestClass]
    [TestCategory("UnitTests")]
    public class TranslationRepositoryTests
    {
        private DbContextOptions<WebAppContext> _options;

        /// <summary>
        /// Initializes the test environment by setting up an in-memory database and adding test data.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _options = new DbContextOptionsBuilder<WebAppContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new WebAppContext(_options))
            {
                context.Translation.AddRange(new List<Translation>
                {
                    new Translation { ID = 1, OriginalText = "Hello", TranslatedText = "Hallo", Translated_at = DateTime.Now },
                    new Translation { ID = 2, OriginalText = "Goodbye", TranslatedText = "Auf Wiedersehen", Translated_at = DateTime.Now },
                    // Add more translations as needed
                });
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Cleans up the test environment by deleting the in-memory database.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            using (var context = new WebAppContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }

        /// <summary>
        /// Tests the AddTranslation method of the TranslationRepository class.
        /// It verifies that a translation is added to the database.
        /// </summary>
        [TestMethod]
        public void AddTranslation_ShouldAddTranslationToDatabase()
        {
            using (var context = new WebAppContext(_options))
            {
                var repository = new TranslationRepository(context);
                var newTranslation = new Translation { ID = 3, OriginalText = "Yes", TranslatedText = "Ja", Translated_at = DateTime.Now };

                repository.AddTranslation(newTranslation);
                Assert.IsTrue(repository.TranslationExists(newTranslation.ID));
            }
        }

        /// <summary>
        /// Tests the RemoveTranslation method of the TranslationRepository class.
        /// It verifies that a translation is removed from the database.
        /// </summary>
        [TestMethod]
        public void RemoveTranslation_ShouldRemoveTranslationFromDatabase()
        {
            using (var context = new WebAppContext(_options))
            {
                var repository = new TranslationRepository(context);
                var translationIdToRemove = 1;

                repository.DeleteTranslation(translationIdToRemove);
                Assert.IsFalse(repository.TranslationExists(translationIdToRemove));
            }
        }

        /// <summary>
        /// Tests the GetTranslationById method of the TranslationRepository class.
        /// It verifies that a translation is returned if it exists in the database.
        /// </summary>
        [TestMethod]
        public void GetTranslation_ShouldReturnTranslationIfExists()
        {
            using (var context = new WebAppContext(_options))
            {
                var repository = new TranslationRepository(context);
                var translationId = 2;

                var translation = repository.GetTranslationById(translationId);
                Assert.IsNotNull(translation);
                Assert.AreEqual(translationId, translation!.ID);
            }
        }

        /// <summary>
        /// Tests the GetAllTranslations method of the TranslationRepository class.
        /// It verifies that all translations are returned from the database.
        /// </summary>
        [TestMethod]
        public void GetAllTranslations_ShouldReturnAllTranslations()
        {
            using (var context = new WebAppContext(_options))
            {
                var repository = new TranslationRepository(context);
                var expectedTranslations = new List<Translation>
                {
                    new Translation { ID = 1, OriginalText = "Hello", TranslatedText = "Hallo", Translated_at = DateTime.Now },
                    new Translation { ID = 2, OriginalText = "Goodbye", TranslatedText = "Auf Wiedersehen", Translated_at = DateTime.Now },
                    // Add more translations as needed
                };

                var actualTranslations = repository.GetAllTranslations();
                Assert.AreEqual(expectedTranslations.Count, actualTranslations.Count);

                for (int i = 0; i < expectedTranslations.Count; i++)
                {
                    Assert.AreEqual(expectedTranslations[i].ID, actualTranslations[i].ID);
                    Assert.AreEqual(expectedTranslations[i].OriginalText, actualTranslations[i].OriginalText);
                    Assert.AreEqual(expectedTranslations[i].TranslatedText, actualTranslations[i].TranslatedText);
                }
            }
        }

        /// <summary>
        /// Tests the TranslationExists method of the TranslationRepository class.
        /// It verifies that the method returns true if a translation exists in the database.
        /// </summary>
        [TestMethod]
        public void TranslationExists_ShouldReturnTrueIfExists()
        {
            using (var context = new WebAppContext(_options))
            {
                var repository = new TranslationRepository(context);
                var existingTranslationId = 1;

                var translationExists = repository.TranslationExists(existingTranslationId);
                Assert.IsTrue(translationExists);
            }
        }

        /// <summary>
        /// Tests the TranslationExists method of the TranslationRepository class.
        /// It verifies that the method returns false if a translation does not exist in the database.
        /// </summary>
        [TestMethod]
        public void TranslationExists_ShouldReturnFalseIfNotExists()
        {
            using (var context = new WebAppContext(_options))
            {
                var repository = new TranslationRepository(context);
                var nonExistingTranslationId = 99;

                var translationExists = repository.TranslationExists(nonExistingTranslationId);
                Assert.IsFalse(translationExists);
            }
        }
    }
}