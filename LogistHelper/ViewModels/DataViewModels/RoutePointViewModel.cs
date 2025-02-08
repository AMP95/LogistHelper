using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.ViewModels.Base;
using Models.Sugget;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Utilities;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class RoutePointViewModel : DataViewModel<RoutePointDto>
    {
        #region Private

        private IDataSuggest<GeoSuggestItem> _dataSuggest;
        private ObservableCollection<ListItem<string>> _phones;
        private IEnumerable<GeoSuggestItem> _searchResult;
        private GeoSuggestItem _selectedAddress;

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

        public ObservableCollection<ListItem<string>> Phones
        {
            get => _phones;
            set => SetProperty(ref _phones, value);
        }

        public IEnumerable<GeoSuggestItem> SearchResult 
        {
            get => _searchResult;
            set => SetProperty(ref _searchResult, value);
        }

        public GeoSuggestItem SelectedAddress
        {
            get => _selectedAddress;
            set 
            { 
                SetProperty(ref _selectedAddress, value);
                if (value != null)
                {
                    Route = SelectedAddress.Location;
                    Address = SelectedAddress.FullAddress;
                }
                else 
                {
                    Route = string.Empty;
                    Address = string.Empty;
                }
            }
        }

        #endregion Public

        public ICommand AddPhoneCommand { get; private set; }
        public ICommand DeletePhoneCommand { get; private set; }
        public ICommand SearchAddressesCommand { get; private set; }

        public RoutePointViewModel(RoutePointDto route, int counter, IDataSuggest<GeoSuggestItem> dataSuggest) : base(route, counter)
        {
            _dataSuggest = dataSuggest;

            if (route != null)
            {
                if (route.Phones != null)
                {
                    Phones = new ObservableCollection<ListItem<string>>(route.Phones.Select(s => new ListItem<string>(s)));
                }
                SelectedAddress = new GeoSuggestItem() { Location = Route, FullAddress = Address };
            }

            InitCommands();
        }

        public RoutePointViewModel(RoutePointDto route, IDataSuggest<GeoSuggestItem> dataSuggest) : this(route, 0, dataSuggest) { }

        public RoutePointViewModel(IDataSuggest<GeoSuggestItem> dataSuggest) : base() 
        { 
            _dataSuggest = dataSuggest;
            Date = DateTime.Now;
            InitCommands();

        }

        private void InitCommands() 
        {
            AddPhoneCommand = new RelayCommand(() =>
            {
                Phones.Add(new ListItem<string>());
            });

            DeletePhoneCommand = new RelayCommand<Guid>((id) =>
            {
                ListItem<string> item = Phones.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    Phones.Remove(item);
                }
            });

            SearchAddressesCommand = new RelayCommand<string>(async (searchString) =>
            {
                SearchResult = await _dataSuggest.SuggestAsync(searchString);
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

            Phones = new ObservableCollection<ListItem<string>>() { new ListItem<string>() };

            InitCommands();
        }

        public bool CheckValidation() 
        {
            if (string.IsNullOrWhiteSpace(Route))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Company))
            {
                return false;
            }
            if (Date == DateTime.MinValue) 
            {
                return false;
            }
            return true;
        }
    }

    public class RouteViewModelFactory : IViewModelFactory<RoutePointDto>
    {
        IDataSuggest<GeoSuggestItem> _dataSuggest;

        public RouteViewModelFactory(IDataSuggest<GeoSuggestItem> dataSuggest)
        {
            _dataSuggest = dataSuggest;
        }

        public DataViewModel<RoutePointDto> GetViewModel(RoutePointDto dto, int number)
        {
            return new RoutePointViewModel(dto, number, _dataSuggest);
        }

        public DataViewModel<RoutePointDto> GetViewModel(RoutePointDto dto)
        {
            return new RoutePointViewModel(dto, _dataSuggest);
        }

        public DataViewModel<RoutePointDto> GetViewModel()
        {
            return new RoutePointViewModel(_dataSuggest);
        }
    }
}
