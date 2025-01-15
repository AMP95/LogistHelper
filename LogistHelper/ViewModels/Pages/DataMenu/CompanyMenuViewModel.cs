using DTOs;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class ClientMenuPageViewModel : MenuPageViewModel<CompanyDto>
    {
        public ClientMenuPageViewModel(ListViewModel<CompanyDto> list, EditViewModel<CompanyDto> edit) : base(list, edit)
        {
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.DatabaseMenu);
        }
    }

    public class CarrierMenuPageViewModel : MenuPageViewModel<CarrierDto>
    {
        public CarrierMenuPageViewModel(ListViewModel<CarrierDto> list, EditViewModel<CarrierDto> edit) : base(list, edit)
        {
        }
        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.DatabaseMenu);
        }
    }
}
