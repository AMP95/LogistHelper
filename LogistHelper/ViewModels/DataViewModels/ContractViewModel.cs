using CommunityToolkit.Mvvm.Input;
using Dadata.Model;
using DTOs;
using LogistHelper.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class ContractViewModel : DataViewModel<ContractDto>
    {
        #region Private

        private DriverViewModel _driver;
        private VehicleViewModel _vehicle;
        private CarrierViewModel _carrier;

        private RoutePointViewModel _loadingPoint;

        private ObservableCollection<ListItem<RoutePointViewModel>> _unloadingPoints;

        #endregion Private

        #region Public

        public short Number
        {
            get => _dto.Number;
            set
            {
                _dto.Number = value;
                OnPropertyChanged(nameof(Number));
            }
        }
        public DateTime CreationDate
        {
            get => _dto.CreationDate;
            set
            {
                _dto.CreationDate = value;
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        public ContractStatus Status
        {
            get => _dto.Status;
            set
            {
                _dto.Status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public float Volume
        {
            get => _dto.Volume;
            set
            {
                _dto.Volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }
        public float Weight
        {
            get => _dto.Weight;
            set
            {
                _dto.Weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        public float Payment
        {
            get => _dto.Payment;
            set
            {
                _dto.Payment = value;
                OnPropertyChanged(nameof(Payment));
            }
        }

        public float ClientPayment
        {
            get => _dto.ClientPayment;
            set
            {
                _dto.ClientPayment = value;
                OnPropertyChanged(nameof(ClientPayment));
            }
        }

        public float Prepayment
        {
            get => _dto.Prepayment;
            set
            {
                _dto.Prepayment = value;
                OnPropertyChanged(nameof(Prepayment));
            }
        }
        public RecievingType PaymentCondition
        {
            get => _dto.PaymentCondition;
            set
            {
                _dto.PaymentCondition = value;
                OnPropertyChanged(nameof(PaymentCondition));
            }
        }

        public PaymentPriority PayPriority
        {
            get => _dto.PayPriority;
            set
            {
                _dto.PayPriority = value;
                OnPropertyChanged(nameof(PayPriority));
            }
        }

        public DriverViewModel Driver
        {
            get => _driver;
            set
            {
                SetProperty(ref _driver, value);
                if (value != null)
                {
                    _dto.Driver = Driver.GetDto();
                    Carrier = Driver.Carrier;
                    Vehicle = Driver.Vehicle;
                }
                else
                {
                    _dto.Driver = null;
                    Carrier = null;
                    Vehicle = null;
                }
            }
        }

        public VehicleViewModel Vehicle
        {
            get => _vehicle;
            set
            {
                SetProperty(ref _vehicle, value);
                if (value != null)
                {
                    _dto.Vehicle = Vehicle.GetDto();
                }
                else
                {
                    _dto.Vehicle = null;
                }
            }
        }

        public CarrierViewModel Carrier
        {
            get => _carrier;
            set
            {
                SetProperty(ref _carrier, value);
                if (value != null)
                {
                    _dto.Carrier = Carrier.GetDto();
                }
                else
                {
                    _dto.Carrier = null;
                }
            }
        }

        public RoutePointViewModel LoadPoint
        {
            get => _loadingPoint;
            set
            {
                SetProperty(ref _loadingPoint, value);
                if (_loadingPoint != null)
                {
                    _dto.LoadPoint = LoadPoint.GetDto();
                }
                else 
                {
                    _dto.LoadPoint = null;
                }
                OnPropertyChanged(nameof(Route));
            }
        }


        public ObservableCollection<ListItem<RoutePointViewModel>> UnloadPoints
        {
            get => _unloadingPoints;
            set
            {
                SetProperty(ref _unloadingPoints, value);
                OnPropertyChanged(nameof(Route));
            }
        }

        public string Route 
        {
            get 
            { 
                string result = LoadPoint?.Route;

                var unique = UnloadPoints.DistinctBy(p => p.Item.Route);

                foreach (ListItem<RoutePointViewModel> point in unique) 
                {
                    result += $" - {point.Item.Route}";
                }

                return result;
            }
        }

        #endregion Public

        public ICommand AddUnloadCommand { get; }
        public ICommand DeleteUnloadCommand { get; }

        public ContractViewModel(ContractDto dto, int counter) : base(dto, counter)
        {
            if (dto != null) 
            {
                _driver = new DriverViewModel(dto.Driver);
                _loadingPoint = new RoutePointViewModel(dto.LoadPoint);

                if (dto.UnloadPoints != null)
                {
                    UnloadPoints = new ObservableCollection<ListItem<RoutePointViewModel>>(dto.UnloadPoints.Select(p => new ListItem<RoutePointViewModel>() { Item = new RoutePointViewModel(p)}));
                }
                else 
                {
                    UnloadPoints = new ObservableCollection<ListItem<RoutePointViewModel>>();
                }
            }

            AddUnloadCommand = new RelayCommand(() => 
            {
                UnloadPoints.Add(new ListItem<RoutePointViewModel>() { Item = new RoutePointViewModel() { Type = LoadPointType.Download } });
            });

            DeleteUnloadCommand = new RelayCommand<Guid>((id) => 
            {
                ListItem<RoutePointViewModel> item = UnloadPoints.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    UnloadPoints.Remove(item);
                }
            });
        }

        public ContractViewModel(ContractDto dto) : this(dto, 0) { }

        public ContractViewModel() : base() 
        {
            AddUnloadCommand = new RelayCommand(() =>
            {
                UnloadPoints.Add(new ListItem<RoutePointViewModel>() { Item = new RoutePointViewModel() { Type = LoadPointType.Download } });
            });

            DeleteUnloadCommand = new RelayCommand<Guid>((id) =>
            {
                ListItem<RoutePointViewModel> item = UnloadPoints.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    UnloadPoints.Remove(item);
                }
            });
        }

        public override ContractDto GetDto()
        {
            _dto.UnloadPoints = UnloadPoints.Select(p => p.Item.GetDto()).ToList();
            _dto.Carrier = _dto.Driver.Carrier;
            _dto.Vehicle = _dto.Driver.Vehicle;

            return base.GetDto();
        }

        protected override void DefaultInit()
        {
            _dto = new ContractDto();

            Driver = new DriverViewModel();
            LoadPoint = new RoutePointViewModel();
            UnloadPoints = new ObservableCollection<ListItem<RoutePointViewModel>>();
        }
    }

    public class ContractViewModelFactory : IViewModelFactory<ContractDto>
    {
        public DataViewModel<ContractDto> GetViewModel(ContractDto dto, int number)
        {
            return new ContractViewModel(dto, number);
        }

        public DataViewModel<ContractDto> GetViewModel(ContractDto dto)
        {
            return new ContractViewModel(dto);
        }

        public DataViewModel<ContractDto> GetViewModel()
        {
            return new ContractViewModel();
        }
    }
}
