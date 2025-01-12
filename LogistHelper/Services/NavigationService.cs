using DTOs;
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
            pages.Add(PageType.CarrierMenu, typeof(MenuPageViewModel<CarrierDto>));
            pages.Add(PageType.ClientMenu, typeof(MenuPageViewModel<CompanyDto>));
            pages.Add(PageType.DriverMenu, typeof(MenuPageViewModel<DriverDto>));
            pages.Add(PageType.VehicleMenu, typeof(MenuPageViewModel<VehicleDto>));
        }
    }
}
