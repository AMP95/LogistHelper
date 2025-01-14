using DTOs;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.Views;

namespace LogistHelper.ViewModels.Pages
{
    public class ContractMenuViewModel : MenuPageViewModel<ContractDto>
    {
        private AddContractDocumentViewModel _addDocument;
        public ContractMenuViewModel(ListViewModel<ContractDto> list, EditViewModel<ContractDto> edit, AddContractDocumentViewModel document) : base(list, edit)
        {
            _addDocument = document;
            _addDocument.Parent = this;
        }

        public async Task SwitchToDocument(Guid id)
        {
            Content = _addDocument;
            _addDocument.Load(id);
        }
    }

    public class SecondContractMenuViewModel : ContractMenuViewModel
    {
        private AddContractDocumentViewModel _addDocument;
        public SecondContractMenuViewModel(ListViewModel<ContractDto> list, EditViewModel<ContractDto> edit, AddContractDocumentViewModel document) : base(list, edit, document)
        {
        }

        protected async override Task  Init()
        {
            SwitchToEdit(Guid.Empty);
        }
    }
}
