using DTOs;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class VehicleMenuViewModel : MainMenuPageViewModel<VehicleDto>
    {
        public VehicleMenuViewModel(IMainListView<VehicleDto> list, IMainEditView<VehicleDto> edit) : base(list, edit)
        {
        }
        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.DatabaseMenu);
        }
    }
}
