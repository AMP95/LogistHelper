using DTOs;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class DocumentBaseViewModel<T> : DataViewModel<T> where T : DocumentBaseDto
    {
        public string Number
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
        public DocumentDirection Direction
        {
            get => _dto.Direction;
            set
            {
                _dto.Direction = value;
                OnPropertyChanged(nameof(Direction));
            }
        }
        public float Summ
        {
            get => _dto.Summ;
            set
            {
                _dto.Summ = value;
                OnPropertyChanged(nameof(Summ));
            }
        }
        public Guid ContractId
        {
            get => _dto.ContractId;
            set
            {
                _dto.ContractId = value;
                OnPropertyChanged(nameof(ContractId));
            }
        }

        public DocumentBaseViewModel(T document, int counter) : base(document, counter) { }
        public DocumentBaseViewModel(T document) : this(document, 0) { }
        public DocumentBaseViewModel() : base() { }

        protected override void DefaultInit() 
        { }
    }

    public class DocumentViewModel : DocumentBaseViewModel<DocumentDto> 
    {
        public DateTime RecievingDate
        {
            get => _dto.RecievingDate;
            set
            {
                _dto.RecievingDate = value;
                OnPropertyChanged(nameof(RecievingDate));
            }
        }
        public RecievingType RecieveType
        {
            get => _dto.RecieveType;
            set
            {
                _dto.RecieveType = value;
                OnPropertyChanged(nameof(RecieveType));
            }
        }
        public DocumentType Type
        {
            get => _dto.Type;
            set
            {
                _dto.Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        public DocumentViewModel(DocumentDto document, int counter) : base(document, counter) { }
        public DocumentViewModel(DocumentDto document) : this(document, 0) { }
        public DocumentViewModel() : base() { }

        protected override void DefaultInit()
        {
            _dto = new DocumentDto() { RecievingDate = DateTime.Now , CreationDate = DateTime.Now};
        }
    }

    public class PaymentViewModel : DocumentBaseViewModel<PaymentDto>
    {
        public PaymentViewModel(PaymentDto document, int counter) : base(document, counter) { }
        public PaymentViewModel(PaymentDto document) : this(document, 0) { }
        public PaymentViewModel() : base() { }
        protected override void DefaultInit()
        {
            _dto = new PaymentDto() { CreationDate = DateTime.Now };
        }
    }

    public class DocumentViewModelFactory : IViewModelFactory<DocumentDto>
    {
        public DataViewModel<DocumentDto> GetViewModel(DocumentDto dto, int number)
        {
            return new DocumentViewModel(dto, number);
        }

        public DataViewModel<DocumentDto> GetViewModel(DocumentDto dto)
        {
            return new DocumentViewModel(dto);
        }

        public DataViewModel<DocumentDto> GetViewModel()
        {
            return new DocumentViewModel();
        }
    }

    public class PaymentViewModelFactory : IViewModelFactory<PaymentDto>
    {
        public DataViewModel<PaymentDto> GetViewModel(PaymentDto dto, int number)
        {
            return new PaymentViewModel(dto, number);
        }

        public DataViewModel<PaymentDto> GetViewModel(PaymentDto dto)
        {
            return new PaymentViewModel(dto);
        }

        public DataViewModel<PaymentDto> GetViewModel()
        {
            return new PaymentViewModel();
        }
    }
}
