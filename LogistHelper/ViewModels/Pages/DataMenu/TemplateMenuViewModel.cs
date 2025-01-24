using DTOs.Dtos;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    internal class TemplateMenuViewModel : MainMenuPageViewModel<ContractTemplateDto>
    {
        public TemplateMenuViewModel(IMainListView<ContractTemplateDto> list, IMainEditView<ContractTemplateDto> edit) : base(list, edit)
        {
        }
    }
}
