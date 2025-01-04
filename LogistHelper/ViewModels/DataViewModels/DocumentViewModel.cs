using CommunityToolkit.Mvvm.ComponentModel;
using DTOs;
using System.ComponentModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class DocumentViewModel : ObservableObject, IDataErrorInfo
    {
        #region Private

        private DocumentDto _document;

        #endregion Private

        #region Public

        public Guid Id 
        { 
            get => _document.Id;
        }

        public DateTime CreationDate
        {
            get => _document.CreationDate;
            set 
            { 
                _document.CreationDate = value;
                OnPropertyChanged(nameof(CreationDate));
            }
        }

        public DateTime RecievingDate
        {
            get => _document.RecievingDate;
            set
            {
                _document.CreationDate = value;
                OnPropertyChanged(nameof(RecievingDate));
            }
        }

        public Guid ContractId
        {
            get => _document.ContractId;
            set
            {
                _document.ContractId = value;
                OnPropertyChanged(nameof(ContractId));
            }
        }

        public DocumentDirection Direction
        {
            get => _document.Direction;
            set
            {
                _document.Direction = value;
                OnPropertyChanged(nameof(Direction));
            }
        }

        public string Number
        {
            get => _document.Number;
            set
            {
                _document.Number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        public RecievingType RecieveType
        {
            get => _document.RecieveType;
            set
            {
                _document.RecieveType = value;
                OnPropertyChanged(nameof(RecieveType));
            }
        }

        public float Summ
        {
            get => _document.Summ;
            set
            {
                _document.Summ = value;
                OnPropertyChanged(nameof(Summ));
            }
        }

        public DocumentType Type
        {
            get => _document.Type;
            set
            {
                _document.Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }



        #endregion Public

        #region Validation

        public string Error => _document.Error;
        public string this[string columnName] => _document[columnName];

        #endregion Validation

        public DocumentViewModel(DocumentDto document)
        {
            _document = document;
        }
        public DocumentViewModel()
        {
            _document = new DocumentDto();
        }

        public DocumentDto GetDto() 
        { 
            return _document;
        }
    }
}
