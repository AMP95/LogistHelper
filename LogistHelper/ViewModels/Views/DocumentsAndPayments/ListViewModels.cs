using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    class DocumentListViewModel : SubListViewModel<DocumentDto>
    {
        public DocumentListViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<DocumentDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
            _mainPropertyName = nameof(DocumentDto.ContractId);
        }
    }

    class PaymentListViewModel : SubListViewModel<PaymentDto>
    {
        public PaymentListViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<PaymentDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
            _mainPropertyName = nameof(PaymentDto.ContractId);
        }
    }
}