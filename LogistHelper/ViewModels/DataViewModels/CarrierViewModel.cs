using DTOs;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class CarrierViewModel : CompanyViewModel 
    {
        private CarrierDto _carrier;

        public VAT Vat
        {
            get => _carrier.Vat;
            set
            {
                _carrier.Vat = value;
                OnPropertyChanged(nameof(Vat));
            }
        }

        public CarrierViewModel(CarrierDto carrier) : base(carrier)
        {
            _carrier = carrier;
        }

        public CarrierViewModel()
        {
            _carrier = new CarrierDto();
            _company = _carrier;
        }
    }
}
