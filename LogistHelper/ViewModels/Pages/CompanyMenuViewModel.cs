using CommunityToolkit.Mvvm.ComponentModel;
using DTOs;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.Views;

namespace LogistHelper.ViewModels.Pages
{
    public class CompanyMenuViewModel<T> : BasePageViewModel where T : CompanyDto
    {
        private ObservableObject _content;

        private CompanyListViewModel<T> _list;
        private EditCompanyViewModel<T> _edit;


        #region Public

        public ObservableObject Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        #endregion Public

        public CompanyMenuViewModel(CompanyListViewModel<T> list,
                                    EditCompanyViewModel<T> edit)
        {
            _list = list;
            _edit = edit;

            list.Parent = this;
            _edit.Parent = this;

            SwitchToList();
        }

        public async Task SwitchToList()
        {
            Content = _list;
            _list.Load();
        }

        public async Task SwitchToEdit(Guid id)
        {
            Content = _edit;
            _edit.Load(id);
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.Menu);
        }
    }

    public class ClientMenuPageViewModel : CompanyMenuViewModel<CompanyDto>
    {
        public ClientMenuPageViewModel(CompanyListViewModel<CompanyDto> list, EditCompanyViewModel<CompanyDto> edit) : base(list, edit)
        {
        }
    }

    public class CarrierMenuPageViewModel : CompanyMenuViewModel<CarrierDto>
    {
        public CarrierMenuPageViewModel(CompanyListViewModel<CarrierDto> list, EditCompanyViewModel<CarrierDto> edit) : base(list, edit)
        {
        }
    }
}
