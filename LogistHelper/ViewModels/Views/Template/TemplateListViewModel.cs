using CommunityToolkit.Mvvm.Input;
using DTOs;
using DTOs.Dtos;
using LogistHelper.ViewModels.Base;
using Shared;

namespace LogistHelper.ViewModels.Views
{
    public class TemplateListViewModel : MainListViewModel<ContractTemplateDto>
    {
        private string _searchString;

        public string SearchString
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
        }
        public TemplateListViewModel(IDataAccess access, 
                                     IViewModelFactory<ContractTemplateDto> factory, 
                                     IMessageDialog dialog) : base(access, factory, dialog)
        {
            SearchFirters = new Dictionary<string, string>()
            {
                {  nameof(DriverDto.Name), "Имя"  }
            };
            SelectedFilter = SearchFirters.First();

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
