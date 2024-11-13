using CommunityToolkit.Mvvm.Input;
using LogistHelper.Models;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Pages
{
    public class MenuPageViewModel : BasePageViewModel
    {
        public ICommand NavigateCarrierMenuCommand { get; }

        public MenuPageViewModel()
        {
            NavigateCarrierMenuCommand = new RelayCommand(() => 
            {
                NavigationService.Navigate(PageType.CarrierMenu);
            });
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(PageType.Enter);
        }
    }
}
