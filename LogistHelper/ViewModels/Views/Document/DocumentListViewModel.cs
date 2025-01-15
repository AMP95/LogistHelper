using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.Base.Interfaces;
using ServerClient;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    class DocumentListViewModel : BlockedViewModel,  ISubListView<DocumentDto>
    {
        #region Private

        protected Settings _settings;
        protected IDialog _dialog;
        protected ApiClient _client;

        protected IViewModelFactory<DocumentDto> _factory;
        private ObservableCollection<DataViewModel<DocumentDto>> _list;

        #endregion Private

        #region Public
        public ISubMenuPage<DocumentDto> SubParent { get; set; }
        public Guid MainId { get; set; }
        public ObservableCollection<DataViewModel<DocumentDto>> List
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

        public DocumentListViewModel(ISettingsRepository<Settings> repository, 
                                     IViewModelFactory<DocumentDto> factory, 
                                     IDialog dialog)
        {
            _settings = repository.GetSettings();
            _dialog = dialog;
            _factory = factory;
            _client = new ApiClient(_settings.ServerUri);

            #region CommandsInit

            EditCommand = new RelayCommand<Guid>((id) =>
            {
                SubParent.SwitchToSubEdit(id, MainId);
                Clear();
            });

            AddCommand = new RelayCommand(() =>
            {
                SubParent.SwitchToSubEdit(Guid.Empty, MainId);
                Clear();
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

        public void Clear()
        {
            List?.Clear();
        }

        public async Task Load(Guid id)
        {
            if (id == Guid.Empty) 
            {
                return;
            }

            MainId = id;

            IsBlock = true;
            BlockText = "Загрузка";

            List?.Clear();

            RequestResult<IEnumerable<DocumentDto>> result = await _client.GetMainId<DocumentDto>(MainId);

            if (result.IsSuccess)
            {
                int counter = 0;
                List = new ObservableCollection<DataViewModel<DocumentDto>>(result.Result.Select(c => _factory.GetViewModel(c, counter++)));
            }

            IsBlock = false;
        }

        public async Task Delete(Guid id)
        {
            IsBlock = true;
            BlockText = "Удаление";

            RequestResult<bool> result = await _client.Delete<DocumentDto>(id);

            if (result.IsSuccess)
            {
                await Load(MainId);
            }
            else
            {
                _dialog.ShowError("Не удалось удалить документ.");
                IsBlock = false;
            }
        }
    }
}