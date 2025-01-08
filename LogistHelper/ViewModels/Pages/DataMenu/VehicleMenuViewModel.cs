using DTOs;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class VehicleMenuViewModel : MenuPageViewModel<VehicleDto>
    {
        public VehicleMenuViewModel(ListViewModel<VehicleDto> list, EditViewModel<VehicleDto> edit) : base(list, edit)
        {
        }
    }
}
