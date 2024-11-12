using LogistHelper.Models;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class MenuPageViewModel : BasePageViewModel
    {
        public MenuPageViewModel()
        {
            
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(PageType.Enter);
        }
    }
}
