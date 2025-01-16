using DTOs;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class ClientMenuPageViewModel : MainMenuPageViewModel<CompanyDto>
    {
        public ClientMenuPageViewModel(MainListViewModel<CompanyDto> list, MainEditViewModel<CompanyDto> edit) : base(list, edit)
        {
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.DatabaseMenu);
        }
    }

    public class CarrierMenuPageViewModel : MainMenuPageViewModel<CarrierDto>
    {
        public CarrierMenuPageViewModel(MainListViewModel<CarrierDto> list, MainEditViewModel<CarrierDto> edit) : base(list, edit)
        {
        }
        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.DatabaseMenu);
        }
    }
}
