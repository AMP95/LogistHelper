using DTOs;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class DriverMenuViewModel : MainMenuPageViewModel<DriverDto>
    {
        public DriverMenuViewModel(MainListViewModel<DriverDto> list, MainEditViewModel<DriverDto> edit) : base(list, edit)
        {
        }
    }
}
