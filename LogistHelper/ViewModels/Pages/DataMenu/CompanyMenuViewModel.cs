using DTOs;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class CompanyMenuPageViewModel : MainMenuPageViewModel<CompanyDto>
    {
        public CompanyMenuPageViewModel(IMainListView<CompanyDto> list, IMainEditView<CompanyDto> edit) : base(list, edit)
        {
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.DatabaseMenu);
        }
    }

    public class CarrierMenuPageViewModel : MainMenuPageViewModel<CarrierDto>
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
