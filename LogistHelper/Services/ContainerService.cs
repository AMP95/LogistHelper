﻿using CustomDialog;
using DTOs;
using DTOs.Dtos;
using HelpAPIs;
using HelpAPIs.Settings;
using Log4NetLogger;
using LogistHelper.Models;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using LogistHelper.ViewModels.Pages;
using LogistHelper.ViewModels.Views;
using Microsoft.Extensions.DependencyInjection;
using Models.Suggest;
using Models.Sugget;
using Shared;
using Utilities;

namespace LogistHelper.Services
{
    public static class ContainerService
    {
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

            services.AddTransient<IMainMenuPage<CarrierDto>, CarrierMenuPageViewModel>();
            services.AddTransient<IMainMenuPage<ClientDto>, ClientMenuPageViewModel>();
            services.AddTransient<IMainMenuPage<DriverDto>, DriverMenuViewModel>();
            services.AddTransient<IMainMenuPage<VehicleDto>, VehicleMenuViewModel>();
            services.AddTransient<IMainMenuPage<ContractDto>, ContractMenuViewModel>();
            services.AddTransient<ISubMenuPage<DocumentDto>, DocumentMenuViewModel>();
            services.AddTransient<ISubMenuPage<PaymentDto>, PaymentMenuViewModel>();

            #endregion Pages

            #region Views

            services.AddTransient<IMainListView<CarrierDto>, CarrierListViewModel>();
            services.AddTransient<IMainListView<ClientDto>, ClientListViewModel>();
            services.AddTransient<IMainListView<DriverDto>, DriverListViewModel>();
            services.AddTransient<IMainListView<VehicleDto>, VehicleListViewModel>();
            services.AddTransient<IMainListView<ContractDto>, ContractListViewModel>();
            services.AddTransient<ISubListView<DocumentDto>, DocumentListViewModel>();
            services.AddTransient<ISubListView<PaymentDto>, PaymentListViewModel>();

            services.AddTransient<IMainEditView<CarrierDto>, EditCarrierViewModel>();
            services.AddTransient<IMainEditView<ClientDto>, EditClientViewModel>();
            services.AddTransient<IMainEditView<DriverDto>, EditDriverViewModel>();
            services.AddTransient<IMainEditView<VehicleDto>, EditVehicleViewModel>();
            services.AddTransient<IMainEditView<ContractDto>, EditContractViewModel>();
            services.AddTransient<ISubEditView<DocumentDto>, EditDocumentViewModel>();
            services.AddTransient<ISubEditView<PaymentDto>, EditPaymentViewModel>();

            #endregion Views

            #region Factories

            services.AddTransient<IViewModelFactory<ClientDto>, ClientViewModelFactory>();
            services.AddTransient<IViewModelFactory<CarrierDto>, CarrierViewModelFactory>();
            services.AddTransient<IViewModelFactory<DriverDto>, DriverViewModelFactory>();
            services.AddTransient<IViewModelFactory<VehicleDto>, VehicleViewModelFactory>();
            services.AddTransient<IViewModelFactory<DocumentDto>, DocumentViewModelFactory>();
            services.AddTransient<IViewModelFactory<PaymentDto>, PaymentViewModelFactory>();
            services.AddTransient<IViewModelFactory<RoutePointDto>, RouteViewModelFactory>();
            services.AddTransient<IViewModelFactory<ContractDto>, ContractViewModelFactory>();
            services.AddTransient<IViewModelFactory<FileDto>, FileViewModelFactory>();

            #endregion Factories

            #region Models


            #endregion Models

            #region Services

            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<ISettingsRepository<ApiSettings>, ApiJsonRepository>();
            services.AddSingleton<IDialog, CustomDialogService>();

            services.AddTransient<IDataSuggest<CompanySuggestItem>, DaDataClient>();
            services.AddTransient<IDataSuggest<FmsSuggestItem>, DaDataClient>();
            services.AddTransient<IDataSuggest<TruckModelSuggestItem>, VehicleSuggestClient>();
            services.AddTransient<IDataSuggest<TrailerModelSuggestItem>, VehicleSuggestClient>();
            services.AddTransient<IDataSuggest<GeoSuggestItem>, YandexGeoSuggestClient>();
            services.AddTransient<IDataAccess, ServerClient>();
            services.AddTransient<IFileLoader<FileViewModel>, FileManager>();

            #endregion Services

            return services.BuildServiceProvider();
        }
    }
}
