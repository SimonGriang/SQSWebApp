using DeepL.Model;

namespace WebApp.Services
{
    /// <summary>
    /// Represents a translator wrapper interface.
    /// </summary>
    public interface ITranslatorWrapper
    {
        /// <summary>
        /// Translates the specified text from the source language to the target language asynchronously.
        /// </summary>
        /// <param name="text">The text to be translated.</param>
        /// <param name="sourceLang">The source language of the text.</param>
        /// <param name="targetLang">The target language for translation.</param>
        /// <returns>A task that represents the asynchronous translation operation. The task result contains the translated text.</returns>
        public Task<TextResult> TranslateTextAsync(string text, string sourceLang, string targetLang);

        /// <summary>
        /// Retrieves the available source languages asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an array of source languages.</returns>
        public Task<SourceLanguage[]> GetSourceLanguagesAsync();

        /// <summary>
        /// Retrieves the available target languages asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an array of target languages.</returns>
        public Task<TargetLanguage[]> GetTargetLanguagesAsync();
    }
}