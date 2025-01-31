using CommunityToolkit.Mvvm.Input;
using DTOs.Dtos;
using LogistHelper.ViewModels.Base;
using Shared;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    public class LogistListViewModel : MainListViewModel<LogistDto>
    {
        private string _searchString;

        public string SearchString
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
        }

        #region Commands

        public ICommand AddAccessCommand { get;  }
        public ICommand RemoveAccessCommand { get; }

        #endregion Commands

        public LogistListViewModel(IDataAccess access, 
                                   IViewModelFactory<LogistDto> factory, 
                                   IMessageDialog dialog) : base(access, factory, dialog)
        {
            SearchFirters = new Dictionary<string, string>()
            {
                {  nameof(LogistDto.Name), "Имя"  },
                {  nameof(LogistDto.Login), "Логин"  },
            };

            SelectedFilter = SearchFirters.FirstOrDefault(p => p.Key == nameof(LogistDto.Name));

            ResetFilterCommand = new RelayCommand(async () =>
            {
                SearchString = string.Empty;
                await Load();
            });

            AddAccessCommand = new RelayCommand<Guid>(async(id) => 
            {
                IAccessResult<bool> result = await _client.UpdatePropertyAsync<LogistDto>(id, new KeyValuePair<string, object>(nameof(LogistDto.IsExpired), false));

                if (!result.IsSuccess) 
                {
                    _dialog.ShowError("Не удалось изменить доступ");
                }

                await Load();
            });

            RemoveAccessCommand = new RelayCommand<Guid>(async (id) => 
            {
                IAccessResult<bool> result = await _client.UpdatePropertyAsync<LogistDto>(id, new KeyValuePair<string, object>(nameof(LogistDto.IsExpired), true));

                if (!result.IsSuccess)
                {
                    _dialog.ShowError("Не удалось изменить доступ");
                }

                await Load();
            });
        }

        protected override async Task FilterCommandExecutor()
        {
            await Filter(SelectedFilter.Key, SearchString);
        }
    }
}
