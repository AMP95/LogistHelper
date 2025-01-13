using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.Models;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using ServerClient;
using Shared;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    public class ContractListViewModel : ListViewModel<ContractDto>
    {
        #region Private

        private ContractSearchProperty _selectedSearchProperty;

        private string _textSearchString;
        private ContractStatus _selectedStatus;
        private DateTime _startDate;
        private DateTime _endDate;
        private ObservableCollection<StringItem> _searchResults;
        private StringItem _selectedSearch;

        #endregion Private


        #region Public

        public ObservableCollection<StringItem> SearchResults 
        { 
            get => _searchResults;
            set => SetProperty(ref _searchResults, value);
        }

        public StringItem SelectedSearch 
        {
            get => _selectedSearch;
            set 
            { 
                SetProperty(ref _selectedSearch, value);
                TextSearchString = SelectedSearch?.Item;
            }
        }

        public ContractSearchProperty SelectedSearchProperty 
        {
            get => _selectedSearchProperty;
            set
            {
                SetProperty(ref _selectedSearchProperty, value);
                TextSearchString = string.Empty;
            }
        }

        public string TextSearchString 
        {
            get => _textSearchString;
            set => SetProperty(ref _textSearchString, value);
        }

        public ContractStatus SelectedStatus
        {
            get => _selectedStatus;
            set => SetProperty(ref _selectedStatus, value);
        }

        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        #endregion Public

        #region Commands

        public ICommand SearchClientCommand { get; }
        public ICommand SearchCarrierCommand { get; }
        public ICommand SearchDriverCommand { get; }
        public ICommand FiltrateCommand { get; }

        #endregion Commands

        public ContractListViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<ContractDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
            DateTime now = DateTime.Now;
            StartDate = new DateTime(now.Year, now.Month, 1);
            EndDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

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

            SearchClientCommand = new RelayCommand<string>(async (searchString) => 
            {
                await SearchCarrier(searchString);
            });

            SearchCarrierCommand = new RelayCommand<string>(async (searchString) =>
            {
                await SearchClient(searchString);
            });

            SearchDriverCommand = new RelayCommand<string>(async (searchString) =>
            {
                await SearchDriver(searchString);
            });

            #endregion CommandsInit
        }

        public override async Task Load()
        {
            IsBlock = true;
            BlockText = "Загрузка";

            List?.Clear();

            _startIndex = (int)StartDate.ToOADate();
            _count = (int)EndDate.ToOADate();

            RequestResult<IEnumerable<ContractDto>> result = await _client.GetRange<ContractDto>(_startIndex, _count);

            if (result.IsSuccess)
            {
                int counter = 0;
                List = new ObservableCollection<DataViewModel<ContractDto>>(result.Result.Select(c => _factory.GetViewModel(c, counter++)));
            }

            IsForwardAwaliable = StartDate.Month != DateTime.Now.Month && StartDate.Year != DateTime.Now.Year;
            IsBackwardAwaliable = List.Any();

            IsBlock = false;
        }

        private async Task SearchCarrier(string searchString)
        {
            await Task.Run(async () =>
            {
                RequestResult<IEnumerable<CarrierDto>> result = await _client.Search<CarrierDto>(searchString);

                SearchResults = null;

                if (result.IsSuccess)
                {
                    SearchResults = new ObservableCollection<StringItem>(result.Result.Select(v => new StringItem(v.Name)));
                }
            });
        }

        private async Task SearchClient(string searchString)
        {
            await Task.Run(async () =>
            {
                RequestResult<IEnumerable<CompanyDto>> result = await _client.Search<CompanyDto>(searchString);

                SearchResults = null;

                if (result.IsSuccess)
                {
                    SearchResults = new ObservableCollection<StringItem>(result.Result.Select(v => new StringItem(v.Name)));
                }
            });
        }

        private async Task SearchDriver(string searchString)
        {
            await Task.Run(async () =>
            {
                RequestResult<IEnumerable<DriverDto>> result = await _client.Search<DriverDto>(searchString);

                SearchResults = null;

                if (result.IsSuccess)
                {
                    SearchResults = new ObservableCollection<StringItem>(result.Result.Select(v => new StringItem(v.Name)));
                }
            });
        }
    }
}
