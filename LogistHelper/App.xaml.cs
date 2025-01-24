using LogistHelper.Services;
using System.Text;
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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }

}
