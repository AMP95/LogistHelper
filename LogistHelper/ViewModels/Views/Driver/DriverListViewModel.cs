using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    public class DriverListViewModel : MainListViewModel<DriverDto>
    {
        public DriverListViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<DriverDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
        }
    }
}
