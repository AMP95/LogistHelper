using DTOs;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class ClientMenuPageViewModel : MenuPageViewModel<CompanyDto>
    {
        public ClientMenuPageViewModel(ListViewModel<CompanyDto> list, EditViewModel<CompanyDto> edit) : base(list, edit)
        {
        }
    }

    public class CarrierMenuPageViewModel : MenuPageViewModel<CarrierDto>
    {
        public CarrierMenuPageViewModel(ListViewModel<CarrierDto> list, EditViewModel<CarrierDto> edit) : base(list, edit)
        {
        }
    }
}
