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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITranslationService _translationService;
        private readonly ITranslationRepository _translationRepository;
        private readonly ILanguageRepository _languageReository;

        public HomeController(ILogger<HomeController> logger, ITranslationService translationService, ILanguageRepository languageRepository, ITranslationRepository translationRepository)
        {
            _logger = logger;
            _translationService = translationService;
            _translationRepository = translationRepository;
            _languageReository = languageRepository;
        }

        // GET: Startpage
        public IActionResult Index()
        {
            CreateTranslationViewModel viewModel = CreateTranslationViewModelHandler.createViewModel(_languageReository);
            return View(viewModel);
        }

        // POST: Startpage
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


            CreateTranslationViewModel viewModel = CreateTranslationViewModelHandler.createViewModel(_languageReository);
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

                if (ModelState.IsValid)
                {
                    if (viewModel.Translation is not null)
                    {
                        _translationRepository.AddTranslation(viewModel.Translation);
                    }
                    return View(viewModel);
                }
                else
                {
                    return View(viewModel);
                }
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

        // GET: History
        public IActionResult History()
        {
            return View(_translationRepository.GetAllTranslations());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Home/Details/X
        public IActionResult Details(int? id)
        {
            if (id == null || _translationRepository.TranslationExists(id.Value) == false)
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

        // GET: Home/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _translationRepository.TranslationExists(id.Value) == false)
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

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var translation = _translationRepository.GetTranslationById(id);

            if (translation != null)
            {
                _translationRepository.DeleteTranslation(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}



