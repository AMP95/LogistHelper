using CommunityToolkit.Mvvm.ComponentModel;
using DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class RoutePointViewModel : ObservableObject, IDataErrorInfo
    {
        #region Private

        private RoutePointDto _route;
        private ObservableCollection<string> _phones;

        #endregion Private

        #region Public

        public Guid Id 
        { 
            get => _route.Id;
        }

        public string Route 
        {
            get => _route.Route;
            set 
            { 
                _route.Route = value;
                OnPropertyChanged(nameof(Route));
            }
        }
        public string Address
        {
            get => _route.Address;
            set
            {
                _route.Address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        public LoadingSide Side
        {
            get => _route.Side;
            set
            {
                _route.Side = value;
                OnPropertyChanged(nameof(Side));
            }
        }
        public LoadPointType Type
        {
            get => _route.Type;
            set
            {
                _route.Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        public ObservableCollection<string> Phones
        {
            get => _phones;
            set => SetProperty(ref _phones, value);
        }

        #endregion Public

        #region Validation

        public string this[string columnName] => _route[columnName];

        public string Error => _route.Error;

        #endregion Validation

        public RoutePointViewModel(RoutePointDto route)
        {
            _route = route; 
            Phones = new ObservableCollection<string>(route.Phones);
        }

        public RoutePointViewModel()
        {
            _route = new RoutePointDto()
            {
                Phones = new List<string>()
            };
            Phones = new ObservableCollection<string>();
        }

        public RoutePointDto GetDto() 
        {
            _route.Phones = _phones.ToList();
            return _route;  
        }

    }
}
