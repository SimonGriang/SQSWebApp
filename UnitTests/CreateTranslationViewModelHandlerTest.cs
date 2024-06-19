using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using WebApp.Models;
using WebApp.ViewModelHandler;
using WebApp.ViewModels;

namespace WebApp.Tests
{
    [TestClass]
    public class CreateTranslationViewModelHandlerTest
    {
        private Mock<ILanguageRepository> _languageRepositoryMock = new Mock<ILanguageRepository>();
        private CreateTranslationViewModelHandler _createTranslationViewModelhandler = new CreateTranslationViewModelHandler(new Mock<ILanguageRepository>().Object);

        [TestInitialize]
        public void TestInitialize()
        {
            _languageRepositoryMock = new Mock<ILanguageRepository>();
            _createTranslationViewModelhandler = new CreateTranslationViewModelHandler(_languageRepositoryMock.Object);
        }

        [TestMethod]
        public void CreateViewModel_ShouldReturnViewModelWithLanguages()
        {
            // Arrange
            var languages = new List<Language>
            {
                new Language { ID = 1, Abbreviation = "de", isTargetLanguage = true, isOriginLanguage = false },
                new Language { ID = 2, Abbreviation = "en-US", isTargetLanguage = true, isOriginLanguage = false },
                new Language { ID = 3, Abbreviation = "en-GB", isTargetLanguage = true, isOriginLanguage = false },
                new Language { ID = 4, Abbreviation = "DL", isTargetLanguage = false, isOriginLanguage = true },
                new Language { ID = 5, Abbreviation = "en", isTargetLanguage = false, isOriginLanguage = true }
            };
            _languageRepositoryMock.Setup(x => x.GetAllLanguages()).Returns(languages);

            // Act
            var viewModel = _createTranslationViewModelhandler.createViewModel();

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.originLanguages);
            Assert.IsNotNull(viewModel.targetLanguages);
            Assert.AreEqual(3, viewModel.targetLanguages.Count);
            Assert.AreEqual(2, viewModel.originLanguages.Count);
            Assert.AreEqual(1, viewModel.German);
            Assert.AreEqual(2, viewModel.EnglishUS);
            Assert.AreEqual(3, viewModel.EnglishGB);
            Assert.AreEqual(4, viewModel.DetectLanguage);
            Assert.AreEqual(5, viewModel.English);
        }
    }
}