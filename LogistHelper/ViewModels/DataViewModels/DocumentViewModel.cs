using DTOs;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class DocumentViewModel : DataViewModel<DocumentDto>
    {
        public DateTime CreationDate
        {
            get => _dto.CreationDate;
            set 
            {
                _dto.CreationDate = value;
                OnPropertyChanged(nameof(CreationDate));
            }
        }

        public DateTime RecievingDate
        {
            get => _dto.RecievingDate;
            set
            {
                _dto.CreationDate = value;
                OnPropertyChanged(nameof(RecievingDate));
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

        public DocumentDirection Direction
        {
            get => _dto.Direction;
            set
            {
                _dto.Direction = value;
                OnPropertyChanged(nameof(Direction));
            }
        }

        public string Number
        {
            get => _dto.Number;
            set
            {
                _dto.Number = value;
                OnPropertyChanged(nameof(Number));
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

        public float Summ
        {
            get => _dto.Summ;
            set
            {
                _dto.Summ = value;
                OnPropertyChanged(nameof(Summ));
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
        public DocumentViewModel(DocumentDto document) : base(document) { }
        public DocumentViewModel()
        {
            _dto = new DocumentDto();
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
}
