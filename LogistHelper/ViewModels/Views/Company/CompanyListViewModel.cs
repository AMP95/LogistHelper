using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    public class ClientListViewModel : ListViewModel<ClientDto>
    {
        public ClientListViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<ClientDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
        }
    }

    public class CarrierListViewModel : ListViewModel<CarrierDto>
    {
        public CarrierListViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<CarrierDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
        }
    }


}
