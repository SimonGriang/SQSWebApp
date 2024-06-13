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
        private readonly WebAppContext _context;
        private readonly String authKey = "f2981bee-344a-4a1f-b65f-877950fa3855:fx";
        private Translator translator;

        public TranslationService(WebAppContext context)
        {
            _context = context;
            translator = new Translator(authKey);
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

            if (viewModel.Translation.TranslatedLanguage!.Abbreviation == "DL" )
            {
                translatedText = await translator.TranslateTextAsync(originalText, null, languageTo.Abbreviation);
            }
            else
            {
                translatedText = await translator.TranslateTextAsync(originalText, languageFrom.Abbreviation, languageTo.Abbreviation!);
            }
            viewModel.Translation.TranslatedText = translatedText?.Text;
            viewModel.Translation.translated_at =  DateTime.UtcNow;
            return viewModel;
        }

        public async Task<List<WebApp.Models.Language>> getDeeplLanguages()
        {
            List<WebApp.Models.Language> languages = new List<WebApp.Models.Language>();
            List<WebApp.Models.Language> finallanguages = new List<WebApp.Models.Language>();

            var sourceLanguages = await translator.GetSourceLanguagesAsync();

            foreach (var lang in sourceLanguages)
            {
                WebApp.Models.Language createlan = new WebApp.Models.Language(lang.Name, lang.Code);
                createlan.isOriginLanguage = true;
                languages.Add(createlan);
            }


            var targetLanguages = await translator.GetTargetLanguagesAsync();

            foreach (var lang in targetLanguages)
            {
                bool languageFound = false;

                WebApp.Models.Language createlan = new WebApp.Models.Language(lang.Name, lang.Code);
                foreach (WebApp.Models.Language lan in languages)
                {
                    if (createlan.Abbreviation == (lan.Abbreviation))
                    {
                        lan.isTargetLanguage = true;
                        finallanguages.Add(lan);
                        languageFound = true;
                        break;
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
