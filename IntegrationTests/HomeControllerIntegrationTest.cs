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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DeepL.Model;

namespace WebApp.Tests
{
    [TestClass]
    [TestCategory("IntegrationTests")]
    public class HomeControllerTests
    {
        private IHost _host;
        private IServiceScope _scope;

        private Mock<ITranslatorWrapper> _translatorMock = new Mock<ITranslatorWrapper>();
        private Mock<ITranslationRepository> _translationRepositoryMock = new Mock<ITranslationRepository>();
        private Mock<ILanguageRepository> _languageRepositoryMock = new Mock<ILanguageRepository>();
        private Mock<ICreateTranslationViewModelHandler> _createTranslationViewModelHandlerMock = new Mock<ICreateTranslationViewModelHandler>();
        private Mock<ITempDataDictionary> _tempDataMock = new Mock<ITempDataDictionary>();

        private List<WebApp.Models.Language> allLanguages = new List<WebApp.Models.Language>
            {
                new WebApp.Models.Language
                {
                    Abbreviation = "de",
                    ID = 1,
                    isOriginLanguage = true,
                    isTargetLanguage = true,
                    Name = "German"
                },
                new WebApp.Models.Language
                {
                    Abbreviation = "en-US",
                    ID = 2,
                    isOriginLanguage = false,
                    isTargetLanguage = true,
                    Name = "English (American)"
                }
                ,
                new WebApp.Models.Language
                {
                    Abbreviation = "en-GB",
                    ID = 3,
                    isOriginLanguage = false,
                    isTargetLanguage = true,
                    Name = "English (British)"
                }
                ,
                new WebApp.Models.Language
                {
                    Abbreviation = "es",
                    ID = 4,
                    isOriginLanguage = true,
                    isTargetLanguage = true,
                    Name = "Spanish"
                },
                new WebApp.Models.Language
                {
                    Abbreviation = "en",
                    ID = 5,
                    isOriginLanguage = true,
                    isTargetLanguage = false,
                    Name = "English"
                },
            };

        public HomeController _controller;

