using CustomDialog;
using Log4NetLogger;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Pages;
using LogistHelper.ViewModels.Views;
using LogistHelper.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace LogistHelper.Services
{
    public static class ContainerService
    {
        public static MainWindowViewModel MainWindowViewModel => Services.GetService<MainWindowViewModel>();
        public static ILogger Logger => Services.GetService<ILogger>();

        public static IServiceProvider Services { get; private set; }

        public static void ConfigureServices()
        {
            Services = AddServices();
        }

        private static IServiceProvider AddServices()
        {
            ServiceCollection services = new ServiceCollection();


            #region Pages

            services.AddTransient<EnterPageViewModel, EnterPageViewModel>();
            services.AddTransient<MenuPageViewModel, MenuPageViewModel>();
            services.AddTransient<CarrierMenuViewModel, CarrierMenuViewModel>();

            #endregion Pages

            #region Views

            services.AddTransient<CarrierListViewModel, CarrierListViewModel>();
            services.AddTransient<EditCarrierViewModel, EditCarrierViewModel>();


            #endregion Views

            #region Models


            #endregion Models

            #region Services

            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<ISettingsRepository<Settings>, JsonRepository>();
            services.AddSingleton<IDialog, CustomDialogService>();

            #endregion Services

            return services.BuildServiceProvider();
        }
    }
}
