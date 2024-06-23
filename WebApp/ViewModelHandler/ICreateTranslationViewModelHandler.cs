using WebApp.ViewModels;

namespace WebApp.ViewModelHandler
{
    public interface ICreateTranslationViewModelHandler
    {       
        /// <summary>
        /// Creates the view model for creating translations.
        /// </summary>
        /// <returns>The created view model.</returns>
        public CreateTranslationViewModel createViewModel();
    }
}
