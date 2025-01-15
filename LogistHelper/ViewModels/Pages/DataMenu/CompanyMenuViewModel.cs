using DTOs;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.Base.Interfaces;

namespace LogistHelper.ViewModels.Pages
{
    public class ClientMenuPageViewModel : MenuPageViewModel<ClientDto>
    {
        public ClientMenuPageViewModel(IMainListView<ClientDto> list, IMainEditView<ClientDto> edit) : base(list, edit)
        {
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.DatabaseMenu);
        }
    }

    public class CarrierMenuPageViewModel : MenuPageViewModel<CarrierDto>
    {
        public CarrierMenuPageViewModel(IMainListView<CarrierDto> list, IMainEditView<CarrierDto> edit) : base(list, edit)
        {
        }
        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.DatabaseMenu);
        }
    }
}
