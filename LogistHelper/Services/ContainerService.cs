using CustomDialog;
using DTOs;
using Log4NetLogger;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.DataViewModels;
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
            services.AddTransient<CompanyMenuViewModel<CarrierDto>, CarrierMenuPageViewModel>();
            services.AddTransient<CompanyMenuViewModel<CompanyDto>, ClientMenuPageViewModel>();

            #endregion Pages

            #region Views

            services.AddTransient<CompanyListViewModel<CarrierDto>, CarrierListViewModel>();
            services.AddTransient<CompanyListViewModel<CompanyDto>, ClientListViewModel>();
            services.AddTransient<EditCompanyViewModel<CarrierDto>, EditCarrierViewModel>();
            services.AddTransient<EditCompanyViewModel<CompanyDto>, EditClientViewModel>();

            #endregion Views

            #region Models


            #endregion Models

            #region Services

            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<ISettingsRepository<Settings>, JsonRepository>();
            services.AddSingleton<IDialog, CustomDialogService>();

            services.AddSingleton<ICompanyVmFactory<CompanyDto>, ClientVmFactory>();
            services.AddSingleton<ICompanyVmFactory<CarrierDto>, CarrierVmFactory>();

            #endregion Services

            return services.BuildServiceProvider();
        }
    }
}
