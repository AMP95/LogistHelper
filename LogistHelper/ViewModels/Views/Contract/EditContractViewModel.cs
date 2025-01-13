using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    class EditContractViewModel : EditViewModel<ContractDto>
    {
        public EditContractViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<ContractDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
        }
    }
}
