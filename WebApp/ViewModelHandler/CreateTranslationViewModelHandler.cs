using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.ViewModelHandler
{
    public class CreateTranslationViewModelHandler : ICreateTranslationViewModelHandler
    {
        private readonly ILanguageRepository _languageRepository;

        public CreateTranslationViewModelHandler(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

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

        private void ProcessTargetLanguage(Language lan, CreateTranslationViewModel viewModel, List<Language> targetLanguages)
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

        private void ProcessOriginLanguage(Language lan, CreateTranslationViewModel viewModel, List<Language> originLanguages)
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
