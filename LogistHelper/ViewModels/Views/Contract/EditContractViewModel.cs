using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using ServerClient;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    class EditContractViewModel : EditViewModel<ContractDto>
    {
        #region Private

        private ContractViewModel _contract;

        private IEnumerable<DriverViewModel> _drivers;
        private DriverViewModel _selectedDriver;

        private ObservableCollection<VehicleViewModel> _vehicles;
        private int _selectedVehicleIndex;

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



        #endregion Public


        #region Commands

        public ICommand SearchDriverCommand { get; }

        #endregion Commands

        public EditContractViewModel(ISettingsRepository<Settings> repository, 
                                     IViewModelFactory<ContractDto> factory, 
                                     IDialog dialog) : base(repository, factory, dialog)
        {

            #region CommandsInit

            SearchDriverCommand = new RelayCommand<string>(async(searchString) => 
            { 
                await SearchDriver(searchString);
            });

            #endregion CommandsInit
        }

        public override async Task Load(Guid id)
        {
            if (id == Guid.Empty)
            {
                EditedViewModel = _factory.GetViewModel();
                _contract = EditedViewModel as ContractViewModel;

                _contract.CreationDate = DateTime.Now;
                _contract.Number = (short)(AppSettings.Default.lastContractNumer + 1);

                if (AppSettings.Default.lastContractDate.Year != DateTime.Now.Year) 
                {
                    _contract.Number = 1;
                }
            }
            else
            {
                IsBlock = true;
                BlockText = "Загрузка";

                RequestResult<ContractDto> result = await _client.GetId<ContractDto>(id);

                if (result.IsSuccess)
                {
                    EditedViewModel = _factory.GetViewModel(result.Result);
                    _contract = EditedViewModel as ContractViewModel;
                    SelectedDriver = _contract.Driver;
                }

                IsBlock = false;
            }
        }

        public override bool CheckSave()
        {
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

            RequestResult<bool> result;

            CreateContractDocument();

            if (EditedViewModel.Id == Guid.Empty)
            {
                result = await _client.Add(EditedViewModel.GetDto());
            }
            else
            {
                result = await _client.Update(EditedViewModel.GetDto());
            }

            if (result.IsSuccess)
            {
                AppSettings.Default.lastContractNumer = _contract.Number;
                AppSettings.Default.lastContractDate = _contract.CreationDate;
                AppSettings.Default.Save();

                SendContractToCarrier();

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
                RequestResult<DriverDto> result = await _client.GetId<DriverDto>(id);

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
                RequestResult<IEnumerable<DriverDto>> result = await _client.Search<DriverDto>(searchString);

                Drivers = null;

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

        private async Task CreateContractDocument() 
        { 
        
        }

        private async Task SendContractToCarrier() 
        {
            _dialog.ShowInfo("Заявка отправлена перевозчику", "Новая заявка");
        }
    }
}
