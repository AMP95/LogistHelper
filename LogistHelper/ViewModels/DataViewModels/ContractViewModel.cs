using CommunityToolkit.Mvvm.ComponentModel;
using DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class ContractViewModel : ObservableObject, IDataErrorInfo
    {
        #region Private

        private ContractDto _contract;

        private DriverViewModel _driver;
        private RoutePointViewModel _loadingPoint;

        private ObservableCollection<RoutePointViewModel> _unloadingPoints;

        #endregion Private

        #region Public

        public Guid Id
        {
            get => _contract.Id;
        }

        public short Number
        {
            get => _contract.Number;
            set
            {
                _contract.Number = value;
                OnPropertyChanged(nameof(Number));
            }
        }
        public DateTime CreationDate
        {
            get => _contract.CreationDate;
            set
            {
                _contract.CreationDate = value;
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        public ContractStatus Status
        {
            get => _contract.Status;
            set
            {
                _contract.Status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public float Volume
        {
            get => _contract.Volume;
            set
            {
                _contract.Volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }
        public float Weight
        {
            get => _contract.Weight;
            set
            {
                _contract.Weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        public float Payment
        {
            get => _contract.Payment;
            set
            {
                _contract.Payment = value;
                OnPropertyChanged(nameof(Payment));
            }
        }

        public float Prepayment
        {
            get => _contract.Prepayment;
            set
            {
                _contract.Prepayment = value;
                OnPropertyChanged(nameof(Prepayment));
            }
        }
        public RecievingType PaymentCondition
        {
            get => _contract.PaymentCondition;
            set
            {
                _contract.PaymentCondition = value;
                OnPropertyChanged(nameof(PaymentCondition));
            }
        }

        public PaymentPriority PayPriority
        {
            get => _contract.PayPriority;
            set
            {
                _contract.PayPriority = value;
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
                    _contract.Driver = Driver.GetDto();
                }
                else
                {
                    _contract.Driver = null;
                }
            }
        }

        public TruckViewModel Truck
        {
            get => _driver?.Truck;
            set
            {
                if (_driver != null)
                {
                    _driver.Truck = value;
                    OnPropertyChanged(nameof(Truck));
                }
            }
        }

        public TrailerViewModel Trailer
        {
            get => _driver?.Trailer;
            set
            {
                if (_driver != null)
                {
                    _driver.Trailer = value;
                    OnPropertyChanged(nameof(Trailer));
                }
            }
        }

        public CarrierViewModel Carrier
        {
            get => _driver?.Carrier;
        }

        public RoutePointViewModel LoadPoint
        {
            get => _loadingPoint;
            set
            {
                SetProperty(ref _loadingPoint, value);
                if (_loadingPoint != null)
                {
                    _contract.LoadPoint = LoadPoint.GetDto();
                }
                else 
                {
                    _contract.LoadPoint = null;
                }
            }
        }


        public ObservableCollection<RoutePointViewModel> UnloadPoints
        {
            get => _unloadingPoints;
            set => SetProperty(ref _unloadingPoints, value);
        }

        #endregion Public

        #region Validation

        public string this[string columnName] => _contract[columnName];

        public string Error => _contract.Error;

        #endregion Validation

        public ContractViewModel(ContractDto route)
        {
            _contract = route;

            _driver = new DriverViewModel(_contract.Driver);
            _loadingPoint = new RoutePointViewModel(_contract.LoadPoint);
            UnloadPoints = new ObservableCollection<RoutePointViewModel>(_contract.UnloadPoints.Select(p => new RoutePointViewModel(p)));
        }

        public ContractViewModel()
        {
            _contract = new ContractDto();
            Driver = new DriverViewModel();
            LoadPoint = new RoutePointViewModel();
            UnloadPoints = new ObservableCollection<RoutePointViewModel>();
        }

        public ContractDto GetDto()
        {
            _contract.UnloadPoints = UnloadPoints.Select(p => p.GetDto()).ToList();
            _contract.Carrier = _contract.Driver.Carrier;
            _contract.Trailer = _contract.Driver.Trailer;
            _contract.Truck = _contract.Driver.Truck;

            return _contract;
        }
    }
}
