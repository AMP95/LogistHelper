﻿using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    class EditContractViewModel : MainEditViewModel<ContractDto>
    {
        #region Private

        private ContractViewModel _contract;

        private IEnumerable<DriverViewModel> _drivers;
        private DriverViewModel _selectedDriver;

        private IEnumerable<ClientViewModel> _clients;

        private ObservableCollection<VehicleViewModel> _vehicles;
        private int _selectedVehicleIndex;

        private bool _sendToCarrier;
        private bool _print;

        #endregion Private

        #region Public

        public IEnumerable<DriverViewModel> Drivers 
        {
            get => _drivers;
            set => SetProperty(ref _drivers, value);
        }

        public DriverViewModel SelectedDriver 
        {
            get => _selectedDriver;
            set 
            {
                SetProperty(ref _selectedDriver, value);
                if (value != null)
                {
                    LoadDriverData(value.Id);
                }
                else 
                { 
                    LoadDriverData(Guid.Empty);
                }
            }
        }

        public IEnumerable<ClientViewModel> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        public ObservableCollection<VehicleViewModel> Vehicles
        {
            get => _vehicles;
            set => SetProperty(ref _vehicles, value);
        }

        public int SelectedVehicleIndex
        {
            get => _selectedVehicleIndex;
            set
            {
                SetProperty(ref _selectedVehicleIndex, value);
                if (_contract != null)
                {
                    if (value >= 0)
                    {
                        _contract.Vehicle = Vehicles.ElementAt(value) as VehicleViewModel;
                    }
                    else
                    {
                        _contract.Vehicle = null;
                    }
                }
            }
        }

        public bool Send 
        {
            get => _sendToCarrier;
            set => SetProperty(ref _sendToCarrier, value);
        }

        public bool Print
        {
            get => _print;
            set => SetProperty(ref _print, value);
        }

        #endregion Public


        #region Commands

        public ICommand SearchDriverCommand { get; }
        public ICommand SearchClientCommand { get; }

        #endregion Commands

        public EditContractViewModel(IDataAccess repository, 
                                     IViewModelFactory<ContractDto> factory, 
                                     IDialog dialog) : base(repository, factory, dialog)
        {

            #region CommandsInit

            SearchDriverCommand = new RelayCommand<string>(async(searchString) => 
            { 
                await SearchDriver(searchString);
            });

            SearchClientCommand = new RelayCommand<string>(async(searchString) => 
            {
                await SearchClient(searchString);
            });

            #endregion CommandsInit
        }

       

        public override async Task Load(Guid id)
        {
            if (id == Guid.Empty)
            {
                EditedViewModel = _factory.GetViewModel();
                _contract = EditedViewModel as ContractViewModel;

                DateTime date = DateTime.Now;
                int month = date.Month;

                _contract.CreationDate = date;
                _contract.Number = 1;

                IAccessResult<IEnumerable<ContractDto>> result;
                
                for (int i = month; i >= 1; i--) 
                { 
                    int days = DateTime.DaysInMonth(date.Year, month);

                    result = await _access.GetFilteredAsync<ContractDto>(nameof(ContractDto.CreationDate),
                                                                         new DateTime(date.Year, month, 1).ToString(),
                                                                         new DateTime(date.Year, month, days).ToString());

                    if (result.IsSuccess && result.Result.Any()) 
                    {
                        _contract.Number = (short)(result.Result.OrderByDescending(x => x.CreationDate).FirstOrDefault().Number + 1);
                        break;
                    }
                }

                Print = true;
                Send = true;
            }
            else
            {
                IsBlock = true;
                BlockText = "Загрузка";

                IAccessResult<ContractDto> result = await _access.GetIdAsync<ContractDto>(id);

                if (result.IsSuccess)
                {
                    EditedViewModel = _factory.GetViewModel(result.Result);
                    _contract = EditedViewModel as ContractViewModel;
                    _selectedDriver = _contract.Driver;
                    OnPropertyChanged(nameof(SelectedDriver));

                    var vehResult = await _access.GetFilteredAsync<VehicleDto>("CarrierId", _contract.Carrier.Id.ToString());

                    if (vehResult.IsSuccess)
                    {
                        Vehicles = new ObservableCollection<VehicleViewModel>(vehResult.Result.Select(v => new VehicleViewModel(v)));
                        SelectedVehicleIndex = Vehicles.IndexOf(Vehicles.FirstOrDefault(v => v.Id == _contract.Vehicle.Id));
                    }
                }

                IsBlock = false;

                Print = true;
                Send = false;
            }
        }

        public override bool CheckSave()
        {
            if (_contract.Status == ContractStatus.Failed) 
            {
                _dialog.ShowError("Нельзя сохранить данные сорваной заявки", "Сохранение");
                return false;
            }
            if (_contract.Volume <= 0 || _contract.Weight <= 0)
            {
                _dialog.ShowError("Необходимо указать вес и объем груза", "Сохранение");
                return false;
            }
            if (_contract.Payment <= 0)
            {
                _dialog.ShowError("Необходимо указать стоимость услуг перевозчика", "Сохранение");
                return false;
            }
            if (_contract.Driver == null)
            {
                _dialog.ShowError("Необходимо выбрать водителя", "Сохранение");
                return false;
            }
            if (_contract.Vehicle == null) 
            {
                _dialog.ShowError("Необходимо выбрать транспортное средство", "Сохранение");
                return false;
            }
            if (!_contract.LoadPoint.CheckValidation()) 
            {
                _dialog.ShowError("Необходимо полностью заполнить данные погрузки", "Сохранение");
                return false;
            }
            if (_contract.UnloadPoints.Any(p => !p.Item.CheckValidation())) 
            {
                _dialog.ShowError("Необходимо полностью заполнить данные выгрузки", "Сохранение");
                return false;
            }
            return true;
        }

        public override async Task Save()
        {
            IsBlock = true;
            BlockText = "Сохранение";

            bool result = false;

            CreateContractDocument();

            if (Print)
            {
                PrintContract();
            }

            if (EditedViewModel.Id == Guid.Empty)
            {
                IAccessResult<Guid> addResult = await _access.AddAsync(EditedViewModel.GetDto());
                if (addResult.IsSuccess) 
                {
                    result = true;
                    EditedViewModel.Id = addResult.Result;
                }
            }
            else
            {
                IAccessResult<bool> updateResult = await _access.UpdateAsync(EditedViewModel.GetDto());
                result = updateResult.IsSuccess;
            }

            if (result)
            {
                if (Send)
                {
                    SendContractToCarrier();
                }

                BackCommand.Execute(this);
            }
            else
            {
                _dialog.ShowError("Не удалось сохранить изменения", "Сохранение");
            }

            IsBlock = false;
        }

        private async Task LoadDriverData(Guid id)
        {
            if (id == Guid.Empty) 
            {
                _contract.Driver = null;
                Vehicles = null;
                SelectedVehicleIndex = -1;
                return;
            }

            await Task.Run(async () =>
            {
                IAccessResult<DriverDto> result = await _access.GetIdAsync<DriverDto>(id);

                if (result.IsSuccess)
                {
                    _contract.Driver = new DriverViewModel(result.Result);
                    Vehicles = new ObservableCollection<VehicleViewModel>(result.Result.Carrier.Vehicles.Select(v => new VehicleViewModel(v)));
                    SelectedVehicleIndex = Vehicles.IndexOf(Vehicles.FirstOrDefault(v => v.Id == _contract.Driver.Vehicle?.Id));
                }
            });
        }

        private async Task SearchDriver(string searchString) 
        {
            await Task.Run(async () =>
            {
                IAccessResult<IEnumerable<DriverDto>> result = await _access.GetFilteredAsync<DriverDto>(nameof(DriverDto.Name), searchString);

                if (result.IsSuccess)
                {
                    Drivers = result.Result.Select(d => new DriverViewModel(d));
                }
                else
                {
                    Drivers = null;
                }
            });
        }

        private async Task SearchClient(string searchString)
        {
            await Task.Run(async () =>
            {
                IAccessResult<IEnumerable<ClientDto>> result = await _access.GetFilteredAsync<ClientDto>(nameof(ClientDto.Name), searchString);

                if (result.IsSuccess)
                {
                    Clients = result.Result.Select(d => new ClientViewModel(d));
                }
                else
                {
                    Clients = null;
                }
            });
        }

        private async Task CreateContractDocument() 
        { 
        
        }
        private async Task PrintContract()
        {

        }

        private async Task SendContractToCarrier() 
        {
            _dialog.ShowInfo("Заявка отправлена перевозчику", "Новая заявка");
        }
    }
}
