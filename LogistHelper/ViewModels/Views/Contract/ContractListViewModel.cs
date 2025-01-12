using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.Models;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using ServerClient;
using Shared;
using System.Collections.ObjectModel;

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

        #endregion Private


        #region Public

        public ContractSearchProperty SelectedSearchProperty 
        {
            get => _selectedSearchProperty;
            set => SetProperty(ref _selectedSearchProperty, value);
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

        public ContractListViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<ContractDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
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

        public override async Task Load()
        {
            IsBlock = true;
            BlockText = "Загрузка";

            List?.Clear();

            RequestResult<IEnumerable<ContractDto>> result = await _client.GetRange<ContractDto>(_startIndex, _count);

            if (result.IsSuccess)
            {
                int counter = _startIndex + 1;
                List = new ObservableCollection<DataViewModel<ContractDto>>(result.Result.Select(c => _factory.GetViewModel(c, counter++)));
            }

            IsForwardAwaliable = List != null && List.Count == _count;
            IsBackwardAwaliable = _startIndex > _count;

            IsBlock = false;
        }
    }
}
