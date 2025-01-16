using DTOs;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages.DataMenu
{
    public class DocumentMenuViewModel : SubMenuPageViewModel<DocumentDto>
    {
        public DocumentMenuViewModel(ISubListView<DocumentDto> list, ISubEditView<DocumentDto> edit) : base(list, edit)
        {
        }
    }
}
