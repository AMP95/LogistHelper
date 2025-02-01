using DTOs;
using DTOs.Dtos;
using LogistHelper.Models;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.Pages;
using System.Windows.Controls;

namespace LogistHelper.Services
{
    public static class NavigationService
    {
        private static Dictionary<PageType, Type> pages = new Dictionary<PageType, Type>();

        public static ContentControl ContentControl { get; set; }

        public static void Navigate(PageType page)
        {
            if (ContentControl != null)
            {
                ContentControl.Content = ContainerService.Services.GetService(pages[page]);
            }
        }

        public static void RegisterPages()
        {
            pages.Add(PageType.Enter, typeof(EnterPageViewModel));
            pages.Add(PageType.MainMenu, typeof(MainMenuPageViewModel));
            pages.Add(PageType.DatabaseMenu, typeof(DatabaseMenuPageViewModel));
            pages.Add(PageType.CarrierMenu, typeof(IMainMenuPage<CarrierDto>));
            pages.Add(PageType.ClientMenu, typeof(IMainMenuPage<ClientDto>));
            pages.Add(PageType.DriverMenu, typeof(IMainMenuPage<DriverDto>));
            pages.Add(PageType.VehicleMenu, typeof(IMainMenuPage<VehicleDto>));
            pages.Add(PageType.ContractMenu, typeof(IMainMenuPage<ContractDto>));
            pages.Add(PageType.TemplateMenu, typeof(IMainMenuPage<ContractTemplateDto>));
            pages.Add(PageType.LogistMenu, typeof(IMainMenuPage<LogistDto>));
            pages.Add(PageType.NewContract, typeof(SecondContractMenuViewModel));
            pages.Add(PageType.PaymentMenu, typeof(PaymentPageViewModel));
            pages.Add(PageType.SettingsPage, typeof(SettingsPageViewModel));
        }
    }
}
