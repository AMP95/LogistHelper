using DTOs.Dtos;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    public class EditLogistViewModel : MainEditViewModel<LogistDto>
    {
        public EditLogistViewModel(IDataAccess dataAccess, 
                                   IViewModelFactory<LogistDto> factory, 
                                   IMessageDialog dialog) : base(dataAccess, factory, dialog)
        {
        }
    }
}
