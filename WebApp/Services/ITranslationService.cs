using WebApp.ViewModels;

namespace WebApp.Services
{
    /// <summary>
    /// Represents a translation service that provides methods for translating text.
    /// </summary>
    public interface ITranslationService
    {
        /// <summary>
        /// Retrieves a list of supported languages from the DeepL translation service.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of supported languages.</returns>
        Task<List<WebApp.Models.Language>> getDeeplLanguages();

        /// <summary>
        /// Translates the text provided in the specified view model using the DeepL translation service.
        /// </summary>
        /// <param name="viewModel">The view model containing the text to be translated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the translated text.</returns>
        Task<CreateTranslationViewModel> TranslateTextAsync(CreateTranslationViewModel viewModel);
    }
}
