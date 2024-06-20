using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Tests
{
    [TestClass]
    [TestCategory("UnitTests")]
    public class CreateTranslationViewModelTests
    {
        private CreateTranslationViewModel viewModel = new CreateTranslationViewModel{
            targetLanguages = new List<Language>{
                new Language { ID = 1, Abbreviation = "de", isTargetLanguage = true, isOriginLanguage = false },
                new Language { ID = 2, Abbreviation = "en-US", isTargetLanguage = true, isOriginLanguage = false },
                new Language { ID = 3, Abbreviation = "en-GB", isTargetLanguage = true, isOriginLanguage = false }
                },
            originLanguages= new List<Language>{
                new Language { ID = 4, Abbreviation = "DL", isTargetLanguage = false, isOriginLanguage = true },
                new Language { ID = 5, Abbreviation = "en", isTargetLanguage = false, isOriginLanguage = true }
                },
            English = 1,
            EnglishUS = 2,
            EnglishGB = 3,
            German = 4,
            DetectLanguage = 5
        };


        [TestMethod]
        public void ValidateLanguageTo_ValidLanguage_ReturnsSuccess()
        {
            viewModel.LanguageTo = 1;
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(viewModel) { MemberName = nameof(viewModel.LanguageTo) };
            bool isValid = Validator.TryValidateProperty(viewModel.LanguageTo, validationContext, validationResults);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, validationResults.Count);
        }

        [TestMethod]
        public void ValidateLanguageTo_InvalidLanguage_ReturnsError()
        {
            viewModel.LanguageTo = -1;
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(viewModel);
            var result = viewModel.ValidateLanguageTo(validationContext);

            Assert.AreNotEqual(ValidationResult.Success, result);
            Assert.AreEqual("Bitte wählen Sie eine gültige Zielsprache aus.", result.ErrorMessage);
        }


        [TestMethod]
        public void ValidateOriginLanguages_ValidLanguage_ReturnsSuccess()
        {
            viewModel.LanguageFrom = 4;
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(viewModel);
            var result = viewModel.ValidateOriginLanguages(validationContext);

            Assert.AreEqual(ValidationResult.Success, result);
        }

        [TestMethod]
        public void ValidateOriginLanguages_InvalidLanguage_ReturnsError()
        {
            viewModel.LanguageFrom = -1;
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(viewModel);
            var result = viewModel.ValidateOriginLanguages(validationContext);

            Assert.AreNotEqual(ValidationResult.Success, result);
            Assert.AreEqual("Bitte wählen Sie eine gültige Ausgangssprache aus.", result.ErrorMessage);
        }

        [TestMethod]
        public void Translation_IsRequired_ReturnsError()
        {
            viewModel.Translation = null;
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(viewModel);
            bool isValid = Validator.TryValidateObject(viewModel, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Translation ist erforderlich"));
        }

        [TestMethod]
        public void Translation_IsRequired_ReturnsSuccess()
        {
            viewModel.Translation = new Translation();
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(viewModel);
            bool isValid = Validator.TryValidateObject(viewModel, validationContext, validationResults, true);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, validationResults.Count);
        }
    }
}
