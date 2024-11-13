using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    class DriverViewModel : ObservableObject
    {
        private string _name;
        private string _passportSerial;
        private string _passportIssuer;
        private DateTime _passportDate;
        private ObservableCollection<string> _phones;
        private VehicleViewModel _vehicle;

        public string Name 
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string PassportSerial
        {
            get => _passportSerial;
            set => SetProperty(ref _passportSerial, value);
        }

        public string PassportIssuer
        {
            get => _passportIssuer;
            set => SetProperty(ref _passportIssuer, value);
        }

        public DateTime PassportDate
        {
            get => _passportDate;
            set => SetProperty(ref _passportDate, value);
        }

        public ObservableCollection<string> Phones
        {
            get => _phones;
            set => SetProperty(ref _phones, value);
        }

        public VehicleViewModel Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }
    }
}
