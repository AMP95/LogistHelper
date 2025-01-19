using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class ContractViewModel : DataViewModel<ContractDto>
    {
        #region Private
        private IViewModelFactory<RoutePointDto> _routeFactory;

        private DriverViewModel _driver;
        private VehicleViewModel _vehicle;
        private CarrierViewModel _carrier;
        private ClientViewModel _client;

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
                OnPropertyChanged(nameof(Profit));
            }
        }

        public float Prepayment
        {
            get => _dto.Prepayment;
            set
            {
                _dto.Prepayment = value;
                OnPropertyChanged(nameof(Prepayment));
                OnPropertyChanged(nameof(Profit));
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

        public ClientViewModel Client
        {
            get => _client;
            set
            {
                SetProperty(ref _client, value);
                if (value != null)
                {
                    _dto.Client = Client.GetDto();
                }
                else
                {
                    _dto.Client = null;
                }
            }
        }

        public RoutePointViewModel LoadPoint
        {
            get => _loadingPoint;
            set
            {
                SetProperty(ref _loadingPoint, value);
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

        public float Profit 
        {
            get => ClientPayment - Payment;
        }

        #endregion Public

        public ICommand AddUnloadCommand { get; }
        public ICommand DeleteUnloadCommand { get; }

        public ContractViewModel(ContractDto dto, int counter, IViewModelFactory<RoutePointDto> routeFactory) : base(dto, counter)
        {
            _routeFactory = routeFactory;

            if (dto != null)
            {
                _driver = new DriverViewModel(dto.Driver);
                _vehicle = new VehicleViewModel(dto.Vehicle);
                _carrier = new CarrierViewModel(dto.Carrier);
                _client = new ClientViewModel(dto.Client);
                _loadingPoint = _routeFactory.GetViewModel(dto.LoadPoint) as RoutePointViewModel;

                if (dto.UnloadPoints != null)
                {
                    UnloadPoints = new ObservableCollection<ListItem<RoutePointViewModel>>(dto.UnloadPoints.Select(p => new ListItem<RoutePointViewModel>()
                    {
                        Item = _routeFactory.GetViewModel(p) as RoutePointViewModel
                    }));
                }
                else
                {
                    ListItem<RoutePointViewModel> item = new ListItem<RoutePointViewModel>(_routeFactory.GetViewModel() as RoutePointViewModel);
                    item.Item.Type = LoadPointType.Download;

                    UnloadPoints = new ObservableCollection<ListItem<RoutePointViewModel>>()
                    {
                        item
                    };
                }
            }
            else 
            {
                LoadPoint = _routeFactory.GetViewModel() as RoutePointViewModel;
                UnloadPoints = new ObservableCollection<ListItem<RoutePointViewModel>>();
            }

            AddUnloadCommand = new RelayCommand(() =>
            {
                ListItem<RoutePointViewModel> item = new ListItem<RoutePointViewModel>(_routeFactory.GetViewModel() as RoutePointViewModel);
                item.Item.Type = LoadPointType.Download;

                UnloadPoints.Add(item);
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

        public ContractViewModel(ContractDto dto, IViewModelFactory<RoutePointDto> routeFactory) : this(dto, 0, routeFactory) { }

        public ContractViewModel(IViewModelFactory<RoutePointDto> routeFactory) : base() 
        {
            _routeFactory = routeFactory;

            AddUnloadCommand = new RelayCommand(() =>
            {
                ListItem<RoutePointViewModel> item = new ListItem<RoutePointViewModel>(_routeFactory.GetViewModel() as RoutePointViewModel);
                item.Item.Type = LoadPointType.Download;

                UnloadPoints.Add(item);
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
            _dto.LoadPoint = LoadPoint.GetDto();
            _dto.UnloadPoints = UnloadPoints.Select(p => p.Item.GetDto()).ToList();
            _dto.Carrier = Carrier.GetDto();
            _dto.Client  = Client.GetDto();
            _dto.Vehicle = Vehicle.GetDto();

            return base.GetDto();
        }

        protected override void DefaultInit()
        {
            _dto = new ContractDto();

            Client = new ClientViewModel();
            Driver = new DriverViewModel();
            
        }
    }

    public class ContractViewModelFactory : IViewModelFactory<ContractDto>
    {
        private IViewModelFactory<RoutePointDto> _routeFactory;

        public ContractViewModelFactory(IViewModelFactory<RoutePointDto> routeFactory)
        {
            _routeFactory = routeFactory;
        }

        public DataViewModel<ContractDto> GetViewModel(ContractDto dto, int number)
        {
            return new ContractViewModel(dto, number, _routeFactory);
        }

        public DataViewModel<ContractDto> GetViewModel(ContractDto dto)
        {
            return new ContractViewModel(dto, _routeFactory);
        }

        public DataViewModel<ContractDto> GetViewModel()
        {
            return new ContractViewModel(_routeFactory);
        }
    }
}
