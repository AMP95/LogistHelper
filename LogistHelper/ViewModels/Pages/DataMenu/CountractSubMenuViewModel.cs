using DTOs;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class CountractSubMenuViewModel : BasePageViewModel
    {
        private ISubMenuPage<DocumentDto> _documentSub;
        private ISubMenuPage<PaymentDto> _paymentSub;

        public ISubParent MainParent { get ; set; }

        public ISubMenuPage<DocumentDto> DocumentSub 
        {
            get => _documentSub;
            set => SetProperty(ref _documentSub, value);
        }

        public ISubMenuPage<PaymentDto> PaymentSub
        {
            get => _paymentSub;
            set => SetProperty(ref _paymentSub, value);
        }

        public CountractSubMenuViewModel(ISubMenuPage<DocumentDto> documentSub,
                                         ISubMenuPage<PaymentDto> paymentSub)
        {
            DocumentSub = documentSub;
            PaymentSub = paymentSub;
        }

        public void Load(Guid mainId) 
        {
            DocumentSub.SwitchToList(mainId);
            PaymentSub.SwitchToList(mainId);
        }

        protected override void BackCommandExecutor()
        {
            MainParent.SwitchToMain();
        }
    }
}
