using CommunityToolkit.Mvvm.ComponentModel;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.Views;

namespace LogistHelper.ViewModels.Pages
{
    public class CarrierMenuViewModel : BasePageViewModel
    {
        private ObservableObject _content;

        private CarrierListViewModel _list;
        private EditCarrierViewModel _editCarrier;


        #region Public

        public ObservableObject Content 
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        #endregion Public

        public CarrierMenuViewModel(CarrierListViewModel list,
                                    EditCarrierViewModel edit)
        {
            _list = list;
            _editCarrier = edit;

            list.Parent = this;
            _editCarrier.Parent = this;

            SwitchToList();
        }

        public async Task SwitchToList() 
        { 
            Content = _list;
            _list.Load();
        }

        public async Task SwitchToEdit(Guid id) 
        {
            Content = _editCarrier;
            _editCarrier.Load(id);
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.Menu);
        }
    }
}
