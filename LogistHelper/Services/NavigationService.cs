﻿using LogistHelper.Models;
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
            pages.Add(PageType.Menu, typeof(MenuPageViewModel));
        }
    }
}
