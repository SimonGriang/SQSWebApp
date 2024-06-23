using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModelHandler;
using WebApp.ViewModels;

namespace WebApp.Tests
{
    /// <summary>
    /// Represents a unit test class for the CreateTranslationViewModelHandler class.
    /// </summary>
    [TestClass]
    [TestCategory("UnitTests")]
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

        /// <summary>
        /// Tests the CreateViewModel method of the CreateTranslationViewModelHandler class.
        /// It verifies that the method returns a view model with the expected languages.
        /// </summary>
        [TestMethod]
        public void CreateViewModel_ShouldReturnViewModelWithLanguages()
        {
            // Arrange
            var languages = new List<Language>
            {
                new Language { ID = 1, Abbreviation = "de", IsTargetLanguage = true, IsOriginLanguage = false },
                new Language { ID = 2, Abbreviation = "en-US", IsTargetLanguage = true, IsOriginLanguage = false },
                new Language { ID = 3, Abbreviation = "en-GB", IsTargetLanguage = true, IsOriginLanguage = false },
                new Language { ID = 4, Abbreviation = "DL", IsTargetLanguage = false, IsOriginLanguage = true },
                new Language { ID = 5, Abbreviation = "en", IsTargetLanguage = false, IsOriginLanguage = true }
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