using DTOs;
using LogistHelper.ViewModels.Base;
using System.Collections.ObjectModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class DriverViewModel : DataViewModel<DriverDto>
    {
        #region Private

        private VehicleViewModel _vehicle;
        private CarrierViewModel _carrier;
        private ObservableCollection<ListItem<string>> _phones;

        #endregion Private

        #region Public

        public string Name
        {
            get => _dto.Name;
            set
            {
                _dto.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public DateTime BirthDate
        {
            get => _dto.BirthDate;
            set
            {
                _dto.BirthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }
        public string PassportSerial
        {
            get => _dto.PassportSerial;
            set
            {
                _dto.PassportSerial = value;
                OnPropertyChanged(nameof(PassportSerial));
            }
        }
        public string PassportIssuer
        {
            get => _dto.PassportIssuer;
            set
            {
                _dto.PassportIssuer = value;
                OnPropertyChanged(nameof(PassportIssuer));
            }
        }
        public DateTime PassportDateOfIssue
        {
            get => _dto.PassportDateOfIssue;
            set
            {
                _dto.PassportDateOfIssue = value;
                OnPropertyChanged(nameof(PassportDateOfIssue));
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
                    _dto.Vehicle = _vehicle.GetDto();
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


        public ObservableCollection<ListItem<string>> Phones
        {
            get => _phones;
            set => SetProperty(ref _phones, value);
        }

        #endregion Public

        public DriverViewModel(DriverDto dto, int counter) : base(dto, counter)
        {
            if (dto != null)
            {
                _vehicle = new VehicleViewModel(dto.Vehicle);
                _carrier = new CarrierViewModel(dto.Carrier);

                if (dto.Phones != null)
                {
                    Phones = new ObservableCollection<ListItem<string>>(dto.Phones.Select(s => new ListItem<string>(s)));
                }
                else 
                {
                    Phones = new ObservableCollection<ListItem<string>>() { new ListItem<string>() };
                }
            }
        }

        public DriverViewModel(DriverDto dto) : this(dto, 0) { }

        public DriverViewModel() :base() { }

        public override DriverDto GetDto()
        {
            _dto.Phones = _phones.Select(s => s.Item).ToList();
            return base.GetDto();
        }

        protected override void DefaultInit()
        {
            _dto = new DriverDto()
            {
                BirthDate = DateTime.Now,
                PassportDateOfIssue = DateTime.Now,
            };

            Vehicle = new VehicleViewModel();
            Carrier = new CarrierViewModel();

            Phones = new ObservableCollection<ListItem<string>>() { new ListItem<string>() };
        }
    }

    public class DriverViewModelFactory : IViewModelFactory<DriverDto>
    {
        public DataViewModel<DriverDto> GetViewModel(DriverDto dto, int number)
        {
            return new DriverViewModel(dto, number);
        }

        public DataViewModel<DriverDto> GetViewModel(DriverDto dto)
        {
            return new DriverViewModel(dto);
        }

        public DataViewModel<DriverDto> GetViewModel()
        {
            return new DriverViewModel();
        }
    }
}