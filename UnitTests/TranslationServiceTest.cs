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
    public class TranslationServiceTests
    {
        private Mock<ITranslator> _translatorMock = new Mock<ITranslator>();
        private ITranslationService _translationService = new TranslationService();

        [TestInitialize]
        public void TestInitialize()
        {
            _translatorMock = new Mock<ITranslator>();
            _translationService = new TranslationService();
        }

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
            _translatorMock.Setup(t => t.TranslateTextAsync("Hello", It.IsAny<string>(), "de", It.IsAny<TextTranslateOptions>(), It.IsAny<CancellationToken>())).ReturnsAsync(textResult);

            // Act
            var resultViewModel = await _translationService.TranslateTextAsync(viewModel);

            // Assert
            Assert.IsNotNull(resultViewModel);
            Assert.IsNotNull(resultViewModel.Translation);
            Assert.AreEqual("Hallo", resultViewModel.Translation.TranslatedText);
        }

        [TestMethod]
        public async Task getDeeplLanguages_ShouldReturnAllLanguages()
        {
            // Arrange
            var sourceLanguages = new List<SourceLanguage>
            {
                new SourceLanguage ("English", "en"),
                new SourceLanguage ("German", "de")
            };
            var targetLanguages = new List<TargetLanguage>
            {
                new TargetLanguage ("German", "de", false),
                new TargetLanguage ("French", "fr", false),
            };

            _translatorMock.Setup(t => t.GetSourceLanguagesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(sourceLanguages.ToArray());
            _translatorMock.Setup(t => t.GetTargetLanguagesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(targetLanguages.ToArray());

            // Act
            var resultLanguages = await _translationService.getDeeplLanguages();

            // Assert
            Assert.AreEqual(2, resultLanguages.Count); // Gesamte Anzahl der Sprachen
            Assert.IsTrue(resultLanguages.Exists(l => l.Abbreviation == "en"));
            Assert.IsTrue(resultLanguages.Exists(l => l.Abbreviation == "de"));
            Assert.IsTrue(resultLanguages.Exists(l => l.Abbreviation == "fr"));
        }
    }
}
