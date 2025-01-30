using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    public class ClientListViewModel : MainListViewModel<ClientDto>
    {
        private string _searchString;
        private bool _isPriority;

        public string SearchString 
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
        }

        public bool IsPriority 
        { 
            get => _isPriority;
            set => SetProperty(ref _isPriority, value);
        }

        public override KeyValuePair<string, string> SelectedFilter 
        { 
            get => base.SelectedFilter;
            set 
            { 
                base.SelectedFilter = value;
                SearchString = string.Empty;
                IsPriority = false;
            }
        }

        public ClientListViewModel(IDataAccess repository, IViewModelFactory<ClientDto> factory, IMessageDialog dialog) : base(repository, factory, dialog)
        {
            SearchFirters = new Dictionary<string, string>()
            {
                { nameof(ClientDto.Name), "Название"  },
                { nameof(ClientDto.InnKpp), "ИНН/КПП" },
                { nameof(ClientDto.IsPriority),  "Приоритет" },
            };

            SelectedFilter = SearchFirters.FirstOrDefault(p => p.Key == nameof(ClientDto.Name));

            ResetFilterCommand = new RelayCommand(async () =>
            {
                SearchString = null;
                IsPriority = false;

                await Load();
            });
        }

        protected override async Task FilterCommandExecutor()
        {
            if (SelectedFilter.Key == nameof(ClientDto.IsPriority))
            {
                await Filter(SelectedFilter.Key, IsPriority.ToString());
            }
            else 
            {
                await Filter(SelectedFilter.Key, SearchString);
            }
        }
    }

    public class CarrierListViewModel : MainListViewModel<CarrierDto>
    {
        private string _searchString;
        private VAT _selectedVat;

        public string SearchString
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
        }

        public VAT SelectedVat 
        {
            get => _selectedVat;
            set => SetProperty(ref _selectedVat, value);
        }

        public override KeyValuePair<string, string> SelectedFilter
        {
            get => base.SelectedFilter;
            set
            {
                base.SelectedFilter = value;
                SearchString = string.Empty;
            }
        }

        public CarrierListViewModel(IDataAccess repository, IViewModelFactory<CarrierDto> factory, IMessageDialog dialog) : base(repository, factory, dialog)
        {
            SearchFirters = new Dictionary<string, string>()
            {
                { nameof(CarrierDto.Name) , "Название"},
                { nameof(CarrierDto.InnKpp),  "ИНН/КПП" },
                { nameof(CarrierDto.Vat), "Налог"  },
            };

            SelectedFilter = SearchFirters.FirstOrDefault(p => p.Key == nameof(CarrierDto.Name));

            ResetFilterCommand = new RelayCommand(async () =>
            {
                SearchString = null;

                await Load();
            });
        }

        protected override async Task FilterCommandExecutor()
        {
            if (SelectedFilter.Key == nameof(CarrierDto.Vat))
            {
                await Filter(SelectedFilter.Key, SelectedVat.ToString());
            }
            else
            {
                await Filter(SelectedFilter.Key, SearchString);
            }
        }
    }
}
