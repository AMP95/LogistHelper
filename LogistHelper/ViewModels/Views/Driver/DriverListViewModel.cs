using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    public class DriverListViewModel : MainListViewModel<DriverDto>
    {
        private string _searchString;

        public string SearchString
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
        }
        public DriverListViewModel(IDataAccess repository, IViewModelFactory<DriverDto> factory, IMessageDialog dialog) : base(repository, factory, dialog)
        {
            SearchFirters = new Dictionary<string, string>()
            {
                {  nameof(DriverDto.Name), "Имя"  },
                {  nameof(DriverDto.Carrier), "Перевозчик"  },
            };

            SelectedFilter = SearchFirters.FirstOrDefault(p => p.Key == nameof(VehicleDto.Carrier));

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
