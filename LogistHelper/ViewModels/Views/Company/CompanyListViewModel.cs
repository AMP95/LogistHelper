using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using LogistHelper.ViewModels.Pages;
using ServerClient;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    public class CompanyListViewModel<T> : BlockedViewModel where T : CompanyDto
    {
        #region Private

        IDialog _dialog;

        private ICompanyVmFactory<T> _factory;
        private Settings _settings;
        private ApiClient _client;

        private int _startIndex = 0;
        private int _count = 20;

        private bool _isForwardAwaliable;
        private bool _isBackwardAwaliable;

        private string _searchString;
        private ObservableCollection<CompanyViewModel<T>> _shownCarriers;

        #endregion Private

        #region Public

        public CompanyMenuViewModel<T> Parent;

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

        public ObservableCollection<CompanyViewModel<T>> ShownCarriers
        {
            get => _shownCarriers;
            set => SetProperty(ref _shownCarriers, value);
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

        public CompanyListViewModel(ISettingsRepository<Settings> repository, ICompanyVmFactory<T> vmFactory, IDialog dialog)
        {
            _settings = repository.GetSettings();
            _client = new ApiClient(_settings.ServerUri);
            _dialog = dialog;
            _factory = vmFactory;

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

        public async Task Load()
        {
            IsBlock = true;
            BlockText = "Загрузка";

            ShownCarriers?.Clear();

            RequestResult<IEnumerable<T>> result = await _client.GetRange<T>(_startIndex, _count);

            if (result.IsSuccess)
            {
                int counter = _startIndex + 1;
                ShownCarriers = new ObservableCollection<CompanyViewModel<T>>(result.Result.Select(c => _factory.GetViewModel(c, counter++)));
            }

            IsForwardAwaliable = ShownCarriers != null && ShownCarriers.Count == _count;
            IsBackwardAwaliable = _startIndex > _count;

            IsBlock = false;
        }

        public async Task Search()
        {
            IsBlock = true;
            BlockText = "Поиск";

            ShownCarriers?.Clear();

            RequestResult<IEnumerable<T>> result = await _client.Search<T>(SearchString);

            if (result.IsSuccess)
            {
                int counter = 1;
                ShownCarriers = new ObservableCollection<CompanyViewModel<T>>(result.Result.Select(c => _factory.GetViewModel(c, counter++)));
            }

            IsForwardAwaliable = false;
            IsBackwardAwaliable = false;

            IsBlock = false;

        }

        public async Task Delete(Guid id)
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

    public class ClientListViewModel : CompanyListViewModel<CompanyDto>
    {
        public ClientListViewModel(ISettingsRepository<Settings> repository, ICompanyVmFactory<CompanyDto> vmFactory, IDialog dialog) : base(repository, vmFactory, dialog)
        {
        }
    }

    public class CarrierListViewModel : CompanyListViewModel<CarrierDto>
    {
        public CarrierListViewModel(ISettingsRepository<Settings> repository, ICompanyVmFactory<CarrierDto> vmFactory, IDialog dialog) : base(repository, vmFactory, dialog)
        {
        }
    }


}
