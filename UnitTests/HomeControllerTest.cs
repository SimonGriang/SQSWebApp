using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Controllers;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModelHandler;
using WebApp.ViewModels;

namespace WebApp.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _controller;
        private Mock<ILogger<HomeController>> _loggerMock;
        private Mock<WebAppContext> _contextMock;
        private Mock<TranslationService> _translationServiceMock;
        private Mock<TranslationRepository> _translationRepositoryMock;
        private Mock<LanguageRepository> _languageRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<HomeController>>();
            _contextMock = new Mock<WebAppContext>();
            _translationServiceMock = new Mock<TranslationService>();
            _translationRepositoryMock = new Mock<TranslationRepository>();
            _languageRepositoryMock = new Mock<LanguageRepository>();

            _controller = new HomeController(
                _loggerMock.Object,
                _contextMock.Object,
                _translationServiceMock.Object,
                _languageRepositoryMock.Object,
                _translationRepositoryMock.Object
            );
        }

        [TestMethod]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel();
            _languageRepositoryMock.Setup(repo => repo.GetAllLanguages()).Returns(new List<Language>());

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewModel, result.Model);
        }

        [TestMethod]
        public async Task Index_WithValidViewModel_ReturnsViewResult()
        {
            // Arrange
            var returnedViewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 2,
                Translation = new Translation
                {
                    OriginalText = "Hello",
                    TranslatedText = "Hallo"
                }
            };
            var viewModel = new CreateTranslationViewModel();
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageTo)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageFrom)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageTo)).Returns(new Language());
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageFrom)).Returns(new Language());
            _translationServiceMock.Setup(service => service.TranslateTextAsync(It.IsAny<CreateTranslationViewModel>())).ReturnsAsync(viewModel);

            // Act
            var result = await _controller.Index(returnedViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewModel, result.Model);
        }

        [TestMethod]
        public async Task Index_WithInvalidViewModel_ReturnsViewResultWithModelError()
        {
            // Arrange
            var returnedViewModel = new CreateTranslationViewModel();
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageTo)).Returns(false);
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageFrom)).Returns(false);

            // Act
            var result = await _controller.Index(returnedViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey(""));
        }

        [TestMethod]
        public async Task Index_WithNullTranslation_ReturnsViewResultWithModelError()
        {
            // Arrange
            var returnedViewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 2,
                Translation = null
            };
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageTo)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageFrom)).Returns(true);

            // Act
            var result = await _controller.Index(returnedViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey(""));
        }

        [TestMethod]
        public async Task Index_WithNullTranslationText_ReturnsViewResultWithModelError()
        {
            // Arrange
            var returnedViewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 2,
                Translation = new Translation()
            };
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageTo)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageFrom)).Returns(true);

            // Act
            var result = await _controller.Index(returnedViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey(""));
        }

        [TestMethod]
        public async Task Index_WithValidViewModelAndModelState_ReturnsViewResultWithModelError()
        {
            // Arrange
            var returnedViewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 2,
                Translation = new Translation
                {
                    OriginalText = "Hello",
                    TranslatedText = "Hallo"
                }
            };
            var viewModel = new CreateTranslationViewModel();
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageTo)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageFrom)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageTo)).Returns(new Language());
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageFrom)).Returns(new Language());
            _translationServiceMock.Setup(service => service.TranslateTextAsync(It.IsAny<CreateTranslationViewModel>())).ReturnsAsync(viewModel);
            _controller.ModelState.AddModelError("", "Some error message");

            // Act
            var result = await _controller.Index(returnedViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey(""));
        }

        [TestMethod]
        public void History_ReturnsViewResultWithTranslations()
        {
            // Arrange
            var translations = new List<Translation>();
            _translationRepositoryMock.Setup(repo => repo.GetAllTranslations()).Returns(translations);

            // Act
            var result = _controller.History() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(translations, result.Model);
        }

        [TestMethod]
        public void Details_WithNullId_ReturnsNotFoundResult()
        {
            // Arrange
            int? id = null;

            // Act
            var result = _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Details_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int id = 1;
            _translationRepositoryMock.Setup(repo => repo.TranslationExists(id)).Returns(false);

            // Act
            var result = _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Details_WithValidId_ReturnsViewResult()
        {
            // Arrange
            int id = 1;
            var translation = new Translation();
            _translationRepositoryMock.Setup(repo => repo.TranslationExists(id)).Returns(true);
            _translationRepositoryMock.Setup(repo => repo.GetTranslationById(id)).Returns(translation);

            // Act
            var result = _controller.Details(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(translation, result.Model);
        }

        [TestMethod]
        public void Delete_WithNullId_ReturnsNotFoundResult()
        {
            // Arrange
            int? id = null;

            // Act
            var result = _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Delete_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int id = 1;
            _translationRepositoryMock.Setup(repo => repo.TranslationExists(id)).Returns(false);

            // Act
            var result = _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Delete_WithValidId_ReturnsViewResult()
        {
            // Arrange
            int id = 1;
            var translation = new Translation();
            _translationRepositoryMock.Setup(repo => repo.TranslationExists(id)).Returns(true);
            _translationRepositoryMock.Setup(repo => repo.GetTranslationById(id)).Returns(translation);

            // Act
            var result = _controller.Delete(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(translation, result.Model);
        }

        [TestMethod]
        public void DeleteConfirmed_WithValidId_RedirectsToIndex()
        {
            // Arrange
            int id = 1;
            var translation = new Translation();
            _translationRepositoryMock.Setup(repo => repo.GetTranslationById(id)).Returns(translation);

            // Act
            var result = _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void DeleteConfirmed_WithInvalidId_RedirectsToIndex()
        {
            // Arrange
            int id = 1;
            _translationRepositoryMock.Setup(repo => repo.GetTranslationById(id)).Returns((Translation)null);

            // Act
            var result = _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }
    }
}