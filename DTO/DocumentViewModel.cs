using CommunityToolkit.Mvvm.ComponentModel;
using Models;

namespace DTO
{
    public class DocumentViewModel : ObservableObject
    {
        private DocumentType _docType;
        private DateTime _creationDate;
        private RecievingType _recType;
        private DateTime _recievingDate;
        private string _number;
        private float _sum;

        public Guid Id { get; set; }
        public Guid ContractId { get; set; }


        public DocumentType DocumentType 
        {
            get => _docType;
            set => SetProperty(ref _docType, value);
        }
        public DateTime CreationDate
        {
            get => _creationDate;
            set => SetProperty(ref _creationDate, value);
        }
        public RecievingType RecieveType
        {
            get => _recType;
            set => SetProperty(ref _recType, value);
        }
        public DateTime RecievingDate
        {
            get => _recievingDate;
            set => SetProperty(ref _recievingDate, value);
        }
        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }
        public float Summ
        {
            get => _sum;
            set => SetProperty(ref _sum, value);
        }
    }
}
