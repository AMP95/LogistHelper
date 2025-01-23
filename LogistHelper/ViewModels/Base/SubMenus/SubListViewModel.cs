using CommunityToolkit.Mvvm.Input;
using DTOs.Dtos;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Base
{
    public class SubListViewModel<T> : BlockedViewModel, ISubListView<T> where T : IDto
    {
        #region Private

        protected IDialog _dialog;
        protected IDataAccess _client;

        protected IViewModelFactory<T> _factory;

        private ObservableCollection<DataViewModel<T>> _list;

        protected string _mainPropertyName;

        #endregion Private

        #region Public

        public ISubMenuPage<T> SubParent { get; set; }

        public Guid MainId { get; set; }

        public ObservableCollection<DataViewModel<T>> List
        {
            get => _list;
            set => SetProperty(ref _list, value);
        }

        #endregion Public

        #region Commands

        public ICommand EditCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        #endregion Commands

        public SubListViewModel(IDataAccess access,
                                IViewModelFactory<T> factory,
                                IDialog dialog)
        {
            _dialog = dialog;
            _factory = factory;
            _client = access;

            #region CommandInit

            EditCommand = new RelayCommand<Guid>((id) =>
            {
                SubParent?.SwitchToEdit(id, MainId);
                Clear();
            });

            AddCommand = new RelayCommand(() =>
            {
                SubParent?.SwitchToEdit(Guid.Empty, MainId);
            });

            DeleteCommand = new RelayCommand<Guid>(async (id) =>
            {
                if (_dialog.ShowSure("Удаление"))
                {
                    await Delete(id);
                }
            });

            #endregion CommandInit

        }
        public virtual void Clear()
        {
            List?.Clear();
        }


        public virtual async Task Load(Guid mainId)
        {
            if (mainId == Guid.Empty) 
            { 
                return;
            }

            MainId = mainId;

            IsBlock = true;
            BlockText = "Загрузка";

            List?.Clear();

            IAccessResult<IEnumerable<T>> result = await _client.GetFilteredAsync<T>(_mainPropertyName, mainId.ToString());

            if (result.IsSuccess)
            {
                int counter = 1;
                List = new ObservableCollection<DataViewModel<T>>(result.Result.Select(c => _factory.GetViewModel(c, counter++)));
            }


            IsBlock = false;
        }

        public virtual async Task Delete(Guid id)
        {
            IsBlock = true;
            BlockText = "Удаление";

            IAccessResult<bool> result = await _client.DeleteAsync<T>(id);

            if (result.IsSuccess)
            {
                await Load(MainId);
            }
            else
            {
                _dialog.ShowError("Не удалось удалить объект.");
                IsBlock = false;
            }
        }
    }
}
