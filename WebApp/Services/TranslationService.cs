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
    /// <summary>
    /// Service class for translating text.
    /// </summary>
    public class TranslationService : ITranslationService
    {
        private readonly ITranslatorWrapper _translator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationService"/> class.
        /// </summary>
        /// <param name="translatorWrapper">The translator wrapper.</param>
        public TranslationService(ITranslatorWrapper translatorWrapper)
        {
            _translator = translatorWrapper;
        }

        /// <summary>
        /// Translates the text asynchronously.
        /// </summary>
        /// <param name="viewModel">The view model containing the translation data.</param>
        /// <returns>The updated view model with translated text.</returns>
        public async Task<CreateTranslationViewModel> TranslateTextAsync(CreateTranslationViewModel viewModel)
        {
            // Check if all translation components are provided
            if (viewModel.Translation is null 
                || viewModel.Translation.OriginalLanguage is null 
                || viewModel.Translation.TranslatedLanguage is null 
                || viewModel.Translation.OriginalText is null
                || viewModel.Translation.TranslatedLanguage.Abbreviation is null
                || viewModel.Translation.OriginalLanguage.Abbreviation is null)
            {
                throw new ArgumentNullException("Translation","Translationkomponenten sind nicht vollständig.");
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
            viewModel.Translation.Translated_at =  DateTime.UtcNow;
            return viewModel;
        }

        /// <summary>
        /// Retrieves the list of supported languages for translation.
        /// </summary>
        /// <returns>The list of supported languages.</returns>
        public async Task<List<WebApp.Models.Language>> getDeeplLanguages()
        {
            List<WebApp.Models.Language> languagesTarget = new List<WebApp.Models.Language>();
            List<WebApp.Models.Language> languagesSource = new List<WebApp.Models.Language>();
            List<WebApp.Models.Language> finallanguages = new List<WebApp.Models.Language>();

            // Get source languages
            var sourceLanguages = await _translator.GetSourceLanguagesAsync();
            foreach (var lang in sourceLanguages)
            {
                WebApp.Models.Language createlan = new WebApp.Models.Language(lang.Name, lang.Code);
                createlan.IsOriginLanguage = true;
                createlan.IsTargetLanguage = false;
                languagesSource.Add(createlan);
            }

            // Get target languages
            var targetLanguages = await _translator.GetTargetLanguagesAsync();
            foreach (var lang in targetLanguages)
            {
                WebApp.Models.Language createlan = new WebApp.Models.Language(lang.Name, lang.Code);
                createlan.IsOriginLanguage = true;
                createlan.IsTargetLanguage = false;
                languagesTarget.Add(createlan);
            }

            // Combine source and target languages
            foreach (WebApp.Models.Language language in languagesSource)
            {
                if (languagesTarget.Exists(l => l.Abbreviation == language.Abbreviation)){
                    language.IsTargetLanguage = true;
                    language.IsOriginLanguage = true;
                    finallanguages.Add(language);
                } else {
                    language.IsOriginLanguage = true;
                    language.IsTargetLanguage = false;
                    finallanguages.Add(language);
                }
            }

            // Add remaining target languages
            foreach (WebApp.Models.Language language in languagesTarget)
            {
                if (!finallanguages.Exists(l => l.Abbreviation == language.Abbreviation)){
                    language.IsOriginLanguage = false;
                    language.IsTargetLanguage = true;
                    finallanguages.Add(language);
                } 
            }
            return finallanguages;
        }
    }
}
