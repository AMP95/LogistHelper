using CommunityToolkit.Mvvm.Input;
using DTOs.Dtos;
using LogistHelper.Models.Settings;
using ServerClient;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Base
{
    public class ListViewModel<T> : BlockedViewModel where T : IDto
    {
        #region Private

        protected Settings _settings;
        protected IDialog _dialog;
        protected ApiClient _client;

        private IViewModelFactory<T> _factory;

        protected int _startIndex = 0;
        protected int _count = 20;

        private bool _isForwardAwaliable;
        private bool _isBackwardAwaliable;

        private string _searchString;
        private ObservableCollection<DataViewModel<T>> _list;

        #endregion Private

        #region Public

        public MenuPageViewModel<T> Parent;

        public string SearchString
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
        }

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

        #endregion Public

        #region Commands

        public ICommand BackwardCommand { get; set; }
        public ICommand ForwardCommand { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand ResetSearchCommand { get; set; }

        public ICommand EditCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        #endregion Commands

        public ListViewModel(ISettingsRepository<Settings> repository,
                             IViewModelFactory<T> factory, 
                             IDialog dialog)
        {
            _settings = repository.GetSettings();
            _dialog = dialog;
            _factory = factory;
            _client = new ApiClient(_settings.ServerUri);

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

            SearchCommand = new RelayCommand(async () =>
            {
                await Search();
            });


            ResetSearchCommand = new RelayCommand(async () =>
            {
                SearchString = string.Empty;
                await Load();
            });

            EditCommand = new RelayCommand<Guid>((id) =>
            {
                Parent.SwitchToEdit(id);
                Clear();
            });

            AddCommand = new RelayCommand(() =>
            {
                Parent.SwitchToEdit(Guid.Empty);
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

        public virtual void Clear()
        {
            List = null;
        }

        public virtual async Task Load()
        {
            IsBlock = true;
            BlockText = "Загрузка";

            List?.Clear();

            RequestResult<IEnumerable<T>> result = await _client.GetRange<T>(_startIndex, _count);

            if (result.IsSuccess)
            {
                int counter = _startIndex + 1;
                List = new ObservableCollection<DataViewModel<T>>(result.Result.Select(c => _factory.GetViewModel(c, counter++)));
            }

            IsForwardAwaliable = List != null && List.Count == _count;
            IsBackwardAwaliable = _startIndex > _count;

            IsBlock = false;
        }

        public virtual async Task Search()
        {
            IsBlock = true;
            BlockText = "Поиск";

            List?.Clear();

            RequestResult<IEnumerable<T>> result = await _client.Search<T>(SearchString);

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

            RequestResult<bool> result = await _client.Delete<T>(id);

            if (result.IsSuccess)
            {
                await Load();
            }
            else
            {
                _dialog.ShowError("Не удалось удалить перевозчика. Удаление невозможно если с перевозчиком есть заключенные заявки.");
                IsBlock = false;
            }
        }
    }
}
