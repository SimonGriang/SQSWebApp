using WebApp.ViewModels;

public interface ITranslationService
        {
            Task<List<WebApp.Models.Language>> getDeeplLanguages();
            Task<CreateTranslationViewModel> TranslateTextAsync(CreateTranslationViewModel viewModel);
        }
