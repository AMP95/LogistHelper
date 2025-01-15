using DTOs;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.Base.Interfaces;

namespace LogistHelper.ViewModels.Pages
{
    public class VehicleMenuViewModel : MenuPageViewModel<VehicleDto>
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
