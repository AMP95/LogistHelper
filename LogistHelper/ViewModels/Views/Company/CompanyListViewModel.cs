using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    public class CompanyListViewModel : MainListViewModel<CompanyDto>
    {
        private string _searchString;

        public string SearchString 
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
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

        public CompanyListViewModel(IDataAccess repository, IViewModelFactory<CompanyDto> factory, IMessageDialog dialog) : base(repository, factory, dialog)
        {
            SearchFirters = new Dictionary<string, string>()
            {
                { nameof(CompanyDto.Name), "Название"  },
                { nameof(CompanyDto.InnKpp), "ИНН/КПП" }
            };

            SelectedFilter = SearchFirters.FirstOrDefault(p => p.Key == nameof(CompanyDto.Name));

            ResetFilterCommand = new RelayCommand(async () =>
            {
                SearchString = null;

                await Load();
            });
        }

        protected override async Task FilterCommandExecutor()
        {
            await Filter(SelectedFilter.Key, SearchString);
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
