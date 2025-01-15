using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using LogistHelper.ViewModels.Pages;
using ServerClient;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    public class ContractListViewModel : ListViewModel<ContractDto>
    {
        #region Private

        private ContractFilterProperty _selectedSearchProperty;

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

        public ContractFilterProperty SelectedSearchProperty 
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

        public ICommand AddDocumentCommand { get; set; }
        public ICommand SearchClientCommand { get; }
        public ICommand SearchCarrierCommand { get; }
        public ICommand SearchDriverCommand { get; }
        public ICommand FiltrateCommand { get; }

        #endregion Commands

        public ContractListViewModel(ISettingsRepository<Settings> repository, 
                                     IViewModelFactory<ContractDto> factory, 
                                     IDialog dialog) : base(repository, factory, dialog)
        {
            SelectedSearchProperty = ContractFilterProperty.Date;

            DateTime now = DateTime.Now;
            StartDate = new DateTime(now.Year, now.Month, 1);
            EndDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

            IsBackwardAwaliable = false;
            IsForwardAwaliable = false;

            #region CommandsInit

            ResetSearchCommand = new RelayCommand(async () =>
            {
                DateTime now = DateTime.Now;
                StartDate = new DateTime(now.Year, now.Month, 1);
                EndDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
                SelectedSearchProperty = ContractFilterProperty.Date;
                await Load();
            });

            AddDocumentCommand = new RelayCommand<Guid>((id) => 
            {
                (Parent as ContractMenuViewModel).SwitchToDocument(id);
            });

            SearchCarrierCommand = new RelayCommand<string>(async (searchString) =>
            {
                await SearchCarrier(searchString);
            });

            SearchDriverCommand = new RelayCommand<string>(async (searchString) =>
            {
                await SearchDriver(searchString);
            });

            FiltrateCommand = new RelayCommand(async () => 
            {
                await Load();
            });

            #endregion CommandsInit
        }

        public override async Task Load()
        {
            IsBlock = true;
            BlockText = "Загрузка";

            List?.Clear();

            object[] param = null;

            switch (SelectedSearchProperty) 
            { 
                case ContractFilterProperty.Date: param = new object[] { StartDate, EndDate }; ; break;
                case ContractFilterProperty.Status: param = new object[] { SelectedStatus }; ; break;
                default:
                    param = new object[] { TextSearchString };
                    ;break;

            }

            RequestResult<IEnumerable<ContractDto>> result = await _client.GetFilter(SelectedSearchProperty, param);

            if (result.IsSuccess)
            {
                int counter = 1;
                List = new ObservableCollection<DataViewModel<ContractDto>>(result.Result.Select(c => _factory.GetViewModel(c, counter++)));
            }

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
