using CommunityToolkit.Mvvm.Input;
using DTOs;
using DTOs.Dtos;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Microsoft.Win32;
using Shared;
using System.Windows.Input;
using Utilities;

namespace LogistHelper.ViewModels.Views
{
    public class ContractListViewModel : MainListViewModel<ContractDto>
    {
        #region Private

        private string _searchString;
        private ContractStatus _selectedStatus;
        private DateTime _startDate;
        private DateTime _endDate;
        private IFileLoader<FileViewModel> _fileLoader;

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
        public ICommand DownloadFileCommand { get; set; }

        #endregion Commands

        public ContractListViewModel(IDataAccess repository, 
                                     IViewModelFactory<ContractDto> factory, 
                                     IDialog dialog,
                                     IFileLoader<FileViewModel> fileLoader) : base(repository, factory, dialog)
        {
            IsBackwardAwaliable = false;
            IsForwardAwaliable = false;
            _fileLoader = fileLoader;

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
                    await _client.UpdatePropertyAsync<ContractDto>(id, new KeyValuePair<string, object>("Status", ContractStatus.Failed.ToString()));

                    await Load();
                }
            });

            DownloadFileCommand = new RelayCommand<Guid>(async (id) =>
            {
                OpenFolderDialog folderDialog = new OpenFolderDialog();

                if (folderDialog.ShowDialog() == true)
                {
                    string directory = folderDialog.FolderName;

                    var fileResult = await _client.GetFilteredAsync<FileDto>(nameof(FileDto.DtoId), id.ToString());

                    if (fileResult.IsSuccess)
                    {
                        var fileViewModel = new FileViewModel(fileResult.Result.First());

                        if (await _fileLoader.DownloadFile(directory, fileViewModel))
                        {
                            _dialog.ShowSuccess("Файл сохранен");
                        }
                        else
                        {
                            _dialog.ShowError("Ошибка сохранения файлов");
                        }
                    }
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
