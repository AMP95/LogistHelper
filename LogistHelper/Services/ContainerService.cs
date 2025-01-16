using CustomDialog;
using DTOs;
using Log4NetLogger;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using LogistHelper.ViewModels.Pages;
using LogistHelper.ViewModels.Pages.DataMenu;
using LogistHelper.ViewModels.Views;
using LogistHelper.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace LogistHelper.Services
{
    public static class ContainerService
    {
        public static MainWindowViewModel MainWindowViewModel => Services.GetService<MainWindowViewModel>();
        public static ISettingsRepository<Settings> SettingsRepository => Services.GetService<ISettingsRepository<Settings>>();
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
            services.AddTransient<MainMenuPageViewModel, MainMenuPageViewModel>();
            services.AddTransient<DatabaseMenuPageViewModel, DatabaseMenuPageViewModel>();
            services.AddTransient<SecondContractMenuViewModel, SecondContractMenuViewModel>();
            services.AddTransient<CountractSubMenuViewModel, CountractSubMenuViewModel>();

            services.AddTransient<MainMenuPageViewModel<CarrierDto>, CarrierMenuPageViewModel>();
            services.AddTransient<MainMenuPageViewModel<CompanyDto>, ClientMenuPageViewModel>();
            services.AddTransient<MainMenuPageViewModel<DriverDto>, DriverMenuViewModel>();
            services.AddTransient<MainMenuPageViewModel<VehicleDto>, VehicleMenuViewModel>();
            services.AddTransient<MainMenuPageViewModel<ContractDto>, ContractMenuViewModel>();
            services.AddTransient<SubMenuPageViewModel<DocumentDto>, DocumentMenuViewModel>();
            services.AddTransient<SubMenuPageViewModel<PaymentDto>, PaymentMenuViewModel>();

            #endregion Pages

            #region Views

            services.AddTransient<MainListViewModel<CarrierDto>, CarrierListViewModel>();
            services.AddTransient<MainListViewModel<CompanyDto>, ClientListViewModel>();
            services.AddTransient<MainListViewModel<DriverDto>, DriverListViewModel>();
            services.AddTransient<MainListViewModel<VehicleDto>, VehicleListViewModel>();
            services.AddTransient<MainListViewModel<ContractDto>, ContractListViewModel>();
            services.AddTransient<SubListViewModel<DocumentDto>, DocumentListViewModel>();
            services.AddTransient<SubListViewModel<PaymentDto>, PaymentListViewModel>();

            services.AddTransient<MainEditViewModel<CarrierDto>, EditCarrierViewModel>();
            services.AddTransient<MainEditViewModel<CompanyDto>, EditClientViewModel>();
            services.AddTransient<MainEditViewModel<DriverDto>, EditDriverViewModel>();
            services.AddTransient<MainEditViewModel<VehicleDto>, EditVehicleViewModel>();
            services.AddTransient<MainEditViewModel<ContractDto>, EditContractViewModel>();
            services.AddTransient<SubEditViewModel<DocumentDto>, EditDocumentViewModel>();
            services.AddTransient<SubEditViewModel<PaymentDto>, EditPaymentViewModel>();

            #endregion Views

            #region Factories

            services.AddSingleton<IViewModelFactory<ClientDto>, ClientViewModelFactory>();
            services.AddSingleton<IViewModelFactory<CarrierDto>, CarrierViewModelFactory>();
            services.AddSingleton<IViewModelFactory<DriverDto>, DriverViewModelFactory>();
            services.AddSingleton<IViewModelFactory<VehicleDto>, VehicleViewModelFactory>();
            services.AddSingleton<IViewModelFactory<DocumentDto>, DocumentViewModelFactory>();
            services.AddSingleton<IViewModelFactory<PaymentDto>, PaymentViewModelFactory>();
            services.AddSingleton<IViewModelFactory<RoutePointDto>, RouteViewModelFactory>();
            services.AddSingleton<IViewModelFactory<ContractDto>, ContractViewModelFactory>();

            #endregion Factories

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
