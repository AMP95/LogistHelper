using DTOs.Dtos;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base.Interfaces;

namespace LogistHelper.ViewModels.Base
{

    public class MenuPageViewModel<T> : BasePageViewModel, IMainMenuPage<T> where T : IDto
    {
        private object _content;

        protected IMainListView<T> _list;
        protected IMainEditView<T> _edit;

        public object Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public MenuPageViewModel(IMainListView<T> list,
                                 IMainEditView<T> edit)
        {
            _list = list;
            _edit = edit;

            _list.Parent = this;
            _edit.Parent = this;

            Init();
        }

        protected virtual async Task Init() 
        {
            await SwitchToList();
        }

        public async Task SwitchToList()
        {
            Content = _list;
            await _list.Load();
        }

        public async Task SwitchToEdit(Guid id)
        {
            Content = _edit;
            await _edit.Load(id);
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.MainMenu);
        }
    }
}
