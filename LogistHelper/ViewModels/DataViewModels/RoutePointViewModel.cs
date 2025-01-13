using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class RoutePointViewModel : DataViewModel<RoutePointDto>
    {
        #region Private

        private ObservableCollection<StringItem> _phones;

        #endregion Private

        #region Public

        public string Company
        {
            get => _dto.Company;
            set
            {
                _dto.Company = value;
                OnPropertyChanged(nameof(Company));
            }
        }

        public string Route 
        {
            get => _dto.Route;
            set 
            {
                _dto.Route = value;
                OnPropertyChanged(nameof(Route));
            }
        }

        public string Address
        {
            get => _dto.Address;
            set
            {
                _dto.Address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public DateTime Date
        {
            get => _dto.DateAndTime;
            set
            {
                DateTime time = DateTime.ParseExact(Time, "HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault);
                _dto.DateAndTime = new DateTime(value.Year, value.Month, value.Day, time.Hour, time.Minute, 0);
                OnPropertyChanged(nameof(Date));
            }
        }

        public string Time
        {
            get => _dto.DateAndTime.ToString("HH:mm");
            set
            {
                DateTime time = DateTime.ParseExact(value, "HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault);
                _dto.DateAndTime = new DateTime(Date.Year, Date.Month, Date.Day, time.Hour, time.Minute, 0);
                OnPropertyChanged(nameof(Time));
            }
        }

        public LoadingSide Side
        {
            get => _dto.Side;
            set
            {
                _dto.Side = value;
                OnPropertyChanged(nameof(Side));
            }
        }
        public LoadPointType Type
        {
            get => _dto.Type;
            set
            {
                _dto.Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        public ObservableCollection<StringItem> Phones
        {
            get => _phones;
            set => SetProperty(ref _phones, value);
        }

        #endregion Public

        public ICommand AddPhoneCommand { get; }
        public ICommand DeletePhoneCommand { get; }

        public RoutePointViewModel(RoutePointDto route, int counter) : base(route, counter)
        {
            if (route != null)
            {
                Phones = new ObservableCollection<StringItem>(route.Phones.Select(s => new StringItem(s)));
            }

            AddPhoneCommand = new RelayCommand(() => 
            {
                Phones.Add(new StringItem());
            });

            DeletePhoneCommand = new RelayCommand<Guid>((id) => 
            {
                StringItem item = Phones.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    Phones.Remove(item);
                }
            });
        }

        public RoutePointViewModel(RoutePointDto route) : this(route, 0) { }

        public RoutePointViewModel() : base() 
        {
            AddPhoneCommand = new RelayCommand(() =>
            {
                Phones.Add(new StringItem());
            });

            DeletePhoneCommand = new RelayCommand<Guid>((id) =>
            {
                StringItem item = Phones.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    Phones.Remove(item);
                }
            });
        }

        public override RoutePointDto GetDto() 
        {
            _dto.Phones = _phones.Select(s => s.Item).ToList();
            return base.GetDto();  
        }

        protected override void DefaultInit()
        {
            _dto = new RoutePointDto();
            Phones = new ObservableCollection<StringItem>() { new StringItem() };
        }
    }

    public class RouteViewModelFactory : IViewModelFactory<RoutePointDto>
    {
        public DataViewModel<RoutePointDto> GetViewModel(RoutePointDto dto, int number)
        {
            return new RoutePointViewModel(dto, number);
        }

        public DataViewModel<RoutePointDto> GetViewModel(RoutePointDto dto)
        {
            return new RoutePointViewModel(dto);
        }

        public DataViewModel<RoutePointDto> GetViewModel()
        {
            return new RoutePointViewModel();
        }
    }
}
