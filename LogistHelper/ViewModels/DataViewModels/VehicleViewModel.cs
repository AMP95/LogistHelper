using DTOs;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class VehicleViewModel : DataViewModel<VehicleDto>
    {
        private CarrierViewModel _carrier;

        #region Public

        public string TruckModel
        {
            get => _dto.TruckModel;
            set
            {
                _dto.TruckModel = value;
                OnPropertyChanged(nameof(TruckModel));
            }
        }

        public string TruckNumber
        {
            get => _dto.TruckNumber;
            set
            {
                _dto.TruckNumber = value;
                OnPropertyChanged(nameof(TruckNumber));
            }
        }

        public string TrailerModel
        {
            get => _dto.TrailerModel;
            set
            {
                _dto.TrailerModel = value;
                OnPropertyChanged(nameof(TrailerModel));
            }
        }

        public string TrailerNumber
        {
            get => _dto.TrailerNumber;
            set
            {
                _dto.TrailerNumber = value;
                OnPropertyChanged(nameof(TrailerNumber));
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

        #endregion Public

        public VehicleViewModel(VehicleDto dto, int counter) : base(dto, counter) 
        {
            if (dto != null)
            {
                _carrier = new CarrierViewModel(dto.Carrier);
            }
        }
        public VehicleViewModel(VehicleDto dto) : this(dto, 0) { }

        public VehicleViewModel() : base() { }

        protected override void DefaultInit()
        {
            _dto = new VehicleDto();
            Carrier = new CarrierViewModel();
        }
    }

    public class VehicleViewModelFactory : IViewModelFactory<VehicleDto>
    {
        public DataViewModel<VehicleDto> GetViewModel(VehicleDto dto, int number)
        {
            return new VehicleViewModel(dto, number);
        }

        public DataViewModel<VehicleDto> GetViewModel(VehicleDto dto)
        {
            return new VehicleViewModel(dto);
        }

        public DataViewModel<VehicleDto> GetViewModel()
        {
            return new VehicleViewModel();
        }
    }
}
