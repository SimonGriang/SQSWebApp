using DeepL.Model;

public interface ITranslatorWrapper
{
    public Task<TextResult> TranslateTextAsync(string text, string sourceLang, string targetLang);
    public Task<SourceLanguage[]> GetSourceLanguagesAsync();
    public Task<TargetLanguage[]> GetTargetLanguagesAsync();
}