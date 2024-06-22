using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.IntegrationTests
{
    [TestClass]
    [TestCategory("IntegrationTests")]
    public class TranslationServiceIntegrationTest
    {
        private static TranslationService _TranslationService = new TranslationService(new TranslatorWrapper("f2981bee-344a-4a1f-b65f-877950fa3855:fx")); 
        
        
        [TestMethod]
        public async Task TranslateTextAsync_ShouldTranslateText_WithOriginLanguage()
        {
            // Arrange
            CreateTranslationViewModel viewModel = new CreateTranslationViewModel
            {
                Translation = new Translation
                {
                    OriginalText = "This is a test",
                    OriginalLanguage = new Language { Name = "English", Abbreviation = "en" },
                    TranslatedLanguage = new Language { Name = "German", Abbreviation = "de" }
                }
            };

            // Act
            var result = await _TranslationService.TranslateTextAsync(viewModel);

            // Assert
            Assert.IsNotNull(result.Translation);
            Assert.IsNotNull(result.Translation.TranslatedText);
            Assert.AreEqual("Dies ist ein Test", result.Translation.TranslatedText);
        }

        [TestMethod]
        public async Task TranslateTextAsync_ShouldTranslateText_WithoutOriginLanguage()
        {
            // Arrange
            CreateTranslationViewModel viewModel = new CreateTranslationViewModel
            {
                Translation = new Translation
                {
                    OriginalText = "This is a test",
                    OriginalLanguage = new Language { Name = "DetectLanguage", Abbreviation = "DL" },
                    TranslatedLanguage = new Language { Name = "German", Abbreviation = "de" }
                }
            };

            // Act
            var result = await _TranslationService.TranslateTextAsync(viewModel);

            // Assert
            Assert.IsNotNull(result.Translation);
            Assert.IsNotNull(result.Translation.TranslatedText);
            Assert.AreEqual("Dies ist ein Test", result.Translation.TranslatedText);
        }

        [TestMethod]
        public async Task GetDeeplLanguages_ShouldReturnLanguages()
        {
            // Act
            var result = await _TranslationService.getDeeplLanguages();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
    }
}



