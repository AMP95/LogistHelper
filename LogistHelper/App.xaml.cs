using LogistHelper.Services;
using System.Windows;

namespace LogistHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ContainerService.ConfigureServices();
            NavigationService.RegisterPages();
            ContainerService.Logger.Log(message: "APP START");
        }
    }

}
