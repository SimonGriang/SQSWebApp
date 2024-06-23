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
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using DeepL;
using System.Diagnostics;

namespace WebApp.Tests
{
    [TestClass]
    [TestCategory("UnitTests")]
    public class HomeControllerTests
    {
        public required HomeController _controller;
        private Mock<ITranslationService> _translationServiceMock = new Mock<ITranslationService>();
        private Mock<ITranslationRepository> _translationRepositoryMock = new Mock<ITranslationRepository>();
        private Mock<ILanguageRepository> _languageRepositoryMock = new Mock<ILanguageRepository>();
        private Mock<ICreateTranslationViewModelHandler> _createTranslationViewModelHandlerMock = new Mock<ICreateTranslationViewModelHandler>();
        private Mock<ITempDataDictionary> _tempDataMock = new Mock<ITempDataDictionary>();


        /// <summary>
        /// Initializes the test setup before each test method is executed.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _controller = new HomeController(
                _translationServiceMock.Object,
                _languageRepositoryMock.Object,
                _translationRepositoryMock.Object,
                _createTranslationViewModelHandlerMock.Object
            )
            {
                TempData = _tempDataMock.Object
            };

        }

        /// <summary>
        /// Tests the behavior of the Index method in the HomeController class, 
        /// ensuring that it returns a ViewResult with the expected model type.
        /// </summary>
        [TestMethod]
        public void Index_ReturnsViewResult(){
            
            // Arrange
            var targetLanguageList = new List<Language>
            {
                new Language
                {
                    Abbreviation = "en",
                    ID = 1,
                    IsOriginLanguage = true,
                    IsTargetLanguage = false,
                    Name = "English"
                }
            };

            var originLanguageList = new List<Language>
            {
            new Language
                {
                    Abbreviation = "de",
                    ID = 2,
                    IsOriginLanguage = false,
                    IsTargetLanguage = true,
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
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(typeof(CreateTranslationViewModel), result.Model.GetType());
        }

        /// <summary>
        /// Tests the behavior of the Index method when the originLanguages parameter is null.
        /// It verifies that the method returns a NotFoundResult.
        /// </summary>
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

        /// <summary>
        /// Tests the behavior of the Index method when the target languages are null.
        /// It verifies that the method returns a NotFoundResult.
        /// </summary>
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

        /// <summary>
        /// Tests the behavior of the Index method when an exception is thrown.
        /// </summary>
        /// <remarks>
        /// This test verifies that the Index method throws an exception and sets the appropriate error message in TempData.
        /// </remarks>
        [TestMethod]
        public void Index_ReturnsErrorMessage_ExceptionIsThrown()
        {
            // Arrange
            _createTranslationViewModelHandlerMock.Setup(x => x.createViewModel()).Throws(new System.Exception("Test Exception Message"));
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            _controller.TempData = tempData;

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(_controller.TempData.ContainsKey("ErrorMessage"));
            Assert.AreEqual("Ein unerwarteter Fehler ist aufgetreten: " + "Test Exception Message", _controller.TempData["ErrorMessage"]);
        }

        /// <summary>
        /// Tests the behavior of the Index method when the originLanguages list is empty.
        /// It verifies that the method returns a NotFoundResult.
        /// </summary>
        [TestMethod]
        public void Index_ReturnsNotFound_WhenOriginLanguagesIsEmpty()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel { originLanguages = new List<Language>(), targetLanguages = new List<Language> {new Language(){ 
                    Abbreviation = "en",
                    ID = 1,
                    IsOriginLanguage = false,
                    IsTargetLanguage = true,
                    Name = "English"
            }}};
            _createTranslationViewModelHandlerMock.Setup(x => x.createViewModel()).Returns(viewModel);

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        /// <summary>
        /// Tests the behavior of the Index method when the target languages list is empty.
        /// It verifies that the method returns a NotFoundResult.
        /// </summary>
        [TestMethod]
        public void Index_ReturnsNotFound_WhenTargetLanguagesIsEmpty()
        {
            // Arrange
            var viewModel = new CreateTranslationViewModel { 
                originLanguages = new List<Language> { new Language(){ 
                    Abbreviation = "en",
                    ID = 1,
                    IsOriginLanguage = true,
                    IsTargetLanguage = false,
                    Name = "English"
            } 
            }, targetLanguages = new List<Language>() };
            _createTranslationViewModelHandlerMock.Setup(x => x.createViewModel()).Returns(viewModel);

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        /// <summary>
        /// Tests the behavior of the Index method when the target view model is null.
        /// It verifies that the method returns a NotFoundResult.
        /// </summary>
        [TestMethod]
        public void Index_ReturnsNotFound_WhenTargetViewModelIsNull()
        {
            // Arrange
            CreateTranslationViewModel? viewModel = null;
            _createTranslationViewModelHandlerMock.Setup(x => x.createViewModel()).Returns(viewModel!);

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        /// <summary>
        /// Represents an asynchronous operation that can return a value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
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

            var viewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 2,
                Translation = new Translation
                {
                    OriginalText = "Hello",
                    TranslatedText = "Hallo",
                    OriginalLanguage = new Language(),
                    TranslatedLanguage = new Language(),
                }
            };

            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageTo)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageFrom)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageTo)).Returns(new Language());
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageFrom)).Returns(new Language());
            _createTranslationViewModelHandlerMock.Setup(handler => handler.createViewModel()).Returns(viewModel);
            _translationServiceMock.Setup(service => service.TranslateTextAsync(It.IsAny<CreateTranslationViewModel>())).ReturnsAsync(returnedViewModel);

            // Act
            var result = await _controller.Index(returnedViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(typeof(CreateTranslationViewModel), result.Model.GetType());
        }

        /// <summary>
        /// Represents an asynchronous operation that can return a value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
        [TestMethod]
        public async Task Index_WithValidViewModel_ThrowsExceptionInternalError()
        {
            // Arrange
            var returnedViewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 2,
                Translation = null
            };

            var viewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 2,
                Translation = new Translation
                {
                    OriginalText = "Hello",
                    TranslatedText = "Hallo",
                    OriginalLanguage = new Language(),
                    TranslatedLanguage = new Language(),
                }
            };

            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageTo)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageFrom)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageTo)).Returns(new Language());
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageFrom)).Returns(new Language());
            _createTranslationViewModelHandlerMock.Setup(handler => handler.createViewModel()).Returns(viewModel);
            _translationServiceMock.Setup(service => service.TranslateTextAsync(It.IsAny<CreateTranslationViewModel>())).ReturnsAsync(returnedViewModel);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            _controller.TempData = tempData;

            // Act
            var result = await _controller.Index(returnedViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(_controller.TempData.ContainsKey("ErrorMessage"));
            Assert.AreEqual("Ein unerwarteter Fehler ist aufgetreten: " + "Modelstate ist not valid or Translation is null.", _controller.TempData["ErrorMessage"]);
        }

        /// <summary>
        /// Represents an asynchronous operation that can return a value.
        /// </summary>
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

        /// <summary>
        /// Represents an asynchronous operation that can return a value.
        /// </summary>
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

        /// <summary>
        /// Represents an asynchronous operation that can return a value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
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

        /// <summary>
        /// Represents an asynchronous operation that produces a result of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
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

        /// <summary>
        /// Represents an asynchronous operation that can return a value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
        [TestMethod]
        public async Task Index_WithValidModelState_ThrowsDeeplConnectionException()
        {
            // Arrange
            var returnedViewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 2,
                Translation = new Translation
                {
                    OriginalText = "Hello",
                    TranslatedText = "Hallo",
                    OriginalLanguage = new Language(),
                    TranslatedLanguage = new Language()
                }
            };

            // Arrange
            var viewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 2,
                Translation = new Translation
                {
                    OriginalText = "Hello",
                    TranslatedText = "Hallo",
                    OriginalLanguage = new Language(),
                    TranslatedLanguage = new Language()
                }
            };
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageTo)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageFrom)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageTo)).Returns(new Language());
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageFrom)).Returns(new Language());
            _translationServiceMock.Setup(service => service.TranslateTextAsync(It.IsAny<CreateTranslationViewModel>())).Throws(new ConnectionException("Test Exception Message", new Exception()));
            _createTranslationViewModelHandlerMock.Setup(handler => handler.createViewModel()).Returns(viewModel);

            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            _controller.TempData = tempData;

            // Act
            var result = await _controller.Index(returnedViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(_controller.TempData.ContainsKey("ErrorMessage"));
            Assert.AreEqual("Es konnte keine Verbindung zum Webservice aufgerufen werden: " + "Test Exception Message", _controller.TempData["ErrorMessage"]);
        }

        /// <summary>
        /// Represents an asynchronous operation that can return a value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
        [TestMethod]
        public async Task Index_WithValidModelState_ThrowsDeeplQuotaExceededException()
        {
            // Arrange
            var returnedViewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 2,
                Translation = new Translation
                {
                    OriginalText = "Hello",
                    TranslatedText = "Hallo",
                    OriginalLanguage = new Language(),
                    TranslatedLanguage = new Language()
                }
            };

            // Arrange
            var viewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 2,
                Translation = new Translation
                {
                    OriginalText = "Hello",
                    TranslatedText = "Hallo",
                    OriginalLanguage = new Language(),
                    TranslatedLanguage = new Language()
                }
            };
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageTo)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageFrom)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageTo)).Returns(new Language());
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageFrom)).Returns(new Language());
            _translationServiceMock.Setup(service => service.TranslateTextAsync(It.IsAny<CreateTranslationViewModel>())).Throws(new QuotaExceededException("Test Exception Message"));
            _createTranslationViewModelHandlerMock.Setup(handler => handler.createViewModel()).Returns(viewModel);

            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            _controller.TempData = tempData;

            // Act
            var result = await _controller.Index(returnedViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(_controller.TempData.ContainsKey("ErrorMessage"));
            Assert.AreEqual("Das Kontigent an möglichen Übersetzungen der Software ist ereicht: " + "Test Exception Message", _controller.TempData["ErrorMessage"]);
        }


        /// <summary>
        /// Represents an asynchronous operation that can return a value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
        [TestMethod]    
        public async Task Index_WithValidModelState_ThrowsDeeplException()
        {
            // Arrange
            var returnedViewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 2,
                Translation = new Translation
                {
                    OriginalText = "Hello",
                    TranslatedText = "Hallo",
                    OriginalLanguage = new Language(),
                    TranslatedLanguage = new Language()
                }
            };

            // Arrange
            var viewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 2,
                Translation = new Translation
                {
                    OriginalText = "Hello",
                    TranslatedText = "Hallo",
                    OriginalLanguage = new Language(),
                    TranslatedLanguage = new Language()
                }
            };
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageTo)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageFrom)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageTo)).Returns(new Language());
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageFrom)).Returns(new Language());
            _translationServiceMock.Setup(service => service.TranslateTextAsync(It.IsAny<CreateTranslationViewModel>())).Throws(new DeepLException("Test Exception Message"));
            _createTranslationViewModelHandlerMock.Setup(handler => handler.createViewModel()).Returns(viewModel);

            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            _controller.TempData = tempData;

            // Act
            var result = await _controller.Index(returnedViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(_controller.TempData.ContainsKey("ErrorMessage"));
            Assert.AreEqual("Fehlerhafte Sprachkombination angegeben: " + "Test Exception Message", _controller.TempData["ErrorMessage"]);
        }


        /// <summary>
        /// Tests the <see cref="HomeController.Error"/> method to ensure it returns a <see cref="ViewResult"/> with an <see cref="ErrorViewModel"/>.
        /// </summary>
        [TestMethod]
        public void Error_ReturnsViewResultWithErrorViewModel()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            using (var activity = new Activity("TestActivity"))
            {
                activity.Start();
                
                // Act
                var result = _controller.Error() as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                var model = result.Model as ErrorViewModel;
                Assert.IsNotNull(model);
                Assert.IsNotNull(model.RequestId);

                activity.Stop();
            }
        }


        /// <summary>
        /// Tests the History method of the HomeController and verifies that it returns a ViewResult with translations.
        /// </summary>
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

        /// <summary>
        /// Tests the behavior of the Details method when a null id is provided.
        /// Expects the method to return a NotFoundResult.
        /// </summary>
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

        /// <summary>
        /// Tests the behavior of the Details method when an invalid ID is provided, and expects a NotFoundResult.
        /// </summary>
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

        /// <summary>
        /// Tests the Details method of the HomeController when a valid ID is provided, and expects a ViewResult to be returned.
        /// </summary>
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
            Assert.IsTrue(result.ViewData.ModelState.IsValid);
        }

        /// <summary>
        /// Tests the behavior of the Delete method when a null id is provided. 
        /// It should return a NotFoundResult.
        /// </summary>
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

        /// <summary>
        /// Tests the Delete method of the HomeController when an invalid ID is provided, and expects a NotFoundResult.
        /// </summary>
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

        /// <summary>
        /// Tests the Delete method of the HomeController when a valid ID is provided, and verifies that it returns a ViewResult.
        /// </summary>
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

        /// <summary>
        /// Tests the behavior of the DeleteConfirmed method when a valid ID is provided, and verifies that it redirects to the Index action.
        /// </summary>
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
            Assert.AreEqual("History", result.ActionName);
        }

        /// <summary>
        /// Tests the behavior of the DeleteConfirmed method when an invalid ID is provided.
        /// It should redirect to the index page.
        /// </summary>
        [TestMethod]
        public void DeleteConfirmed_WithInvalidId_RedirectsToIndex()
        {
            // Arrange
            int id = 1;
            _translationRepositoryMock.Setup(repo => repo.GetTranslationById(id)).Returns((Translation)null!);

            // Act
            var result = _controller.DeleteConfirmed(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}