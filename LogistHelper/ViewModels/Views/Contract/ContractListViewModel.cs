using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using Shared;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    public class ContractListViewModel : MainListViewModel<ContractDto>
    {
        #region Private

        private string _searchString;
        private ContractStatus _selectedStatus;
        private DateTime _startDate;
        private DateTime _endDate;

        #endregion Private


        #region Public

        public string SearchString 
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
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

        public override KeyValuePair<string, string> SelectedFilter 
        { 
            get => base.SelectedFilter;
            set 
            { 
                base.SelectedFilter = value;

                SearchString = string.Empty;

                DateTime now = DateTime.Now;
                StartDate = new DateTime(now.Year, now.Month, 1);
                EndDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
            }
        }

        #endregion Public

        #region Commands

        public ICommand AddDocumentCommand { get; set; }
        public ICommand SetFailCommand { get; set; }

        #endregion Commands

        public ContractListViewModel(ISettingsRepository<Settings> repository, 
                                     IViewModelFactory<ContractDto> factory, 
                                     IDialog dialog) : base(repository, factory, dialog)
        {
            IsBackwardAwaliable = false;
            IsForwardAwaliable = false;

            SearchFirters = new Dictionary<string, string>()
            {
                {  nameof(ContractDto.CreationDate), "Дата создания" },
                {  nameof(ContractDto.Number), "Номер" },
                {  nameof(ContractDto.Status), "Статус" },
                {  nameof(ContractDto.Client),"Заказчик" },
                {  nameof(ContractDto.Carrier),"Перевочик" },
                {  nameof(ContractDto.Driver), "Водитель" },
                {  nameof(ContractDto.Vehicle),"ТС" },
                {  nameof(ContractDto.LoadPoint), "Загрузка" },
                {  nameof(ContractDto.UnloadPoints), "Выгрузка" },
            };

            SelectedFilter = SearchFirters.FirstOrDefault(p => p.Key == nameof(ContractDto.CreationDate));

            #region CommandsInit

            ResetFilterCommand = new RelayCommand(async () =>
            {
                SelectedFilter = SearchFirters.FirstOrDefault(p => p.Key == nameof(ContractDto.CreationDate));

                await Load();
            });

            AddDocumentCommand = new RelayCommand<Guid>((id) => 
            {
                (MainParent as ISubParent).SwitchToSub(id);
            });

            SetFailCommand = new RelayCommand<Guid>(async (id) => 
            {
                if (_dialog.ShowQuestion("После установки статуса заявки - Срыв, восстановить статус будет невозможно. Вы уверены?")) 
                { 
                    await _client.UpdateContractStatus(id, ContractStatus.Failed);

                    await Load();
                }
            });

            #endregion CommandsInit
        }

        protected override async Task FilterCommandExecutor()
        {
            if (SelectedFilter.Key == nameof(ContractDto.CreationDate))
            {
                await Filter(SelectedFilter.Key, StartDate.ToString(), EndDate.ToString());
            }
            else if (SelectedFilter.Key == nameof(ContractDto.Status))
            {
                await Filter(SelectedFilter.Key, SelectedStatus.ToString());
            }
            else 
            {
                await Filter(SelectedFilter.Key, SearchString);
            }
        }

        public override async Task Load()
        {
            IsBlock = true;
            BlockText = "Загрузка";

            await FilterCommandExecutor();

            IsBlock = false;
        }
    }
}
