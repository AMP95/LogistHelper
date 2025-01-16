using DTOs;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class DriverMenuViewModel : MainMenuPageViewModel<DriverDto>
    {
        public DriverMenuViewModel(IMainListView<DriverDto> list, IMainEditView<DriverDto> edit) : base(list, edit)
        {
        }
        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.DatabaseMenu);
        }
    }
}
