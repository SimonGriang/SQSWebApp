using DeepL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModelHandler;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    /// <summary>
    /// Represents the controller for the home page and related actions.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ITranslationService _translationService;
        private readonly ITranslationRepository _translationRepository;
        private readonly ILanguageRepository _languageReository;
        private readonly ICreateTranslationViewModelHandler _createTranslationViewModelHandler;
        private readonly String errorMessage = "ErrorMessage";

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="translationService">The translation service.</param>
        /// <param name="languageRepository">The language repository.</param>
        /// <param name="translationRepository">The translation repository.</param>
        /// <param name="createTranslationViewModelHandler">The create translation view model handler.</param>
        public HomeController(ITranslationService translationService, ILanguageRepository languageRepository, ITranslationRepository translationRepository, ICreateTranslationViewModelHandler createTranslationViewModelHandler)
        {
            _translationService = translationService;
            _translationRepository = translationRepository;
            _languageReository = languageRepository;
            _createTranslationViewModelHandler = createTranslationViewModelHandler;
        }

        /// <summary>
        /// Gets the start page.
        /// </summary>
        /// <returns>The start page view.</returns>
        public IActionResult Index()
        {
            // Exception Handling noch einfügen falls bei der Erstellung des ViewModels ein Fehler auftritt
            try
            {
                CreateTranslationViewModel viewModel = _createTranslationViewModelHandler.createViewModel();
                if (viewModel is null || viewModel.originLanguages is null || viewModel.targetLanguages is null || viewModel.originLanguages.Count == 0 || viewModel.targetLanguages.Count == 0)
                {
                    return NotFound();
                }
                return View(viewModel);
            }
            catch (Exception exception)
            {
                TempData[errorMessage] = "Ein unerwarteter Fehler ist aufgetreten: " + exception.Message;
                return View();
            }
        }

        /// <summary>
        /// Handles the post request for the start page.
        /// </summary>
        /// <param name="returnedViewModel">The returned view model.</param>
        /// <returns>The start page view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CreateTranslationViewModel returnedViewModel)
        {
            if (returnedViewModel.LanguageTo == 0 || returnedViewModel.LanguageFrom == 0 || _languageReository.LanguageExists(returnedViewModel.LanguageTo) == false || _languageReository.LanguageExists(returnedViewModel.LanguageFrom) == false)
            {
                ModelState.AddModelError("", "Ungültige Sprachauswahl. Bitte wählen Sie gültige Sprachen aus.");
                return View(returnedViewModel);
            }
            Language? languageTo = _languageReository.GetLanguage(returnedViewModel.LanguageTo);
            Language? languageFrom = _languageReository.GetLanguage(returnedViewModel.LanguageFrom);

            CreateTranslationViewModel viewModel = _createTranslationViewModelHandler.createViewModel();
            viewModel.LanguageFrom = returnedViewModel.LanguageFrom;
            viewModel.LanguageTo = returnedViewModel.LanguageTo;

            if (viewModel.Translation is not null)
            {
                viewModel.Translation.OriginalLanguage = languageFrom;
                viewModel.Translation.TranslatedLanguage = languageTo;
                if (returnedViewModel.Translation is not null)
                    viewModel.Translation.OriginalText = returnedViewModel.Translation.OriginalText;
            }
            else
            {
                ModelState.AddModelError("", "Geben Sie Text für die Übersetzung ein.");
                return View(returnedViewModel);
            }

            try
            {
                viewModel = await _translationService.TranslateTextAsync(viewModel);

                if (ModelState.IsValid && viewModel.Translation is not null)
                {
                    _translationRepository.AddTranslation(viewModel.Translation);
                }
                throw new Exception("Modelstate ist not valid or Translation is null.");
            }
            catch (ConnectionException connectionException)
            {
                TempData["ErrorMessage"] = "Es konnte keine Verbindung zum Webservice aufgerufen werden: " + connectionException.Message;
                return View();
            }
            catch (QuotaExceededException quotaExceededException)
            {
                TempData["ErrorMessage"] = "Das Kontigent an möglichen Übersetzungen der Software ist ereicht: " + quotaExceededException.Message;
                return View(viewModel);
            }
            catch (DeepLException deeplException)
            {
                TempData["ErrorMessage"] = "Fehlerhafte Sprachkombination angegeben: " + deeplException.Message;
                return View(viewModel);
            }
            catch (Exception exception)
            {
                TempData["ErrorMessage"] = "Ein unerwarteter Fehler ist aufgetreten: " + exception.Message;
                return View(viewModel);
            }
        }

        /// <summary>
        /// Gets the history page.
        /// </summary>
        /// <returns>The history page view.</returns>
        public IActionResult History()
        {
            return View(_translationRepository.GetAllTranslations());
        }

        /// <summary>
        /// Handles the request for details of a translation.
        /// </summary>
        /// <param name="id">The translation ID.</param>
        /// <returns>The details view of the translation.</returns>
        public IActionResult Details(int? id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            if (id == null || !_translationRepository.TranslationExists(id.Value))
            {
                return NotFound();
            }
            var translation = _translationRepository.GetTranslationById(id.Value);

            if (translation == null)
            {
                return NotFound();
            }

            return View(translation);
        }

        /// <summary>
        /// Handles the request to delete a translation.
        /// </summary>
        /// <param name="id">The translation ID.</param>
        /// <returns>The delete view of the translation.</returns>
        public IActionResult Delete(int? id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            if (id is null || !_translationRepository.TranslationExists(id.Value))
            {
                return NotFound();
            }

            var translation = _translationRepository.GetTranslationById(id.Value);

            if (translation == null)
            {
                return NotFound();
            }

            return View(translation);
        }

        /// <summary>
        /// Handles the post request to delete a translation.
        /// </summary>
        /// <param name="id">The translation ID.</param>
        /// <returns>The history page view.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var translation = _translationRepository.GetTranslationById(id);

            if (translation != null)
            {
                _translationRepository.DeleteTranslation(id);
                return RedirectToAction(nameof(History));
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Handles the error page.
        /// </summary>
        /// <returns>The error page view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}



