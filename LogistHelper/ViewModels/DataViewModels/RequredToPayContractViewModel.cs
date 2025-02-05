using DTOs;
using DTOs.Dtos;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class RequredToPayContractViewModel : DataViewModel<RequiredToPayContractDto>
    {
        private bool _isSelectedToPrint;

        public bool IsSelectedToPrint 
        { 
            get => _isSelectedToPrint;
            set => SetProperty(ref _isSelectedToPrint, value);
        }

        public short Number 
        { 
            get => _dto.Number;
            set 
            { 
                _dto.Number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        public DateTime CreationDate
        {
            get => _dto.CreationDate;
            set
            {
                _dto.CreationDate = value;
                OnPropertyChanged(nameof(CreationDate));
            }
        }

        public string Carrier
        {
            get => _dto.Carrier;
            set
            {
                _dto.Carrier = value;
                OnPropertyChanged(nameof(Carrier));
            }
        }

        public string Driver
        {
            get => _dto.Driver;
            set
            {
                _dto.Driver = value;
                OnPropertyChanged(nameof(Driver));
            }
        }

        public string Vehicle
        {
            get => _dto.Vehicle;
            set
            {
                _dto.Vehicle = value;
                OnPropertyChanged(nameof(Vehicle));
            }
        }

        public double Balance
        {
            get => _dto.Balance;
            set
            {
                _dto.Balance = value;
                OnPropertyChanged(nameof(Balance));
            }
        }

        public PayType Type
        {
            get => _dto.Type;
            set
            {
                _dto.Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public int DaysToExpiration
        {
            get => _dto.DaysToExpiration;
            set
            {
                _dto.DaysToExpiration = value;
                OnPropertyChanged(nameof(DaysToExpiration));
            }
        }


        protected override void DefaultInit()
        {
            _dto = new RequiredToPayContractDto();
        }

        public RequredToPayContractViewModel(RequiredToPayContractDto dto, int counter) : base(dto, counter) { }
        public RequredToPayContractViewModel(RequiredToPayContractDto dto) : this(dto, 0) { }
        public RequredToPayContractViewModel() : base() { }
    }
}
