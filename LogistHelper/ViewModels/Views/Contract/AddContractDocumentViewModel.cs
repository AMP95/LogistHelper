using CommunityToolkit.Mvvm.ComponentModel;
using DTOs;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.Pages;

namespace LogistHelper.ViewModels.Views
{
    public class AddContractDocumentViewModel : ObservableObject
    {
        public ContractMenuViewModel Parent { get; set; }

        public virtual async Task Load(Guid id)
        {
        }
    }
}
