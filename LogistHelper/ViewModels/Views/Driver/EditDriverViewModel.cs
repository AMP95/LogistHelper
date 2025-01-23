using CommunityToolkit.Mvvm.Input;
using Dadata;
using Dadata.Model;
using DTOs;
using LogistHelper.Models;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Models.Suggest;
using Shared;
using System.Runtime;
using System.Windows.Input;
using Utilities;

namespace LogistHelper.ViewModels.Views
{
    public class EditDriverViewModel : MainEditViewModel<DriverDto>
    {
        private DriverViewModel _driver;

        private IDataSuggest<FmsSuggestItem> _dataSuggest;
        private List<DataViewModel<VehicleDto>> _vehicles;
        private int _selectedIndex;

        private IEnumerable<DataViewModel<CarrierDto>> _carriers;
        private DataViewModel<CarrierDto> _selectedCarrier;

        private IEnumerable<FmsSuggestItem> _issuersList;
        private FmsSuggestItem _selectedIssuer;

        private IViewModelFactory<CarrierDto> _carrierFactory;
        private IViewModelFactory<VehicleDto> _vehicleFactory;


        #region Public

        public IEnumerable<DataViewModel<CarrierDto>> Carriers 
        {
            get => _carriers;
            set => SetProperty(ref _carriers, value);
        }

        public List<DataViewModel<VehicleDto>> Vehicles
        {
            get => _vehicles;
            set => SetProperty(ref _vehicles, value);
        }

        public int SelectedVehicleIndex
        {
            get => _selectedIndex;
            set
            {
                SetProperty(ref _selectedIndex, value);
                if (_driver != null) 
                {
                    if (value >= 0)
                    {
                        _driver.Vehicle = Vehicles.ElementAt(value) as VehicleViewModel;
                    }
                    else 
                    {
                        _driver.Vehicle = null;
                    }
                }
            }
        }

        public IEnumerable<FmsSuggestItem> IssuersList
        {
            get => _issuersList;
            set => SetProperty(ref _issuersList, value);
        }

        public FmsSuggestItem SelectedIssuer 
        { 
            get => _selectedIssuer;
            set
            {
                SetProperty(ref _selectedIssuer, value);
                if (SelectedIssuer != null)
                {
                    _driver.PassportIssuer = SelectedIssuer.Name;
                }
                else
                {
                    _driver.PassportIssuer = string.Empty;
                }
            }
        }

        public DataViewModel<CarrierDto> SelectedCarrier 
        {
            get => _selectedCarrier;
            set 
            { 
                SetProperty(ref _selectedCarrier, value);
                _driver.Carrier = SelectedCarrier as CarrierViewModel;
                SearchVehicles();
            }
        }

        #endregion Public



        #region Commands

        public ICommand DeletePhoneCommand { get; set; }
        public ICommand AddPhoneCommand { get; set; }

        public ICommand SearchIssuerCommand { get; set; }
        public ICommand SearchCarrierCommand { get; set; }

        #endregion Commands

        public EditDriverViewModel(IDataAccess repository, 
                                   IViewModelFactory<DriverDto> factory, 
                                   IViewModelFactory<CarrierDto> carrierFactory, 
                                   IViewModelFactory<VehicleDto> vehicleFactory, 
                                   IDialog dialog,
                                   IDataSuggest<FmsSuggestItem> dataSuggest) : base(repository, factory, dialog)
        {
            _carrierFactory = carrierFactory;
            _vehicleFactory = vehicleFactory;
            _dataSuggest = dataSuggest;

            #region CommandsInit

            AddPhoneCommand = new RelayCommand(() =>
            {
                _driver.Phones.Add(new ListItem<string>());
            });

            DeletePhoneCommand = new RelayCommand<Guid>((id) =>
            {
                ListItem<string> item = _driver.Phones.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    _driver.Phones.Remove(item);
                }
            });

            SearchIssuerCommand = new RelayCommand<string>(async (searchString) =>
            {
                IssuersList = await _dataSuggest.SuggestAsync(searchString);
            });

            SearchCarrierCommand = new RelayCommand<string>(async (searchString) => 
            {
                await SearchCarrier(searchString);
            });

            #endregion CommandsInit
        }

        public override async Task Load(Guid id)
        {
            await base.Load(id);

            if (EditedViewModel != null)
            {
                _driver = EditedViewModel as DriverViewModel;
                SelectedCarrier = _driver.Carrier;
            }
        }

        public override void Clear()
        {
            base.Clear();
            SelectedCarrier = null;
            SelectedVehicleIndex = -1;
        }

        private async Task SearchCarrier(string searchString)
        {
            await Task.Run(async () => 
            {
                IAccessResult<IEnumerable<CarrierDto>> result = await _access.GetFilteredAsync<CarrierDto>(nameof(DriverDto.Carrier), searchString);

                SelectedCarrier = null;

                if (result.IsSuccess)
                {
                    Carriers = result.Result.Select(c => _carrierFactory.GetViewModel(c));
                }
                else 
                {
                    Carriers = null;
                }
            });
        }

        private async Task SearchVehicles() 
        {
            await Task.Run(async () =>
            {
                IAccessResult<IEnumerable<VehicleDto>> result = await _access.GetFilteredAsync<VehicleDto>("CarrierId", SelectedCarrier?.Id.ToString());

                Vehicles = null;

                if (result.IsSuccess)
                {
                    Vehicles = result.Result.Select(v => _vehicleFactory.GetViewModel(v)).ToList();
                    SelectedVehicleIndex = Vehicles.IndexOf(Vehicles.FirstOrDefault(v => v.Id == _driver.Vehicle?.Id));
                }
            });
        }

        public override bool CheckSave()
        {
            if (string.IsNullOrWhiteSpace(_driver.Name)) 
            {
                _dialog.ShowError("Необходимо указать имя");
                return false;
            }
            if (_driver.BirthDate == DateTime.MinValue)
            {
                _dialog.ShowError("Необходимо указать дату рождения");
                return false;
            }
            if (string.IsNullOrWhiteSpace(_driver.PassportSerial))
            {
                _dialog.ShowError("Необходимо указать серию и номер паспорта");
                return false;
            }
            if (string.IsNullOrWhiteSpace(_driver.PassportIssuer))
            {
                _dialog.ShowError("Необходимо указать орган выдачи паспорта");
                return false;
            }
            if (_driver.PassportDateOfIssue == DateTime.MinValue)
            {
                _dialog.ShowError("Необходимо указать дату выдачи паспорта");
                return false;
            }
            if (!_driver.Phones.Any())
            {
                _dialog.ShowError("Необходимо указать телефон для связи");
                return false;
            }
            if (_driver.Phones.Any(p => string.IsNullOrWhiteSpace(p.Item))) 
            {
                _dialog.ShowError("Необходимо заполнить все номера");
                return false;
            }
            return true;
        }
    }
}
