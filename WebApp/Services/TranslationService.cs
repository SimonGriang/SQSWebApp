using System;
using System.Threading.Tasks;
using DeepL;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels;
using WebApp.ViewModelHandler;
using DeepL.Model;

namespace WebApp.Services
{
    public class TranslationService : ITranslationService
    {
        private ITranslatorWrapper _translator;

        public TranslationService(ITranslatorWrapper translatorWrapper)
        {
            _translator = translatorWrapper;
        }

        public async Task<CreateTranslationViewModel> TranslateTextAsync(CreateTranslationViewModel viewModel)
        {
            if (viewModel.Translation is null 
                || viewModel.Translation.OriginalLanguage is null 
                || viewModel.Translation.TranslatedLanguage is null 
                || viewModel.Translation.OriginalText is null
                || viewModel.Translation.TranslatedLanguage.Abbreviation is null)
            {
                throw new ArgumentNullException(nameof(viewModel.Translation));
            }

            TextResult? translatedText = null;
            WebApp.Models.Language languageTo = viewModel.Translation.TranslatedLanguage!;
            WebApp.Models.Language languageFrom = viewModel.Translation.OriginalLanguage!;
            string originalText = viewModel.Translation.OriginalText!;

            if (viewModel.Translation.OriginalLanguage!.Abbreviation == "DL" )
            {
                translatedText = await _translator.TranslateTextAsync(originalText, null, languageTo.Abbreviation);
            }
            else
            {
                translatedText = await _translator.TranslateTextAsync(originalText, languageFrom.Abbreviation, languageTo.Abbreviation!);
            }
            viewModel.Translation.TranslatedText = translatedText?.Text;
            viewModel.Translation.translated_at =  DateTime.UtcNow;
            return viewModel;
        }

        public async Task<List<WebApp.Models.Language>> getDeeplLanguages()
        {
            List<WebApp.Models.Language> languages = new List<WebApp.Models.Language>();
            List<WebApp.Models.Language> finallanguages = new List<WebApp.Models.Language>();

            var sourceLanguages = await _translator.GetSourceLanguagesAsync();

            foreach (var lang in sourceLanguages)
            {
                WebApp.Models.Language createlan = new WebApp.Models.Language(lang.Name, lang.Code);
                createlan.isOriginLanguage = true;
                languages.Add(createlan);
            }


            var targetLanguages = await _translator.GetTargetLanguagesAsync();

            foreach (var lang in targetLanguages)
            {
                bool languageFound = false;

                WebApp.Models.Language createlan = new WebApp.Models.Language(lang.Name, lang.Code);
                foreach (WebApp.Models.Language lan in languages)
                {
                    if (createlan.Abbreviation == lan.Abbreviation)
                    {
                        lan.isTargetLanguage = true;
                        lan.isOriginLanguage = true;
                        finallanguages.Add(lan);
                        languageFound = true;
                        break;
                    } else {
                        if (finallanguages.Contains(lan) == false)
                        {
                            lan.isOriginLanguage = true;
                            lan.isTargetLanguage = false;
                            languageFound = false;
                            finallanguages.Add(lan);
                        }
                    }
                }
                if (languageFound == false)
                {
                    createlan.isOriginLanguage = false;
                    createlan.isTargetLanguage = true;
                    finallanguages.Add(createlan);
                }
            }
            return finallanguages;
        }
    }
}
