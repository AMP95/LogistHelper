using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.Base.Interfaces;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    class EditDocumentViewModel : EditViewModel<DocumentDto>, ISubEditView<DocumentDto>
    {
        public ISubMenuPage<DocumentDto> SubParent { get; set; }

        public EditDocumentViewModel(ISettingsRepository<Settings> repository, 
                                     IViewModelFactory<DocumentDto> factory, 
                                     IDialog dialog) : base(repository, factory, dialog)
        {
        }

        public Task Load(Guid subId, Guid mainId)
        {
            throw new NotImplementedException();
        }
    }
}
