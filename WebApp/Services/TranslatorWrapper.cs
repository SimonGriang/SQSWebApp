using DeepL;
using DeepL.Model;


namespace WebApp.Services
{
    public class TranslatorWrapper : ITranslatorWrapper
    {
        private readonly Translator _translator;

        public TranslatorWrapper(string authKey)
        {
            _translator = new Translator(authKey);
        }

        public async Task<TextResult> TranslateTextAsync(string text, string sourceLang, string targetLang)
        {
            return await _translator.TranslateTextAsync(text, sourceLang, targetLang);
        }

        public async Task<SourceLanguage[]> GetSourceLanguagesAsync()
        {
            return await _translator.GetSourceLanguagesAsync();
        }

        public async Task<TargetLanguage[]> GetTargetLanguagesAsync()
        {
            return await _translator.GetTargetLanguagesAsync();
        }
    }
}