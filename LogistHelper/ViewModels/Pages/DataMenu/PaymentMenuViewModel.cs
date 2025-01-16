using DTOs;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class PaymentMenuViewModel : SubMenuPageViewModel<PaymentDto>
    {
        public PaymentMenuViewModel(ISubListView<PaymentDto> list, ISubEditView<PaymentDto> edit) : base(list, edit)
        {
        }
    }
}
