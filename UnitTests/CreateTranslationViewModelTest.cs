using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApp.Models;
using WebApp.ViewModels;

/// <summary>
/// Contains unit tests for the <see cref="CreateTranslationViewModel"/> class.
/// </summary>
namespace WebApp.Tests
{
    [TestClass]
    [TestCategory("UnitTests")]
    public class CreateTranslationViewModelTests
    {
        /// <summary>
        /// Creation of a valid CreateTranslationViewModel object.
        /// </summary>
        private CreateTranslationViewModel viewModel = new CreateTranslationViewModel{
            targetLanguages = new List<Language>{
                new Language { ID = 1, Abbreviation = "de", IsTargetLanguage = true, IsOriginLanguage = false },
                new Language { ID = 2, Abbreviation = "en-US", IsTargetLanguage = true, IsOriginLanguage = false },
                new Language { ID = 3, Abbreviation = "en-GB", IsTargetLanguage = true, IsOriginLanguage = false }
                },
            originLanguages= new List<Language>{
                new Language { ID = 4, Abbreviation = "DL", IsTargetLanguage = false, IsOriginLanguage = true },
                new Language { ID = 5, Abbreviation = "en", IsTargetLanguage = false, IsOriginLanguage = true }
                },
            English = 1,
            EnglishUS = 2,
            EnglishGB = 3,
            German = 4,
            DetectLanguage = 5,
        };


        /// <summary>
        /// Tests the validation of the LanguageTo property when a valid language is set.
        /// </summary>
        /// <remarks>
        /// This test sets the LanguageTo property of the view model to a valid language value and validates it using the Validator.TryValidateProperty method.
        /// It asserts that the validation result is successful and that there are no validation errors.
        /// </remarks>
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

        /// <summary>
        /// Tests the behavior of the ValidateLanguageTo method when an invalid language is set for the LanguageTo property.
        /// </summary>
        /// <remarks>
        /// This test verifies that an error message is returned when an invalid language is set for the LanguageTo property of the view model.
        /// </remarks>
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

        /// <summary>
        /// Tests the validation logic of the <see cref="CreateTranslationViewModel.ValidateLanguageTo"/> method
        /// when the targetLanguages property is null, and expects an error message to be returned.
        /// </summary>
        [TestMethod]
        public void ValidateLanguageTo_targetLanguagesNull_ReturnsError()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel
            {
                targetLanguages = null,
                originLanguages = new List<Language>{
                new Language { ID = 4, Abbreviation = "DL", IsTargetLanguage = false, IsOriginLanguage = true },
                new Language { ID = 5, Abbreviation = "en", IsTargetLanguage = false, IsOriginLanguage = true }
                },
                English = 1,
                EnglishUS = 2,
                EnglishGB = 3,
                German = 4,
                DetectLanguage = 5,
            };
            viewModel.LanguageTo = -1;

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(viewModel);

            // Act
            var result = viewModel.ValidateLanguageTo(validationContext);

            // Assert
            Assert.AreNotEqual(ValidationResult.Success, result);
            Assert.AreEqual("Keine Sprachen vorhanden, Validierung übersprungen.", result.ErrorMessage);
        }

        /// <summary>
        /// Validates the origin languages when the originLanguages list is null.
        /// </summary>
        /// <returns>
        /// Returns a validation result indicating an error if the originLanguages list is null.
        /// </returns>
        [TestMethod]
        public void ValidateLanguagOrigin_OriginLanguagesNull_ReturnsError()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel
            {
                targetLanguages = new List<Language>{
                new Language { ID = 4, Abbreviation = "DL", IsTargetLanguage = false, IsOriginLanguage = true },
                new Language { ID = 5, Abbreviation = "en", IsTargetLanguage = false, IsOriginLanguage = true }
                },
                originLanguages = null,
                English = 1,
                EnglishUS = 2,
                EnglishGB = 3,
                German = 4,
                DetectLanguage = 5,
            };
            viewModel.LanguageTo = -1;

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(viewModel);

            // Act
            var result = viewModel.ValidateOriginLanguages(validationContext);

            // Assert
            Assert.AreNotEqual(ValidationResult.Success, result);
            Assert.AreEqual("Keine Sprachen vorhanden, Validierung übersprungen.", result.ErrorMessage);
        }

        /// <summary>
        /// Tests the validation of the LanguageTo property when it is valid.
        /// </summary>
        [TestMethod]
        public void ValidateLanguageTo_Valid_ReturnsSuccess()
        {
            viewModel.LanguageFrom = 4;
            viewModel.LanguageTo = 1;
            var validationContext = new ValidationContext(viewModel);
            var result = viewModel.ValidateLanguageTo(validationContext);

            Assert.AreEqual(ValidationResult.Success, result);
        }


        /// <summary>
        /// Tests the validation of origin languages when a valid language is selected.
        /// </summary>
        [TestMethod]
        public void ValidateOriginLanguages_ValidLanguage_ReturnsSuccess()
        {
            viewModel.LanguageFrom = 4;
            var validationContext = new ValidationContext(viewModel);
            var result = viewModel.ValidateOriginLanguages(validationContext);

            Assert.AreEqual(ValidationResult.Success, result);
        }

        /// <summary>
        /// Tests the behavior of the <see cref="CreateTranslationViewModel.ValidateOriginLanguages"/> method when an invalid language is selected.
        /// </summary>
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

        /// <summary>
        /// Tests the behavior of the <see cref="Translation_IsRequired_ReturnsError"/> method.
        /// It verifies that when the translation is null, the method returns an error message.
        /// </summary>
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

        /// <summary>
        /// Tests the behavior of the Translation_IsRequired method when the Translation property is set to a new instance of Translation.
        /// It verifies that the validation succeeds and no validation errors are returned.
        /// </summary>
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
