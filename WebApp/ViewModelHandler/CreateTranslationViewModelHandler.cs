using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.ViewModelHandler
{
    /// <summary>
    /// Handles the creation of the view model for creating translations.
    /// </summary>
    public class CreateTranslationViewModelHandler : ICreateTranslationViewModelHandler
    {
        private readonly ILanguageRepository _languageRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTranslationViewModelHandler"/> class.
        /// </summary>
        /// <param name="languageRepository">The language repository.</param>
        public CreateTranslationViewModelHandler(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        /// <summary>
        /// Creates the view model for creating translations.
        /// </summary>
        /// <returns>The created view model.</returns>
        public CreateTranslationViewModel createViewModel()
        {
            var viewModel = new CreateTranslationViewModel
            {
                Translation = new Translation()
            };

            var allLanguages = _languageRepository.GetAllLanguages();
            var originLanguages = new List<Language>();
            var targetLanguages = new List<Language>();

            foreach (var lan in allLanguages)
            {
                ProcessTargetLanguage(lan, viewModel, targetLanguages);
                ProcessOriginLanguage(lan, viewModel, originLanguages);
            }

            viewModel.originLanguages = originLanguages;
            viewModel.targetLanguages = targetLanguages;

            return viewModel;
        }

        /// <summary>
        /// Processes all target languages Includes certain target languages from the properties
        /// </summary>
        /// <returns>The created view model.</returns>
        private static void ProcessTargetLanguage(Language lan, CreateTranslationViewModel viewModel, List<Language> targetLanguages)
        {
            if (lan.IsTargetLanguage)
            {
                targetLanguages.Add(lan);
                switch (lan.Abbreviation)
                {
                    case "de":
                        viewModel.German = lan.ID;
                        break;
                    case "en-US":
                        viewModel.EnglishUS = lan.ID;
                        break;
                    case "en-GB":
                        viewModel.EnglishGB = lan.ID;
                        break;
                }
            }
        }

        /// <summary>
        /// Processes all source languages Includes certain source languages from the properties
        /// </summary>
        /// <returns>The created view model.</returns>

        private static void ProcessOriginLanguage(Language lan, CreateTranslationViewModel viewModel, List<Language> originLanguages)
        {
            if (lan.IsOriginLanguage)
            {
                originLanguages.Add(lan);
                switch (lan.Abbreviation)
                {
                    case "DL":
                        viewModel.DetectLanguage = lan.ID;
                        break;
                    case "en":
                        viewModel.English = lan.ID;
                        break;
                }
            }
        }

    }
}
