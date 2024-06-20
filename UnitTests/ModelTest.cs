using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Models;

namespace WebApp.Tests
{
    [TestClass]
    public class LanguageTranslationTests
    {
        [TestMethod]
        public void LanguageTestDefaultConstructor()
        {
            var language = new Language();
            Assert.AreEqual(0, language.ID);
            Assert.AreEqual(string.Empty, language.Name);
            Assert.IsNull(language.Abbreviation);
            Assert.IsFalse(language.isTargetLanguage);
            Assert.IsFalse(language.isOriginLanguage);
        }

        [TestMethod]
        public void LanguageTestParameterizedConstructor()
        {
            var language = new Language("English", "EN");
            Assert.AreEqual("English", language.Name);
            Assert.AreEqual("EN", language.Abbreviation);
        }

        [TestMethod]
        public void LanguageTestSetAndGetID()
        {
            var language = new Language();
            language.ID = 1;
            Assert.AreEqual(1, language.ID);
        }

        [TestMethod]
        public void LanguageTestSetAndGetName()
        {
            var language = new Language();
            language.Name = "Spanish";
            Assert.AreEqual("Spanish", language.Name);
        }

        [TestMethod]
        public void LanguageTestSetAndGetAbbreviation()
        {
            var language = new Language();
            language.Abbreviation = "ES";
            Assert.AreEqual("ES", language.Abbreviation);
        }

        [TestMethod]
        public void LanguageTestSetAndGetIsTargetLanguage()
        {
            var language = new Language();
            language.isTargetLanguage = true;
            Assert.IsTrue(language.isTargetLanguage);
        }

        [TestMethod]
        public void LanguageTestSetAndGetIsOriginLanguage()
        {
            var language = new Language();
            language.isOriginLanguage = true;
            Assert.IsTrue(language.isOriginLanguage);
        }

        [TestMethod]
        public void TranslationTestSetAndGetID()
        {
            var translation = new Translation();
            translation.ID = 1;
            Assert.AreEqual(1, translation.ID);
        }

        [TestMethod]
        public void TranslationTestSetAndGetOriginalText()
        {
            var translation = new Translation();
            translation.OriginalText = "Hello";
            Assert.AreEqual("Hello", translation.OriginalText);
        }

        [TestMethod]
        public void TranslationTestSetAndGetTranslatedText()
        {
            var translation = new Translation();
            translation.TranslatedText = "Hola";
            Assert.AreEqual("Hola", translation.TranslatedText);
        }

        [TestMethod]
        public void TranslationTestSetAndGetTranslatedAt()
        {
            var translation = new Translation();
            var date = DateTime.Now;
            translation.translated_at = date;
            Assert.AreEqual(date, translation.translated_at);
        }

        [TestMethod]
        public void TranslationTestSetAndGetOriginalLanguage()
        {
            var translation = new Translation();
            var language = new Language("English", "EN");
            translation.OriginalLanguage = language;
            Assert.AreEqual(language, translation.OriginalLanguage);
        }

        [TestMethod]
        public void TranslationTestSetAndGetTranslatedLanguage()
        {
            var translation = new Translation();
            var language = new Language("Spanish", "ES");
            translation.TranslatedLanguage = language;
            Assert.AreEqual(language, translation.TranslatedLanguage);
        }

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