using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.DataViewModels;
using LogistHelper.ViewModels.Pages;
using ServerClient;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    public class CarrierListViewModel : ObservableObject
    {
        #region Private

        private bool _isBlock;

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

        public bool IsBlock 
        {
            get => _isBlock;
            set => SetProperty(ref _isBlock, value);
        }

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

        #endregion Commands

        public CarrierListViewModel(ISettingsRepository<Settings> repository)
        {
            _settings = repository.GetSettings();
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
            });

            AddCommand = new RelayCommand(() =>
            {
                Parent.SwitchToEdit(Guid.Empty);
            });


            #endregion CommandsInit
        }

        public async Task Load() 
        {
            IsBlock = true;

            ShownCarriers?.Clear();

            RequestResult<IEnumerable<CarrierDto>> result = await _client.GetRange<CarrierDto>(_startIndex, _startIndex + _count);

            if (result.IsSuccess) 
            {
                ShownCarriers = new ObservableCollection<CarrierViewModel>(result.Result.Select(c => new CarrierViewModel(c)));
            }

            IsForwardAwaliable = ShownCarriers.Count == _count;
            IsBackwardAwaliable = _startIndex > _count;

            IsBlock = false;
        }

        public async Task Search() 
        {
            IsBlock = true;

            ShownCarriers?.Clear();

            RequestResult<IEnumerable<CarrierDto>> result = await _client.Search<CarrierDto>(SearchString);

            if (result.IsSuccess)
            {
                ShownCarriers = new ObservableCollection<CarrierViewModel>(result.Result.Select(c => new CarrierViewModel(c)));
            }

            IsForwardAwaliable = false;
            IsBackwardAwaliable = false;

            IsBlock = false;

        }
    }
}
