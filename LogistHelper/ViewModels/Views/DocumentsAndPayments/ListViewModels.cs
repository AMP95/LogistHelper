using DTOs;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    class DocumentListViewModel : SubListViewModel<DocumentDto>
    {
        public DocumentListViewModel(IDataAccess repository, IViewModelFactory<DocumentDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
            _mainPropertyName = nameof(DocumentDto.ContractId);
        }
    }

    class PaymentListViewModel : SubListViewModel<PaymentDto>
    {
        public PaymentListViewModel(IDataAccess repository, IViewModelFactory<PaymentDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
            _mainPropertyName = nameof(PaymentDto.ContractId);
        }
    }
}