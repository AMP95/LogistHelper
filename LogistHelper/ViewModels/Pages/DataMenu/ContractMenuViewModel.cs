using DTOs;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.Base.Interfaces;

namespace LogistHelper.ViewModels.Pages
{
    public class ContractMenuViewModel : MenuPageViewModel<ContractDto>, ISubMenuPage<DocumentDto>
    {

        private ISubListView<DocumentDto> _documentlist;
        private ISubEditView<DocumentDto> _documentedit;

        public ContractMenuViewModel(IMainListView<ContractDto> list,
                                     IMainEditView<ContractDto> edit,
                                     ISubListView<DocumentDto> documentlist,
                                     ISubEditView<DocumentDto> documentedit) : base(list, edit)
        {
            _documentlist = documentlist;
            _documentlist.SubParent = this;

            _documentedit = documentedit;
            _documentedit.SubParent = this;
        }

        public Task SwitchToMainList()
        {
            throw new NotImplementedException();
        }

        public Task SwitchToSubEdit(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SwitchToSubList(Guid mainId)
        {
            throw new NotImplementedException();
        }
    }

    public class SecondContractMenuViewModel : ContractMenuViewModel
    {
        public SecondContractMenuViewModel(IMainListView<ContractDto> list,
                                           IMainEditView<ContractDto> edit, 
                                           ISubListView<DocumentDto> documentlist,
                                           ISubEditView<DocumentDto> documentedit) : base(list, edit, documentlist, documentedit)
        {

        }

        protected async override Task  Init()
        {
            SwitchToEdit(Guid.Empty);
        }
    }
}
