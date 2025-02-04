using DTOs;
using LogistHelper.Models;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Shared;

namespace LogistHelper.ViewModels.Pages
{
    class CompanySettingsPageViewModel : BasePageViewModel, IMainMenuPage<CompanyDto>
    {

        #region Private

        private IMainEditView<CompanyDto> _editor;
        private IDataAccess _access;


        #endregion Private

        public IMainEditView<CompanyDto> CompanyEditor 
        {
            get => _editor;
            set => SetProperty(ref _editor, value);
        }

        public CompanySettingsPageViewModel(IMainEditView<CompanyDto> conpanyEditor,
                                            IDataAccess dataAccess)
        {
            CompanyEditor = conpanyEditor;
            CompanyEditor.MainParent = this;

            _access = dataAccess;

            SwitchToEdit(Guid.Empty);
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(PageType.MainMenu);
        }

        public Task SwitchToList()
        {
            BackCommand?.Execute(null);
            return Task.CompletedTask;
        }

        public async Task SwitchToEdit(Guid id)
        {
            IAccessResult<IEnumerable<CompanyDto>> result = await _access.GetFilteredAsync<CompanyDto>(nameof(CompanyDto.Type), CompanyType.Current.GetDescription());

            if (result.IsSuccess && result.Result.Any())
            {
                CompanyEditor.Load(result.Result.FirstOrDefault().Id);
            }
            else
            {
                CompanyEditor.Load(Guid.Empty);

                (CompanyEditor.EditedViewModel as CompanyViewModel).Type = CompanyType.Current;
            }
        }
    }
}
