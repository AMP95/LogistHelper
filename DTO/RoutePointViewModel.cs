using CommunityToolkit.Mvvm.ComponentModel;
using Models;

namespace DTO
{
    public class RoutePointViewModel : ObservableObject
    {
        private string _address;
        private LoadingType _loadingType;
        private string _phones;

        public Guid Id { get; set; }


        public string Address 
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public string Phones
        {
            get => _phones;
            set => SetProperty(ref _phones, value);
        }

        public LoadingType Loading 
        {
            get => _loadingType;
            set => SetProperty(ref _loadingType, value);
        }

    }
}
