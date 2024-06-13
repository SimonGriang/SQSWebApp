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
        private Mock<ITranslationService> _translationServiceMock;
        private Mock<ITranslationRepository> _translationRepositoryMock;
        private Mock<ILanguageRepository> _languageRepositoryMock;
        private Mock<ICreateTranslationViewModelHandler> _createTranslationViewModelHandlerMock;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<HomeController>>();
            _translationServiceMock = new Mock<ITranslationService>();
            _translationRepositoryMock = new Mock<ITranslationRepository>();
            _languageRepositoryMock = new Mock<ILanguageRepository>();
            _createTranslationViewModelHandlerMock = new Mock<ICreateTranslationViewModelHandler>();

            _controller = new HomeController(
                _loggerMock.Object,
                _translationServiceMock.Object,
                _languageRepositoryMock.Object,
                _translationRepositoryMock.Object,
                _createTranslationViewModelHandlerMock.Object
            );
        }

        [TestMethod]
        public void Index_ReturnsViewResult(){
             // Arrange

            var targetLanguageList = new List<Language>
            {
                new Language
                {
                    Abbreviation = "en",
                    ID = 1,
                    isOriginLanguage = true,
                    isTargetLanguage = false,
                    Name = "English"
                }
            };

            var originLanguageList = new List<Language>
            {
            new Language
                {
                    Abbreviation = "de",
                    ID = 2,
                    isOriginLanguage = false,
                    isTargetLanguage = true,
                    Name = "German"
                }
            };

            var languageList = new List<Language>();
            languageList.AddRange(targetLanguageList);
            languageList.AddRange(originLanguageList);

            var viewModel = new CreateTranslationViewModel(){originLanguages = originLanguageList, targetLanguages = targetLanguageList};
            _languageRepositoryMock.Setup(repo => repo.GetAllLanguages()).Returns(languageList);
            _createTranslationViewModelHandlerMock.Setup(x => x.createViewModel()).Returns(viewModel);


            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(CreateTranslationViewModel), result.Model.GetType());
        }

        [TestMethod]
        public void Index_ReturnsNotFound_WhenOriginLanguagesIsNull()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel { originLanguages = null, targetLanguages = new List<Language>() };
            _createTranslationViewModelHandlerMock.Setup(x => x.createViewModel()).Returns(viewModel);

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Index_ReturnsNotFound_WhenTargetLanguagesIsNull()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel { originLanguages = new List<Language>(), targetLanguages = null };
            _createTranslationViewModelHandlerMock.Setup(x => x.createViewModel()).Returns(viewModel);

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Index_ReturnsNotFound_WhenOriginLanguagesIsEmpty()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel { originLanguages = new List<Language>(), targetLanguages = new List<Language> {new Language(){ 
                    Abbreviation = "en",
                    ID = 1,
                    isOriginLanguage = false,
                    isTargetLanguage = true,
                    Name = "English"
            }}};
            _createTranslationViewModelHandlerMock.Setup(x => x.createViewModel()).Returns(viewModel);

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Index_ReturnsNotFound_WhenTargetLanguagesIsEmpty()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel { 
                originLanguages = new List<Language> { new Language(){ 
                    Abbreviation = "en",
                    ID = 1,
                    isOriginLanguage = true,
                    isTargetLanguage = false,
                    Name = "English"
            } 
            }, targetLanguages = new List<Language>() };
            _createTranslationViewModelHandlerMock.Setup(x => x.createViewModel()).Returns(viewModel);

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
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
            _createTranslationViewModelHandlerMock.Setup(handler => handler.createViewModel()).Returns(viewModel);
            _translationServiceMock.Setup(service => service.TranslateTextAsync(It.IsAny<CreateTranslationViewModel>())).ReturnsAsync(viewModel);

            // Act
            var result = await _controller.Index(returnedViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(CreateTranslationViewModel), result.Model.GetType());
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
            _createTranslationViewModelHandlerMock.Setup(handler => handler.createViewModel()).Returns(new CreateTranslationViewModel());

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
            _createTranslationViewModelHandlerMock.Setup(handler => handler.createViewModel()).Returns(new CreateTranslationViewModel());

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
            _createTranslationViewModelHandlerMock.Setup(handler => handler.createViewModel()).Returns(new CreateTranslationViewModel());

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