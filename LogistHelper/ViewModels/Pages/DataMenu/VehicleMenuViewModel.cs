using DTOs;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class VehicleMenuViewModel : MenuPageViewModel<VehicleDto>
    {
        public VehicleMenuViewModel(ListViewModel<VehicleDto> list, EditViewModel<VehicleDto> edit) : base(list, edit)
        {
        }
        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.DatabaseMenu);
        }
    }
}
