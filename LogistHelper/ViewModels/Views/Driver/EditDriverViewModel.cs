using CommunityToolkit.Mvvm.Input;
using Dadata;
using Dadata.Model;
using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using ServerClient;
using Shared;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    public class EditDriverViewModel : EditViewModel<DriverDto>
    {
        private DriverViewModel _driver;
        private List<CarrierViewModel> _carriers;
        private CarrierViewModel _selectedCarrier;

        private string _issuerCode;

        #region Public

        public List<CarrierViewModel> Carriers 
        {
            get => _carriers;
            set => SetProperty(ref _carriers, value);
        }

        public CarrierViewModel SelectedCarrier 
        {
            get => _selectedCarrier;
            set 
            { 
                SetProperty(ref _selectedCarrier, value);
            }
        }

        public string IssuerCode
        {
            get => _issuerCode;
            set => SetProperty(ref _issuerCode, value);
        }

        #endregion Public



        #region Commands

        public ICommand DeletePhoneCommand { get; set; }
        public ICommand AddPhoneCommand { get; set; }

        public ICommand SearchPassportIssuerCommand { get; set; }

        #endregion Commands

        public EditDriverViewModel(ISettingsRepository<Settings> repository, 
                                   IViewModelFactory<DriverDto> factory, 
                                   IDialog dialog) : base(repository, factory, dialog)
        {
            #region CommandsInit

            AddPhoneCommand = new RelayCommand(() =>
            {
                _driver.Phones.Add(new StringItem());
            });

            DeletePhoneCommand = new RelayCommand<Guid>((id) =>
            {
                StringItem item = _driver.Phones.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    _driver.Phones.Remove(item);
                }
            });

            SearchPassportIssuerCommand = new RelayCommand(async () =>
            {
                await Search();
            });

            #endregion CommandsInit
        }

        public override async Task Load(Guid id)
        {
            //if (id == Guid.Empty)
            //{
            //    EditedViewModel = _factory.GetViewModel();
            //}
            //else
            //{
            //    IsBlock = true;
            //    BlockText = "Загрузка";

            //    RequestResult<T> result = await _client.GetId<T>(id);

            //    if (result.IsSuccess)
            //    {
            //        EditedViewModel = _factory.GetViewModel(result.Result);
            //    }

            //    IsBlock = false;
            //}
            _driver = EditedViewModel as DriverViewModel;
        }

        public async Task Search()
        {
            var api = new OutwardClientAsync(_settings.DaDataApiKey);
            var response = await api.Suggest<FmsUnit>(IssuerCode);
            var result = response.suggestions.FirstOrDefault();

            if (result != null) 
            {
                _driver.PassportIssuer = result.value;
            }
        }
    }
}
