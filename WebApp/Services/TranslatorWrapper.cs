using DeepL;
using DeepL.Model;


namespace WebApp.Services
{
    /// <summary>
    /// Wrapper class for the Translator service.
    /// </summary>
    public class TranslatorWrapper : ITranslatorWrapper
    {
        private readonly Translator _translator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorWrapper"/> class.
        /// </summary>
        /// <param name="authKey">The authentication key for the Translator service.</param>
        public TranslatorWrapper(string authKey)
        {
            _translator = new Translator(authKey);
        }

        /// <summary>
        /// Translates the specified text from the source language to the target language.
        /// </summary>
        /// <param name="text">The text to be translated.</param>
        /// <param name="sourceLang">The source language of the text.</param>
        /// <param name="targetLang">The target language for the translation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains the translated text.</returns>
        public async Task<TextResult> TranslateTextAsync(string text, string sourceLang, string targetLang)
        {
            return await _translator.TranslateTextAsync(text, sourceLang, targetLang);
        }

        /// <summary>
        /// Retrieves the available source languages for translation.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains an array of <see cref="SourceLanguage"/> objects.</returns>
        public async Task<SourceLanguage[]> GetSourceLanguagesAsync()
        {
            return await _translator.GetSourceLanguagesAsync();
        }

        /// <summary>
        /// Retrieves the available target languages for translation.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains an array of <see cref="TargetLanguage"/> objects.</returns>
        public async Task<TargetLanguage[]> GetTargetLanguagesAsync()
        {
            return await _translator.GetTargetLanguagesAsync();
        }
    }
}