using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.Base.Interfaces;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    class DocumentListViewModel : ListViewModel<DocumentDto>, ISubListView<DocumentDto>
    {
        public ISubMenuPage<DocumentDto> SubParent { get; set; }

        public DocumentListViewModel(ISettingsRepository<Settings> repository, 
                                     IViewModelFactory<DocumentDto> factory, 
                                     IDialog dialog) : base(repository, factory, dialog)
        {

        }

        public Task Load(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}