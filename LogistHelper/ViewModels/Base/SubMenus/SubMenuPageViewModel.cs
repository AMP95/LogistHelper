using DTOs.Dtos;
using LogistHelper.Services;

namespace LogistHelper.ViewModels.Base
{
    public class SubMenuPageViewModel<T> : BasePageViewModel, ISubMenuPage<T> where T : IDto
    {
        private object _content;

        protected ISubListView<T> _list;
        protected ISubEditView<T> _edit;

        public object Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public SubMenuPageViewModel(ISubListView<T> list,
                                    ISubEditView<T> edit)
        {
            _list = list;
            _edit = edit;

            _list.SubParent = this;
            _edit.SubParent = this;
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.MainMenu);
        }

        public async Task SwitchToEdit(Guid id, Guid mainId)
        {
            Content = _edit;
            await _edit.Load(id, mainId);
        }

        public async Task SwitchToList(Guid mainId)
        {
            Content = _list;
            await _list.Load(mainId);
            
        }
    }
}
