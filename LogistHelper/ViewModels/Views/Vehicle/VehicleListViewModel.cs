using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    internal class VehicleListViewModel : ListViewModel<VehicleDto>
    {
        public VehicleListViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<VehicleDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
        }
    }
}
