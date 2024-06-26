using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeepL;
using DeepL.Model;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Tests
{
    [TestClass]
    [TestCategory("UnitTests")]
    public class TranslationServiceTests
    {
        private Mock<ITranslatorWrapper> _translatorMock;
        private TranslationService _translationService;

        public TranslationServiceTests()
        {
            _translatorMock = new Mock<ITranslatorWrapper>();
            _translationService = new TranslationService(_translatorMock.Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _translatorMock = new Mock<ITranslatorWrapper>();
            _translationService = new TranslationService(_translatorMock.Object);
        }

        /// <summary>
        /// Represents an asynchronous operation that can return a value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
        [TestMethod]
        public async Task TranslateTextAsync_WithValidInput_ShouldTranslateAndReturnViewModel()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel
            {
                Translation = new Translation
                {
                    OriginalLanguage = new Models.Language { Name = "English", Abbreviation = "en" },
                    TranslatedLanguage = new Models.Language { Name = "German", Abbreviation = "de" },
                    OriginalText = "Hello"
                }
            };

            var textResult = new TextResult("Hallo", "en");
            _translatorMock.Setup(t => t.TranslateTextAsync("Hello", It.IsAny<string>(), "de")).ReturnsAsync(textResult);

            // Act
            var resultViewModel = await _translationService.TranslateTextAsync(viewModel);

            // Assert
            Assert.IsNotNull(resultViewModel);
            Assert.IsNotNull(resultViewModel.Translation);
            Assert.AreEqual("Hallo", resultViewModel.Translation.TranslatedText);
        }

        /// <summary>
        /// Represents an asynchronous operation that can return a value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
        [TestMethod]
        public async Task TranslateTextAsync_WithValidInputDL_ShouldTranslateAndReturnViewModel()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel
            {
                Translation = new Translation
                {
                    OriginalLanguage = new Models.Language { Name = "Detected Language", Abbreviation = "DL" },
                    TranslatedLanguage = new Models.Language { Name = "German", Abbreviation = "de" },
                    OriginalText = "Hello"
                }
            };

            _translatorMock.Setup(t => t.TranslateTextAsync("Hello", It.IsAny<string>(), "de")).ReturnsAsync(new TextResult("Hallo", "en"));

            // Act
            var resultViewModel = await _translationService.TranslateTextAsync(viewModel);

            // Assert
            Assert.IsNotNull(resultViewModel);
            Assert.IsNotNull(resultViewModel.Translation);
            Assert.AreEqual("Hallo", resultViewModel.Translation.TranslatedText);
        }

        /// <summary>
        /// Asynchronously translates the text based on the provided view model.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task TranslateTextAsync_WithInvalidInputNoTranslatedLanguage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel
            {
                Translation = new Translation
                {
                    OriginalLanguage = new Models.Language { Name = "English", Abbreviation = "en" },
                    TranslatedLanguage = null,
                    OriginalText = "Hello"
                }
            };

            // Assert & Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _translationService.TranslateTextAsync(viewModel));
        }

        /// <summary>
        /// Asynchronously translates the text provided in the view model.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task TranslateTextAsync_WithInvalidInputNoOriginalText_ShouldThrowArgumentNullException()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel
            {
                Translation = new Translation
                {
                    OriginalLanguage = new Models.Language { Name = "English", Abbreviation = "en" },
                    TranslatedLanguage = new Models.Language { Name = "German", Abbreviation = "de" },
                    OriginalText = null
                }
            };

            // Assert & Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _translationService.TranslateTextAsync(viewModel));
        }

        /// <summary>
        /// Asynchronously translates the text using the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model containing the translation details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task TranslateTextAsync_WithInvalidInputNoOriginalLanguage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel
            {
                Translation = new Translation
                {
                    TranslatedLanguage = new Models.Language { Name = "English", Abbreviation = "en" },
                    OriginalLanguage = null,
                    OriginalText = "Hello"
                }
            };

            // Assert & Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _translationService.TranslateTextAsync(viewModel));
        }

        /// <summary>
        /// Tests the TranslateTextAsync method with invalid input (no original language abbreviation) and expects an ArgumentNullException to be thrown.
        /// </summary>
        [TestMethod]
        public async Task TranslateTextAsync_WithInvalidInputNoOriginalLanguageAbbreviation_ShouldThrowArgumentNullException()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel
            {
                Translation = new Translation
                {
                    TranslatedLanguage = new Models.Language { Name = "English", Abbreviation = null },
                    OriginalLanguage = new Models.Language { Name = "German", Abbreviation = "de" },
                    OriginalText = "Hello"
                }
            };

            // Assert & Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _translationService.TranslateTextAsync(viewModel));
        }

        /// <summary>
        /// Asynchronously translates the text using the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model containing the translation details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task TranslateTextAsync_WithInvalidInputNoTranslatedLanguageAbbreviation_ShouldThrowArgumentNullException()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel
            {
                Translation = new Translation
                {
                    TranslatedLanguage = new Models.Language { Name = "English", Abbreviation = "en" },
                    OriginalLanguage = new Models.Language { Name = "German", Abbreviation = null },
                    OriginalText = "Hello"
                }
            };

            // Assert & Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _translationService.TranslateTextAsync(viewModel));
        }

        /// <summary>
        /// Tests the TranslateTextAsync method with invalid input and no translation.
        /// Expects the method to throw an ArgumentNullException.
        /// </summary>
        [TestMethod]
        public async Task TranslateTextAsync_WithInvalidInputNoTranslation_ShouldThrowArgumentNullException()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel
            {
                Translation = null
            };

            // Assert & Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _translationService.TranslateTextAsync(viewModel));
        }

        /// <summary>
        /// Test method to verify that the <see cref="TranslationService.getDeeplLanguages"/> method returns all languages.
        /// </summary>
        [TestMethod]
        public async Task getDeeplLanguages_ShouldReturnAllLanguages()
        {
            // Arrange
            var sourceLanguages = new List<SourceLanguage>
            {
                new SourceLanguage ( "en", "English"),
                new SourceLanguage ( "de", "German")
            };
            var targetLanguages = new List<TargetLanguage>
            {
                new TargetLanguage ("de", "German", false),
                new TargetLanguage ("fr" ,"French", false),
            };

            _translatorMock.Setup(t => t.GetSourceLanguagesAsync()).ReturnsAsync(sourceLanguages.ToArray());
            _translatorMock.Setup(t => t.GetTargetLanguagesAsync()).ReturnsAsync(targetLanguages.ToArray());

            // Act
            List<Models.Language> resultLanguages = await _translationService.getDeeplLanguages();

            // Assert
            Assert.AreEqual(3, resultLanguages.Count); // Gesamte Anzahl der Sprachen
            Assert.IsTrue(resultLanguages.Exists(l => l.Abbreviation == "en" && l.IsOriginLanguage && !l.IsTargetLanguage));
            Assert.IsTrue(resultLanguages.Exists(l => l.Abbreviation == "de" && l.IsOriginLanguage && l.IsTargetLanguage));
            Assert.IsTrue(resultLanguages.Exists(l => l.Abbreviation == "fr" && !l.IsOriginLanguage && l.IsTargetLanguage));
        }
    }
}
