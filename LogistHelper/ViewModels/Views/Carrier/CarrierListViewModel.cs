using CommunityToolkit.Mvvm.ComponentModel;
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
    public class CarrierListViewModel : BlockedViewModel
    {
        #region Private

        IDialog _dialog;

        private Settings _settings;
        private ApiClient _client;

        private int _startIndex = 0;
        private int _count = 20;

        private bool _isForwardAwaliable;
        private bool _isBackwardAwaliable;

        private string _searchString;
        private ObservableCollection<CarrierViewModel> _shownCarriers;

        #endregion Private

        #region Public

        public CarrierMenuViewModel Parent;

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

        public ObservableCollection<CarrierViewModel> ShownCarriers 
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

        public CarrierListViewModel(ISettingsRepository<Settings> repository, IDialog dialog)
        {
            _settings = repository.GetSettings();
            _client = new ApiClient(_settings.ServerUri);
            _dialog = dialog;

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

            RequestResult<IEnumerable<CarrierDto>> result = await _client.GetRange<CarrierDto>(_startIndex, _count);

            if (result.IsSuccess) 
            {
                int counter = _startIndex + 1;
                ShownCarriers = new ObservableCollection<CarrierViewModel>(result.Result.Select(c => new CarrierViewModel(c) { Number = counter++ }));
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

            RequestResult<IEnumerable<CarrierDto>> result = await _client.Search<CarrierDto>(SearchString);

            if (result.IsSuccess)
            {
                int counter = 1;
                ShownCarriers = new ObservableCollection<CarrierViewModel>(result.Result.Select(c => new CarrierViewModel(c) { Number = counter++ }));
            }

            IsForwardAwaliable = false;
            IsBackwardAwaliable = false;

            IsBlock = false;

        }

        public async Task Delete(Guid id) 
        {
            IsBlock = true;
            BlockText = "Удаление";

            RequestResult<bool> result = await _client.Delete<CarrierDto>(id);

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
