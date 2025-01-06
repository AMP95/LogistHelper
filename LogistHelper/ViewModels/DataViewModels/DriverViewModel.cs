using CommunityToolkit.Mvvm.ComponentModel;
using DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class DriverViewModel : ObservableObject, IDataErrorInfo
    {
        #region Private

        private DriverDto _driver;

        private TruckViewModel _truck;
        private TrailerViewModel _trailer;
        private CarrierViewModel _carrier;
        private ObservableCollection<StringItem> _phones;

        #endregion Private

        #region Public

        public Guid Id
        {
            get => _driver.Id;
        }

        public string Name
        {
            get => _driver.Name;
            set
            {
                _driver.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public DateTime BirthDate
        {
            get => _driver.BirthDate;
            set
            {
                _driver.BirthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }
        public string PassportSerial
        {
            get => _driver.PassportSerial;
            set
            {
                _driver.PassportSerial = value;
                OnPropertyChanged(nameof(PassportSerial));
            }
        }
        public string PassportIssuer
        {
            get => _driver.PassportIssuer;
            set
            {
                _driver.PassportIssuer = value;
                OnPropertyChanged(nameof(PassportIssuer));
            }
        }
        public DateTime PassportDateOfIssue
        {
            get => _driver.PassportDateOfIssue;
            set
            {
                _driver.PassportDateOfIssue = value;
                OnPropertyChanged(nameof(PassportDateOfIssue));
            }
        }
        public TruckViewModel Truck
        {
            get => _truck;
            set 
            { 
                SetProperty(ref _truck, value);
                if (value != null)
                {
                    _driver.Truck = (TruckDto)Truck.GetDto();
                }
                else 
                {
                    _driver.Truck = null;
                }
            }
        }

        public TrailerViewModel Trailer
        {
            get => _trailer;
            set 
            { 
                SetProperty(ref _trailer, value);
                if (value != null)
                {
                    _driver.Trailer = (TrailerDto)Trailer.GetDto();
                }
                else 
                {
                    _driver.Trailer = null;
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
                    _driver.Carrier = (CarrierDto)Carrier.GetDto();
                }
                else
                {
                    _driver.Carrier = null;
                }
            }
        }


        public ObservableCollection<StringItem> Phones
        {
            get => _phones;
            set => SetProperty(ref _phones, value);
        }

        #endregion Public

        #region Validation

        public string this[string columnName] => _driver[columnName];

        public string Error => _driver.Error;

        #endregion Validation

        public DriverViewModel(DriverDto route)
        {
            _driver = route;
            _truck = new TruckViewModel(_driver.Truck);
            _trailer = new TrailerViewModel(_driver.Trailer);
            _carrier = new CarrierViewModel(_driver.Carrier);

            Phones = new ObservableCollection<StringItem>(route.Phones.Select(s => new StringItem(s)));
        }

        public DriverViewModel()
        {
            _driver = new DriverDto();

            Truck = new TruckViewModel();
            Trailer = new TrailerViewModel();
            Carrier = new CarrierViewModel();

            Phones = new ObservableCollection<StringItem>();
        }

        public DriverDto GetDto()
        {
            _driver.Phones = _phones.Select(s => s.Item).ToList();
            return _driver;
        }
    }
}
