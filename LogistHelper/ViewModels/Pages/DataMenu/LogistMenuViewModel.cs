using DTOs.Dtos;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class LogistMenuViewModel : MainMenuPageViewModel<LogistDto>
    {
        public LogistMenuViewModel(IMainListView<LogistDto> list, IMainEditView<LogistDto> edit) : base(list, edit)
        {
        }
    }
}
