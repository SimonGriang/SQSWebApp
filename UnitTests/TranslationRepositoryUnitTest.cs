using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Tests
{
    [TestClass]
    public class TranslationTests
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
                context.Translation.AddRange(new List<Translation>
                {
                    new Translation { ID = 1, OriginalText = "Hello", TranslatedText = "Hallo", translated_at = DateTime.Now },
                    new Translation { ID = 2, OriginalText = "Goodbye", TranslatedText = "Auf Wiedersehen", translated_at = DateTime.Now },
                    // Add more translations as needed
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
        public void AddTranslation_ShouldAddTranslationToDatabase()
        {
            using (var context = new WebAppContext(_options))
            {
                var repository = new TranslationRepository(context);
                var newTranslation = new Translation { ID = 3, OriginalText = "Yes", TranslatedText = "Ja", translated_at = DateTime.Now };

                repository.AddTranslation(newTranslation);
                Assert.IsTrue(repository.TranslationExists(newTranslation.ID));
            }
        }

        [TestMethod]
        public void RemoveTranslation_ShouldRemoveTranslationFromDatabase()
        {
            using (var context = new WebAppContext(_options))
            {
                var repository = new TranslationRepository(context);
                var translationIdToRemove = 1;

                repository.RemoveTranslation(translationIdToRemove);
                Assert.IsFalse(repository.TranslationExists(translationIdToRemove));
            }
        }

        [TestMethod]
        public void GetTranslation_ShouldReturnTranslationIfExists()
        {
            using (var context = new WebAppContext(_options))
            {
                var repository = new TranslationRepository(context);
                var translationId = 2;

                var translation = repository.GetTranslation(translationId);
                Assert.IsNotNull(translation);
                Assert.AreEqual(translationId, translation!.ID);
            }
        }

        [TestMethod]
        public void GetAllTranslations_ShouldReturnAllTranslations()
        {
            using (var context = new WebAppContext(_options))
            {
                var repository = new TranslationRepository(context);
                var expectedTranslations = new List<Translation>
                {
                    new Translation { ID = 1, OriginalText = "Hello", TranslatedText = "Hallo", translated_at = DateTime.Now },
                    new Translation { ID = 2, OriginalText = "Goodbye", TranslatedText = "Auf Wiedersehen", translated_at = DateTime.Now },
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