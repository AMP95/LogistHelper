using CommunityToolkit.Mvvm.ComponentModel;
using DTOs.Dtos;
using LogistHelper.Services;

namespace LogistHelper.ViewModels.Base
{
    public class MenuPageViewModel<T> : BasePageViewModel where T : IDto
    {
        private ObservableObject _content;

        protected ListViewModel<T> _list;
        protected EditViewModel<T> _edit;

        public ObservableObject Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public MenuPageViewModel(ListViewModel<T> list,
                                 EditViewModel<T> edit)
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
}
