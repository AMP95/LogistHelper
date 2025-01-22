using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    internal class VehicleListViewModel : MainListViewModel<VehicleDto>
    {
        private string _searchString;

        public string SearchString
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
        }
        public VehicleListViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<VehicleDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {

            SearchFirters = new Dictionary<string, string>()
            {
                { nameof(VehicleDto.Carrier), "Перевозчик"  },
                { nameof(VehicleDto.TruckNumber) , "Номер" },
            };

            SelectedFilter = SearchFirters.FirstOrDefault( p => p.Key == nameof(VehicleDto.Carrier));

            ResetFilterCommand = new RelayCommand(async () =>
            {
                SearchString = string.Empty;
                await Load();
            });
        }

        protected override async Task FilterCommandExecutor()
        {
            await Filter(SelectedFilter.Key, SearchString);
        }
    }
}
