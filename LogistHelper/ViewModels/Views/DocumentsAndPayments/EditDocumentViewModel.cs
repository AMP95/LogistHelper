using DTOs;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    class EditDocumentViewModel : SubEditViewModel<DocumentDto>
    {
        private DocumentViewModel _document;

        public EditDocumentViewModel(IDataAccess repository, IViewModelFactory<DocumentDto> factory, IMessageDialog dialog) : base(repository, factory, dialog)
        {
        }

        public override async Task Load(Guid id)
        {
            await base.Load(id);

            _document = EditedViewModel as DocumentViewModel;
            if (id == Guid.Empty) 
            {
                _document.ContractId = MainId;
                EditedViewModel = _document;
            }
        }
    }

    class EditPaymentViewModel : SubEditViewModel<PaymentDto>
    {
        private PaymentViewModel _payment;

        public EditPaymentViewModel(IDataAccess repository, IViewModelFactory<PaymentDto> factory, IMessageDialog dialog) : base(repository, factory, dialog)
        {
        }

        public override async Task Load(Guid id)
        {
            await base.Load(id);

            _payment = EditedViewModel as PaymentViewModel;
            if (id == Guid.Empty)
            {
                _payment.ContractId = MainId;
                EditedViewModel = _payment;
            }
        }
    }
}
