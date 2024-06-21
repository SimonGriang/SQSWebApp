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
        private readonly ITranslatorWrapper _translator;

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
                || viewModel.Translation.TranslatedLanguage.Abbreviation is null
                || viewModel.Translation.OriginalLanguage.Abbreviation is null)
            {
                throw new ArgumentNullException("Translationkomponenten sind nicht vollständig.");
            }

            TextResult translatedText;
            WebApp.Models.Language languageTo = viewModel.Translation.TranslatedLanguage;
            WebApp.Models.Language languageFrom = viewModel.Translation.OriginalLanguage;
            string originalText = viewModel.Translation.OriginalText!;

            if (viewModel.Translation.OriginalLanguage!.Abbreviation == "DL" )
            {
                translatedText = await _translator.TranslateTextAsync(originalText, null!, languageTo.Abbreviation);
            }
            else
            {
                translatedText = await _translator.TranslateTextAsync(originalText, languageFrom.Abbreviation, languageTo.Abbreviation);
            }
            viewModel.Translation.TranslatedText = translatedText.Text;
            viewModel.Translation.translated_at =  DateTime.UtcNow;
            return viewModel;
        }

        public async Task<List<WebApp.Models.Language>> getDeeplLanguages()
        {
            List<WebApp.Models.Language> languagesTarget = new List<WebApp.Models.Language>();
            List<WebApp.Models.Language> languagesSource = new List<WebApp.Models.Language>();
            List<WebApp.Models.Language> finallanguages = new List<WebApp.Models.Language>();


            var sourceLanguages = await _translator.GetSourceLanguagesAsync();
            foreach (var lang in sourceLanguages)
            {
                WebApp.Models.Language createlan = new WebApp.Models.Language(lang.Name, lang.Code);
                createlan.isOriginLanguage = true;
                createlan.isTargetLanguage = false;
                languagesSource.Add(createlan);
            }


            var targetLanguages = await _translator.GetTargetLanguagesAsync();
            foreach (var lang in targetLanguages)
            {
                WebApp.Models.Language createlan = new WebApp.Models.Language(lang.Name, lang.Code);
                createlan.isOriginLanguage = true;
                createlan.isTargetLanguage = false;
                languagesTarget.Add(createlan);
            }

            foreach (WebApp.Models.Language language in languagesSource)
            {
                if (languagesTarget.Exists(l => l.Abbreviation == language.Abbreviation)){
                    language.isTargetLanguage = true;
                    language.isOriginLanguage = true;
                    finallanguages.Add(language);
                } else {
                    language.isOriginLanguage = true;
                    language.isTargetLanguage = false;
                    finallanguages.Add(language);
                }
            }

            foreach (WebApp.Models.Language language in languagesTarget)
            {
                if (!finallanguages.Exists(l => l.Abbreviation == language.Abbreviation)){
                    language.isOriginLanguage = false;
                    language.isTargetLanguage = true;
                    finallanguages.Add(language);
                } 
            }
            return finallanguages;
        }
    }
}
