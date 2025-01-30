using CommunityToolkit.Mvvm.Input;
using DTOs.Dtos;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Base
{
    public class MainListViewModel<T> : BlockedViewModel, IMainListView<T> where T : IDto
    {
        #region Private

        protected IMessageDialog _dialog;
        protected IDataAccess _client;
        protected IViewModelFactory<T> _factory;

        protected int _startIndex = 0;
        protected int _count = 20;

        private bool _isForwardAwaliable;
        private bool _isBackwardAwaliable;

        private string _searchString;
        private ObservableCollection<DataViewModel<T>> _list;

        private Dictionary<string, string> _searchFirters;
        private KeyValuePair<string, string> _selectedFilter;

        #endregion Private

        #region Public

        public IMainMenuPage<T> MainParent { get; set; }

        public bool IsForwardAwaliable
        {
            get => _isForwardAwaliable;
            set => SetProperty(ref _isForwardAwaliable, value);
        }

        public bool IsBackwardAwaliable
        {
            get => _isBackwardAwaliable;
            set => SetProperty(ref _isBackwardAwaliable, value);
        }

        public ObservableCollection<DataViewModel<T>> List
        {
            get => _list;
            set => SetProperty(ref _list, value);
        }

        public Dictionary<string, string> SearchFirters
        {
            get => _searchFirters;
            set => SetProperty(ref _searchFirters, value);
        }

        public virtual KeyValuePair<string, string> SelectedFilter 
        {
            get => _selectedFilter;
            set => SetProperty(ref _selectedFilter, value);
        }

        #endregion Public

        #region Commands

        public ICommand BackwardCommand { get; set; }
        public ICommand ForwardCommand { get; set; }

        public ICommand FilterCommand { get; set; }
        public ICommand ResetFilterCommand { get; set; }

        public ICommand EditCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        #endregion Commands

        public MainListViewModel(IDataAccess access, 
                                 IViewModelFactory<T> factory, 
                                 IMessageDialog dialog)
        {
            _dialog = dialog;
            _factory = factory;
            _client = access;

            #region CommandsInit

            BackwardCommand = new RelayCommand(async () =>
            {
                if (IsBackwardAwaliable)
                {
                    _startIndex -= _count;
                    await Load();
                }
            });

            ForwardCommand = new RelayCommand(async () =>
            {
                if (IsForwardAwaliable)
                {
                    _startIndex += _count;
                    await Load();
                }
            });

            FilterCommand = new RelayCommand(async () =>
            {
                await FilterCommandExecutor();
            });


            ResetFilterCommand = new RelayCommand(async () =>
            {
                await Load();
            });

            EditCommand = new RelayCommand<Guid>((id) =>
            {
                MainParent?.SwitchToEdit(id);
                Clear();
            });

            AddCommand = new RelayCommand(() =>
            {
                MainParent?.SwitchToEdit(Guid.Empty);
            });

            DeleteCommand = new RelayCommand<Guid>(async (id) =>
            {
                if (_dialog.ShowSure("Удаление"))
                {
                    await Delete(id);
                }
            });


            #endregion CommandsInit
        }

        protected virtual Task FilterCommandExecutor() { return Task.CompletedTask; }

        public virtual void Clear()
        {
            List?.Clear();
        }

        public virtual async Task Load()
        {
            IsBlock = true;
            BlockText = "Загрузка";

            List?.Clear();

            IAccessResult<IEnumerable<T>> result = await _client.GetRangeAsync<T>(_startIndex, _count);

            if (result.IsSuccess)
            {
                int counter = _startIndex + 1;
                List = new ObservableCollection<DataViewModel<T>>(result.Result.Select(c => _factory.GetViewModel(c, counter++)));
            }

            IsForwardAwaliable = List != null && List.Count == _count;
            IsBackwardAwaliable = _startIndex > _count;

            IsBlock = false;
        }

        public virtual async Task Filter(string proerty, params string[] param) 
        {
            IsBlock = true;
            BlockText = "Поиск";

            List?.Clear();

            IAccessResult<IEnumerable<T>> result = await _client.GetFilteredAsync<T>(proerty, param);

            if (result.IsSuccess)
            {
                int counter = 1;
                List = new ObservableCollection<DataViewModel<T>>(result.Result.Select(c => _factory.GetViewModel(c, counter++)));
            }

            IsForwardAwaliable = false;
            IsBackwardAwaliable = false;

            IsBlock = false;
        }

        public virtual async Task Delete(Guid id)
        {
            IsBlock = true;
            BlockText = "Удаление";

            IAccessResult<bool> result = await _client.DeleteAsync<T>(id);

            if (result.IsSuccess)
            {
                await Load();
            }
            else
            {
                _dialog.ShowError("Не удалось удалить объект.");
                IsBlock = false;
            }
        }
    }
}
