using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    internal class VehicleListViewModel : MainListViewModel<VehicleDto>
    {
        public VehicleListViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<VehicleDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
        }
    }
}