        [TestInitialize]
        public void Setup()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<ITranslatorWrapper>(sp => _translatorMock.Object);
                    services.AddTransient<ITranslationRepository>(sp => _translationRepositoryMock.Object);
                    services.AddTransient<ILanguageRepository>(sp => _languageRepositoryMock.Object);
                    services.AddTransient<ICreateTranslationViewModelHandler, CreateTranslationViewModelHandler>();
                    services.AddTransient<ITranslationService, TranslationService>(); // DI for TranslationService
                    services.AddControllersWithViews();
                })
                .Build();

            _scope = _host.Services.CreateScope();

            _controller = new HomeController(
                _scope.ServiceProvider.GetRequiredService<ITranslationService>(),
                _scope.ServiceProvider.GetRequiredService<ILanguageRepository>(),
                _scope.ServiceProvider.GetRequiredService<ITranslationRepository>(),
                _scope.ServiceProvider.GetRequiredService<ICreateTranslationViewModelHandler>()
            )
            {
                TempData = _tempDataMock.Object
            };
        }


        [TestMethod]
        public void Index_ReturnsViewResult(){
            // Arrange
            _languageRepositoryMock.Setup(repo => repo.GetAllLanguages()).Returns(allLanguages);

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(typeof(CreateTranslationViewModel), result.Model.GetType());
            CreateTranslationViewModel viewModel = (CreateTranslationViewModel)result.Model;
            Assert.IsNotNull(viewModel.originLanguages);
            Assert.IsTrue(viewModel.originLanguages.Count == 3);
            Assert.IsNotNull(viewModel.targetLanguages);
            Assert.IsTrue(viewModel.targetLanguages.Count == 4);
            Assert.IsNotNull(viewModel.German);
            Assert.IsTrue(viewModel.German == 1);
            Assert.IsNotNull(viewModel.EnglishUS);
            Assert.IsTrue(viewModel.EnglishUS == 2);
            Assert.IsNotNull(viewModel.EnglishGB);
            Assert.IsTrue(viewModel.EnglishGB == 3);    
            Assert.IsNotNull(viewModel.English);
            Assert.IsTrue(viewModel.English == 5);
        }
        [TestMethod]
        public async Task Index_WithValidViewModel_ReturnsViewResult()
        {
            // Arrange
            var returnedViewModel = new CreateTranslationViewModel
            {
                LanguageTo = 1,
                LanguageFrom = 5,
                
                Translation = new Translation
                {
                    OriginalText = "Hello",
                }
            };
            _languageRepositoryMock.Setup(repo => repo.GetAllLanguages()).Returns(allLanguages);
            _languageRepositoryMock.Setup(repo => repo.GetAllLanguages()).Returns(allLanguages);
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageTo)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.LanguageExists(returnedViewModel.LanguageFrom)).Returns(true);
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageTo)).Returns(new WebApp.Models.Language
                {
                    Abbreviation = "en",
                    ID = 5,
                    isOriginLanguage = true,
                    isTargetLanguage = false,
                    Name = "English"
                });
            _languageRepositoryMock.Setup(repo => repo.GetLanguage(returnedViewModel.LanguageFrom)).Returns(new WebApp.Models.Language
                {
                    Abbreviation = "de",
                    ID = 1,
                    isOriginLanguage = true,
                    isTargetLanguage = true,
                    Name = "German"
                });
            _translatorMock.Setup(t => t.TranslateTextAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new TextResult("Hallo", "de"));


            // Act
            var result = await _controller.Index(returnedViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(typeof(CreateTranslationViewModel), result.Model.GetType());
            CreateTranslationViewModel viewModel = (CreateTranslationViewModel)result.Model;
            Assert.IsNotNull(viewModel.originLanguages);
            Assert.IsTrue(viewModel.originLanguages.Count == 3);
            Assert.IsNotNull(viewModel.targetLanguages);
            Assert.IsTrue(viewModel.targetLanguages.Count == 4);
            Assert.IsNotNull(viewModel.German);
            Assert.IsTrue(viewModel.German == 1);
            Assert.IsNotNull(viewModel.EnglishUS);
            Assert.IsTrue(viewModel.EnglishUS == 2);
            Assert.IsNotNull(viewModel.EnglishGB);
            Assert.IsTrue(viewModel.EnglishGB == 3);    
            Assert.IsNotNull(viewModel.English);
            Assert.IsTrue(viewModel.English == 5);
            Assert.IsNotNull(viewModel.Translation);
            Assert.AreEqual("Hello", viewModel.Translation.OriginalText);
            Assert.AreEqual("Hallo", viewModel.Translation.TranslatedText);
        }

        [TestMethod]
        public void History_ReturnsViewResultWithTranslations()
        {
            // Arrange
            var translations = new List<Translation>(){
                new Translation(){
                    ID = 1,
                    OriginalText = "Hello",
                    TranslatedText = "Hallo",
                    OriginalLanguage = new WebApp.Models.Language
                    {
                        Abbreviation = "en",
                        ID = 5,
                        isOriginLanguage = true,
                        isTargetLanguage = false,
                        Name = "English"
                    },
                    TranslatedLanguage = new WebApp.Models.Language
                    {
                        Abbreviation = "de",
                        ID = 1,
                        isOriginLanguage = true,
                        isTargetLanguage = true,
                        Name = "German"
                    }
                },
                new Translation(){
                    ID = 2,
                    OriginalText = "Goodbye",
                    TranslatedText = "Auf Wiedersehen",
                    OriginalLanguage = new WebApp.Models.Language
                    {
                        Abbreviation = "en",
                        ID = 5,
                        isOriginLanguage = true,
                        isTargetLanguage = false,
                        Name = "English"
                    },
                    TranslatedLanguage = new WebApp.Models.Language
                    {
                        Abbreviation = "de",
                        ID = 1,
                        isOriginLanguage = true,
                        isTargetLanguage = true,
                        Name = "German"
                    }
                }
            };
            _translationRepositoryMock.Setup(repo => repo.GetAllTranslations()).Returns(translations);

            // Act
            var result = _controller.History() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(translations, result.Model);
        }
    }
}