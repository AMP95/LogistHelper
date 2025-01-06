﻿using CustomDialog;
using DTOs;
using Log4NetLogger;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
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

            services.AddTransient<MenuPageViewModel<CarrierDto>, CarrierMenuPageViewModel>();
            services.AddTransient<MenuPageViewModel<CompanyDto>, ClientMenuPageViewModel>();
            services.AddTransient<MenuPageViewModel<DriverDto>, DriverMenuViewModel>();

            #endregion Pages

            #region Views

            services.AddTransient<ListViewModel<CarrierDto>, CarrierListViewModel>();
            services.AddTransient<ListViewModel<CompanyDto>, ClientListViewModel>();
            services.AddTransient<ListViewModel<DriverDto>, DriverListViewModel>();

            services.AddTransient<EditViewModel<CarrierDto>, EditCarrierViewModel>();
            services.AddTransient<EditViewModel<CompanyDto>, EditClientViewModel>();
            services.AddTransient<EditViewModel<DriverDto>, EditDriverViewModel>();

            #endregion Views

            #region Factories

            services.AddSingleton<IViewModelFactory<CompanyDto>, ClientViewModelFactory>();
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
