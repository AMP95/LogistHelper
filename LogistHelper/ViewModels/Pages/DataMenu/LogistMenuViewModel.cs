using DTOs.Dtos;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages.DataMenu
{
    public class LogistMenuViewModel : MainMenuPageViewModel<LogistDto>
    {
        public LogistMenuViewModel(IMainListView<LogistDto> list, IMainEditView<LogistDto> edit) : base(list, edit)
        {
        }
    }
}
