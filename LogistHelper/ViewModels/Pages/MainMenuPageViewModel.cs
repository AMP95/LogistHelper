using CommunityToolkit.Mvvm.Input;
using LogistHelper.Models;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Pages
{
    public class MainMenuPageViewModel : BasePageViewModel
    {
        public ICommand NewContractCommand { get; }
        public ICommand NavigateContractsMenuCommand { get; }
        public ICommand NavigateBillsMenuCommand { get; }
        public ICommand NavigateDatabaseMenuCommand { get; }
        public ICommand NavigateAdminCommand { get; }
        public ICommand NavigateSettingsCommand { get; }

        public MainMenuPageViewModel()
        {
            NewContractCommand = new RelayCommand(() => 
            {
                NavigationService.Navigate(PageType.NewContract);
            });

            NavigateContractsMenuCommand = new RelayCommand(() =>
            {
                NavigationService.Navigate(PageType.ContractMenu);
            });

            NavigateBillsMenuCommand = new RelayCommand(() => 
            {
                
            });

            NavigateDatabaseMenuCommand = new RelayCommand(() =>
            {
                NavigationService.Navigate(PageType.DatabaseMenu);
            });

            NavigateAdminCommand = new RelayCommand(() =>
            {
                
            });

            NavigateSettingsCommand = new RelayCommand(() =>
            {
                
            });
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(PageType.Enter);
        }
    }
}
