using CommunityToolkit.Mvvm.ComponentModel;
using LogistHelper.Models;
using System.Collections.ObjectModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    class CompanyViewModel : ObservableObject
    {
        private string _name;
        private string _address;
        private string _inn;
        private string _kpp;
        private List<string> _emails;
        private string _phones;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        public string InnKpp
        {
            get => $"{_inn}/{_kpp}";
            set
            {
                string[] strings = value.Split('/');
                _inn = strings[0];
                if (strings.Length > 1)
                {
                    _kpp = strings[1];
                }
            }
        }
        public string Emails
        {
            get => string.Join(", ", _emails);
            set 
            {
                _emails?.Clear();
                _emails = value.Split(",").ToList();
            }
        }
        public string Phones
        {
            get => _phones;
            set => SetProperty(ref _phones, value);
        }

    }

    class CarrierViewModel : CompanyViewModel
    {
        private VAT _vat;

        private ObservableCollection<DriverViewModel> _drivers;
        private ObservableCollection<VehicleViewModel> _vehicles;
        private ObservableCollection<TractorViewModel> _tractors;
        private ObservableCollection<TrailerViewModel> _trailers;

        public VAT Vat
        {
            get => _vat;
            set => SetProperty(ref _vat, value);
        }
        
        public ObservableCollection<DriverViewModel> Drivers
        {
            get => _drivers;
            set => SetProperty(ref _drivers, value);
        }
        public ObservableCollection<VehicleViewModel> Vehicles
        {
            get => _vehicles;
            set => SetProperty(ref _vehicles, value);
        }
        public ObservableCollection<TractorViewModel> Tractors
        {
            get => _tractors;
            set => SetProperty(ref _tractors, value);
        }
        public ObservableCollection<TrailerViewModel> Trailers
        {
            get => _trailers;
            set => SetProperty(ref _trailers, value);
        }

    }
}
