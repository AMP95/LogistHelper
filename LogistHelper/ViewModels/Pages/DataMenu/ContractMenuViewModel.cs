using DTOs;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class ContractMenuViewModel : MainMenuPageViewModel<ContractDto>, ISubParent
    {
        private CountractSubMenuViewModel _sub;

        public ContractMenuViewModel(IMainListView<ContractDto> list,
                                     IMainEditView<ContractDto> edit,
                                     CountractSubMenuViewModel sub) : base(list, edit)
        {
            _sub = sub;
            _sub.MainParent = this;
        }

        public async Task SwitchToMain()
        {
            await SwitchToList();
        }

        public Task SwitchToSub(Guid mainid)
        {
            _sub.Load(mainid);
            Content = _sub;
            return Task.CompletedTask;
        }
    }

    public class SecondContractMenuViewModel : ContractMenuViewModel
    {
        public SecondContractMenuViewModel(IMainListView<ContractDto> list,
                                           IMainEditView<ContractDto> edit,
                                           CountractSubMenuViewModel sub) : base(list, edit, sub)
        {

        }

        protected async override Task  Init()
        {
            SwitchToEdit(Guid.Empty);
        }
    }
}
