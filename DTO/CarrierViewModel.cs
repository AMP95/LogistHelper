using Models;
using System.Collections.ObjectModel;

namespace DTO
{
    public class CarrierViewModel : CompanyViewModel
    {
        private VAT _vat;
        private ObservableCollection<VehicleViewModel> _vehicles;
        private ObservableCollection<VehiclePartViewModel> _trucks;
        private ObservableCollection<VehiclePartViewModel> _trailers;
        private ObservableCollection<DriverViewModel> _drivers;

        public VAT VAT 
        {
            get => _vat;
            set => SetProperty(ref _vat, value);
        }
        public ObservableCollection<VehicleViewModel> Vehicles
        {
            get => _vehicles;
            set => SetProperty(ref _vehicles, value);
        }
        public ObservableCollection<VehiclePartViewModel> Trucks
        {
            get => _trucks;
            set => SetProperty(ref _trucks, value);
        }
        public ObservableCollection<VehiclePartViewModel> Trailers
        {
            get => _trailers;
            set => SetProperty(ref _trailers, value);
        }
        public ObservableCollection<DriverViewModel> Drivers
        {
            get => _drivers;
            set => SetProperty(ref _drivers, value);
        }
    }
}
