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
        private readonly ITranslationService _translationService;
        private readonly ITranslationRepository _translationRepository;
        private readonly ILanguageRepository _languageReository;
        private readonly ICreateTranslationViewModelHandler _createTranslationViewModelHandler;
        private readonly String errorMessage = "ErrorMessage";

        public HomeController(ITranslationService translationService, ILanguageRepository languageRepository, ITranslationRepository translationRepository, ICreateTranslationViewModelHandler createTranslationViewModelHandler)
        {
            _translationService = translationService;
            _translationRepository = translationRepository;
            _languageReository = languageRepository;
            _createTranslationViewModelHandler = createTranslationViewModelHandler;
        }

        // GET: Startpage
        public IActionResult Index()
        { // Exception Handling noch einfügen falls bei der Erstellung des ViewModels ein Fehler auftritt
            try{
                CreateTranslationViewModel viewModel = _createTranslationViewModelHandler.createViewModel();
                if (viewModel is null || viewModel.originLanguages is null || viewModel.targetLanguages is null || viewModel.originLanguages.Count == 0 || viewModel.targetLanguages.Count == 0)
                {
                    return NotFound();
                }
                return View(viewModel);
            } catch (Exception exception){
                TempData[errorMessage] = "Ein unerwarteter Fehler ist aufgetreten: " + exception.Message;
                return View();
            }  
        }

        // POST: Startpage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CreateTranslationViewModel returnedViewModel)
        {
            if (returnedViewModel.LanguageTo == 0 || returnedViewModel.LanguageFrom == 0 || _languageReository.LanguageExists(returnedViewModel.LanguageTo)|| _languageReository.LanguageExists(returnedViewModel.LanguageFrom))
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
                    return View(viewModel);
                }
                throw new ArgumentException("Modelstate ist not valid or Translation is null.");
            }
            catch (ConnectionException connectionException)
            {
                TempData[errorMessage] = "Es konnte keine Verbindung zum Webservice aufgerufen werden: " + connectionException.Message;
                return View();
            }
            catch (QuotaExceededException quotaExceededException)
            {
                TempData[errorMessage] = "Das Kontigent an möglichen Übersetzungen der Software ist ereicht: " + quotaExceededException.Message;
                return View(viewModel);
            }
            catch (DeepLException deeplException)
            {
                TempData[errorMessage] = "Fehlerhafte Sprachkombination angegeben: " + deeplException.Message;
                return View(viewModel);
            }
            catch (Exception exception)
            {
                TempData[errorMessage] = "Ein unerwarteter Fehler ist aufgetreten: " + exception.Message;
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
            if (ModelState.IsValid)
            {
                return NotFound();
            }
            if (id == null || _translationRepository.TranslationExists(id.Value))
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
            if (ModelState.IsValid)
            {
                return NotFound();
            }
            if (id == null || _translationRepository.TranslationExists(id.Value))
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
            if (ModelState.IsValid)
            {
                return NotFound();
            }
            var translation = _translationRepository.GetTranslationById(id);

            if (translation != null)
            {
                _translationRepository.DeleteTranslation(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}



