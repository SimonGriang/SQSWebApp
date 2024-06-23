using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Models;

namespace WebApp.Tests
{
    /// <summary>
    /// Contains unit tests for the LanguageTranslation class.
    /// </summary>
    [TestClass]
    [TestCategory("UnitTests")]
    public class LanguageTranslationTests
    {
        /// <summary>
        /// Tests the default constructor of the Language class.
        /// </summary>
        [TestMethod]
        public void LanguageTestDefaultConstructor()
        {
            var language = new Language();
            Assert.AreEqual(0, language.ID);
            Assert.AreEqual(string.Empty, language.Name);
            Assert.IsNull(language.Abbreviation);
            Assert.IsFalse(language.IsTargetLanguage);
            Assert.IsFalse(language.IsOriginLanguage);
        }

        /// <summary>
        /// Tests the parameterized constructor of the Language class.
        /// </summary>
        [TestMethod]
        public void LanguageTestParameterizedConstructor()
        {
            var language = new Language("English", "EN");
            Assert.AreEqual("English", language.Name);
            Assert.AreEqual("EN", language.Abbreviation);
        }

        /// <summary>
        /// Tests the ID property of the Language class.
        /// </summary>
        [TestMethod]
        public void LanguageTestSetAndGetID()
        {
            var language = new Language();
            language.ID = 1;
            Assert.AreEqual(1, language.ID);
        }

        /// <summary>
        /// Tests the Name property of the Language class.
        /// </summary>
        [TestMethod]
        public void LanguageTestSetAndGetName()
        {
            var language = new Language();
            language.Name = "Spanish";
            Assert.AreEqual("Spanish", language.Name);
        }

        /// <summary>
        /// Tests the Abbreviation property of the Language class.
        /// </summary>
        [TestMethod]
        public void LanguageTestSetAndGetAbbreviation()
        {
            var language = new Language();
            language.Abbreviation = "ES";
            Assert.AreEqual("ES", language.Abbreviation);
        }

        /// <summary>
        /// Tests the IsTargetLanguage property of the Language class.
        /// </summary>
        [TestMethod]
        public void LanguageTestSetAndGetIsTargetLanguage()
        {
            var language = new Language();
            language.IsTargetLanguage = true;
            Assert.IsTrue(language.IsTargetLanguage);
        }

        /// <summary>
        /// Tests the IsOriginLanguage property of the Language class.
        /// </summary>
        [TestMethod]
        public void LanguageTestSetAndGetIsOriginLanguage()
        {
            var language = new Language();
            language.IsOriginLanguage = true;
            Assert.IsTrue(language.IsOriginLanguage);
        }

        /// <summary>
        /// Tests the ID property of the Translation class.
        /// </summary>
        [TestMethod]
        public void TranslationTestSetAndGetID()
        {
            var translation = new Translation();
            translation.ID = 1;
            Assert.AreEqual(1, translation.ID);
        }

        /// <summary>
        /// Tests the OriginalText property of the Translation class.
        /// </summary>
        [TestMethod]
        public void TranslationTestSetAndGetOriginalText()
        {
            var translation = new Translation();
            translation.OriginalText = "Hello";
            Assert.AreEqual("Hello", translation.OriginalText);
        }

        /// <summary>
        /// Tests the TranslatedText property of the Translation class.
        /// </summary>
        [TestMethod]
        public void TranslationTestSetAndGetTranslatedText()
        {
            var translation = new Translation();
            translation.TranslatedText = "Hola";
            Assert.AreEqual("Hola", translation.TranslatedText);
        }

        /// <summary>
        /// Tests the Translated_at property of the Translation class.
        /// </summary>
        [TestMethod]
        public void TranslationTestSetAndGetTranslatedAt()
        {
            var translation = new Translation();
            var date = DateTime.Now;
            translation.Translated_at = date;
            Assert.AreEqual(date, translation.Translated_at);
        }

        /// <summary>
        /// Tests the OriginalLanguage property of the Translation class.
        /// </summary>
        [TestMethod]
        public void TranslationTestSetAndGetOriginalLanguage()
        {
            var translation = new Translation();
            var language = new Language("English", "EN");
            translation.OriginalLanguage = language;
            Assert.AreEqual(language, translation.OriginalLanguage);
        }

        /// <summary>
        /// Tests the TranslatedLanguage property of the Translation class.
        /// </summary>
        [TestMethod]
        public void TranslationTestSetAndGetTranslatedLanguage()
        {
            var translation = new Translation();
            var language = new Language("Spanish", "ES");
            translation.TranslatedLanguage = language;
            Assert.AreEqual(language, translation.TranslatedLanguage);
        }

        /// <summary>
        /// Tests the OriginalText property validation of the Translation class.
        /// </summary>
        /// [TestMethod]
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