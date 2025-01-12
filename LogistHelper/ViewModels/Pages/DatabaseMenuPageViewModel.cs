using CommunityToolkit.Mvvm.Input;
using LogistHelper.Models;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Pages
{
    public class DatabaseMenuPageViewModel : BasePageViewModel
    {
        public ICommand NavigateCarrierMenuCommand { get; }
        public ICommand NavigateClientMenuCommand { get; }
        public ICommand NavigateDriverMenuCommand { get; }
        public ICommand NavigateVehicleMenuCommand { get; }
        public ICommand NavigateRoutesMenuCommand { get; }

        public DatabaseMenuPageViewModel()
        {

            NavigateCarrierMenuCommand = new RelayCommand(() =>
            {
                NavigationService.Navigate(PageType.CarrierMenu);
            });

            NavigateClientMenuCommand = new RelayCommand(() =>
            {
                NavigationService.Navigate(PageType.ClientMenu);
            });

            NavigateDriverMenuCommand = new RelayCommand(() =>
            {
                NavigationService.Navigate(PageType.DriverMenu);
            });

            NavigateVehicleMenuCommand = new RelayCommand(() =>
            {
                NavigationService.Navigate(PageType.VehicleMenu);
            });
        }
        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(PageType.MainMenu);
        }
    }
}
