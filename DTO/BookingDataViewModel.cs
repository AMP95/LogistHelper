using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace DTO
{
    public class BookingDataViewModel : ObservableObject
    {
        private ObservableCollection<DocumentViewModel> _incomeDocs;
        private DocumentViewModel _outcomeDoc;
        private ObservableCollection<DocumentViewModel> _outcomePay;
        private DocumentViewModel _incomePay;


        public Guid Id { get; set; }
        public Guid ContractId { get; set; }

        public virtual ObservableCollection<DocumentViewModel> IncomeDocuments 
        {
            get => _incomeDocs;
            set => SetProperty(ref _incomeDocs, value);
        }
        public virtual DocumentViewModel OutcomeDocument
        {
            get => _outcomeDoc;
            set => SetProperty(ref _outcomeDoc, value);
        }
        public virtual ObservableCollection<DocumentViewModel> OutcomePayments
        {
            get => _outcomePay;
            set => SetProperty(ref _outcomePay, value);
        }
        public virtual DocumentViewModel IncomePayment
        {
            get => _incomePay;
            set => SetProperty(ref _incomePay, value);
        }
    }
}
