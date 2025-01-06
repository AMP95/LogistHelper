using DTOs;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class VehicleViewModel : DataViewModel<VehicleDto>
    {
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

        #endregion Public

        public VehicleViewModel(VehicleDto dto) : base(dto) { }
        public VehicleViewModel(VehicleDto dto, int counter) : base(dto, counter) { }
        public VehicleViewModel()
        {
            _dto = new VehicleDto();
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
