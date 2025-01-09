using DTOs;
using LogistHelper.ViewModels.Base;
using System.Collections.ObjectModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class RoutePointViewModel : DataViewModel<RoutePointDto>
    {
        #region Private

        private ObservableCollection<StringItem> _phones;

        #endregion Private

        #region Public

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

        public RoutePointViewModel(RoutePointDto route, int counter) : base(route, counter)
        {
            if (route != null)
            {
                Phones = new ObservableCollection<StringItem>(route.Phones.Select(s => new StringItem(s)));
            }
        }

        public RoutePointViewModel(RoutePointDto route) : this(route, 0) { }

        public RoutePointViewModel() : base() { }

        public override RoutePointDto GetDto() 
        {
            _dto.Phones = _phones.Select(s => s.Item).ToList();
            return base.GetDto();  
        }

        protected override void DefaultInit()
        {
            _dto = new RoutePointDto();
            Phones = new ObservableCollection<StringItem>();
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
