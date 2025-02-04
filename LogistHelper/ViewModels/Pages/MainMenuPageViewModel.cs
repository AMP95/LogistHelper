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
        public ICommand NavigateCompanySettingsCommand { get; }

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
                NavigationService.Navigate(PageType.PaymentMenu);
            });

            NavigateDatabaseMenuCommand = new RelayCommand(() =>
            {
                NavigationService.Navigate(PageType.DatabaseMenu);
            });

            NavigateAdminCommand = new RelayCommand(() =>
            {
                NavigationService.Navigate(PageType.LogistMenu);
            });

            NavigateSettingsCommand = new RelayCommand(() =>
            {
                NavigationService.Navigate(PageType.SettingsPage);
            });
            NavigateCompanySettingsCommand = new RelayCommand(() =>
            {
                NavigationService.Navigate(PageType.CompanySettingPage);
            });
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(PageType.Enter);
        }
    }
}
