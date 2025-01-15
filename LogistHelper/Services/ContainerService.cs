using CustomDialog;
using DTOs;
using Log4NetLogger;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.Base.Interfaces;
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

            services.AddTransient<MenuPageViewModel<CarrierDto>, CarrierMenuPageViewModel>();
            services.AddTransient<MenuPageViewModel<ClientDto>, ClientMenuPageViewModel>();
            services.AddTransient<MenuPageViewModel<DriverDto>, DriverMenuViewModel>();
            services.AddTransient<MenuPageViewModel<VehicleDto>, VehicleMenuViewModel>();
            services.AddTransient<MenuPageViewModel<ContractDto>, ContractMenuViewModel>();
            services.AddTransient<SecondContractMenuViewModel, SecondContractMenuViewModel>();

            #endregion Pages

            #region Views

            services.AddTransient<IMainListView<CarrierDto>, CarrierListViewModel>();
            services.AddTransient<IMainListView<ClientDto>, ClientListViewModel>();
            services.AddTransient<IMainListView<DriverDto>, DriverListViewModel>();
            services.AddTransient<IMainListView<VehicleDto>, VehicleListViewModel>();
            services.AddTransient<IMainListView<ContractDto>, ContractListViewModel>();
            services.AddTransient<ISubListView<DocumentDto>, DocumentListViewModel>();

            services.AddTransient<IMainEditView<CarrierDto>, EditCarrierViewModel>();
            services.AddTransient<IMainEditView<ClientDto>, EditClientViewModel>();
            services.AddTransient<IMainEditView<DriverDto>, EditDriverViewModel>();
            services.AddTransient<IMainEditView<VehicleDto>, EditVehicleViewModel>();
            services.AddTransient<IMainEditView<ContractDto>, EditContractViewModel>();
            services.AddTransient<ISubEditView<DocumentDto>, EditDocumentViewModel>();

            #endregion Views

            #region Factories

            services.AddSingleton<IViewModelFactory<ClientDto>, ClientViewModelFactory>();
            services.AddSingleton<IViewModelFactory<CarrierDto>, CarrierViewModelFactory>();
            services.AddSingleton<IViewModelFactory<DriverDto>, DriverViewModelFactory>();
            services.AddSingleton<IViewModelFactory<VehicleDto>, VehicleViewModelFactory>();
            services.AddSingleton<IViewModelFactory<DocumentDto>, DocumentViewModelFactory>();
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
