using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    public class ClientListViewModel : ListViewModel<CompanyDto>
    {
        public ClientListViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<CompanyDto> factory, IDialog dialog) : base(repository, factory, dialog)
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
