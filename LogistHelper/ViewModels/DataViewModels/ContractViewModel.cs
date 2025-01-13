using DTOs;
using LogistHelper.ViewModels.Base;
using System.Collections.ObjectModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class ContractViewModel : DataViewModel<ContractDto>
    {
        #region Private

        private DriverViewModel _driver;
        private ClientViewModel _client;
        private RoutePointViewModel _loadingPoint;

        private ObservableCollection<RoutePointViewModel> _unloadingPoints;

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
                }
                else
                {
                    _dto.Driver = null;
                }
            }
        }

        public VehicleViewModel Vehicle
        {
            get => _driver?.Vehicle;
            set
            {
                if (_driver != null)
                {
                    _driver.Vehicle = value;
                    OnPropertyChanged(nameof(Vehicle));
                }
            }
        }

        public CarrierViewModel Carrier
        {
            get => _driver?.Carrier;
        }

        public ClientViewModel Client
        {
            get => _client;
            set 
            { 
                SetProperty(ref _client, value);
                if (Client != null)
                {
                    _dto.Client = _client.GetDto();
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


        public ObservableCollection<RoutePointViewModel> UnloadPoints
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

                var unique = UnloadPoints.DistinctBy(p => p.Route);

                foreach (RoutePointViewModel point in unique) 
                {
                    result += $" - {point.Route}";
                }

                return result;
            }
        }

        #endregion Public

        public ContractViewModel(ContractDto dto, int counter) : base(dto, counter)
        {
            if (dto != null) 
            {
                _client = new ClientViewModel(_dto.Client);
                _driver = new DriverViewModel(dto.Driver);
                _loadingPoint = new RoutePointViewModel(dto.LoadPoint);

                if (dto.UnloadPoints != null)
                {
                    UnloadPoints = new ObservableCollection<RoutePointViewModel>(dto.UnloadPoints.Select(p => new RoutePointViewModel(p)));
                }
                else 
                {
                    UnloadPoints = new ObservableCollection<RoutePointViewModel>();
                }
            }
        }

        public ContractViewModel(ContractDto dto) : this(dto, 0) { }

        public ContractViewModel() : base() { }

        public override ContractDto GetDto()
        {
            _dto.UnloadPoints = UnloadPoints.Select(p => p.GetDto()).ToList();
            _dto.Carrier = _dto.Driver.Carrier;
            _dto.Vehicle = _dto.Driver.Vehicle;

            return base.GetDto();
        }

        protected override void DefaultInit()
        {
            _dto = new ContractDto();

            Driver = new DriverViewModel();
            Client = new ClientViewModel();
            LoadPoint = new RoutePointViewModel();
            UnloadPoints = new ObservableCollection<RoutePointViewModel>();
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
