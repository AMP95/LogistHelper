using CommunityToolkit.Mvvm.ComponentModel;
using DTOs;
using System.ComponentModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public abstract class VahiclePartViewModel : ObservableObject, IDataErrorInfo
    {
        protected VehicleDto _part;


        #region Public

        public Guid Id
        {
            get => _part.Id;
        }

        public string Model
        {
            get => _part.Model;
            set
            {
                _part.Model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        public string Number
        {
            get => _part.Number;
            set
            {
                _part.Number = value;
                OnPropertyChanged(nameof(Number));
            }
        }



        #endregion Public

        #region Validation

        public string Error => _part.Error;

        public string this[string columnName]
        {
            get
            {
                return _part[columnName];
            }
        }

        #endregion Validation

        public VahiclePartViewModel(VehicleDto part)
        {
            _part = part;
        }

        protected VahiclePartViewModel()
        {
            
        }

        public VehicleDto GetDto() 
        { 
            return _part;
        }
    }

    public class TruckViewModel : VahiclePartViewModel
    {
        public TruckViewModel()
        {
            _part = new TruckDto();
        }

        public TruckViewModel(TruckDto dto) : base(dto) { }

    }

    public class TrailerViewModel : VahiclePartViewModel
    {
        public TrailerViewModel()
        {
            _part = new TrailerDto();
        }

        public TrailerViewModel(TrailerDto dto) : base(dto) { }

    }
}
