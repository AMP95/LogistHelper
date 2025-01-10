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
    public class EditVehicleViewModel : EditViewModel<VehicleDto>
    {
        #region Private

        private VehicleViewModel _vehicle;

        private IEnumerable<DataViewModel<CarrierDto>> _carriers;
        private DataViewModel<CarrierDto> _selectedCarrier;

        private IViewModelFactory<CarrierDto> _carrierFactory;

        private string _searchTruckBrand;
        private IEnumerable<StringItem> _truckBrands;
        private IEnumerable<StringItem> _trailerBrands;

        private string _searchTrailerBrand;
        private StringItem _selectedTruckBrand;
        private StringItem _selectedTrailerBrand;

        #endregion Private

        #region Public

        public string SearchTruckString 
        {
            get => _searchTruckBrand;
            set => SetProperty(ref _searchTruckBrand, value);
        }
        public IEnumerable<StringItem> TruckBrands 
        {
            get => _truckBrands;
            set => SetProperty(ref _truckBrands, value);
        }

        public StringItem SelectedTruckBrand
        {
            get => _selectedTruckBrand;
            set => SetProperty(ref _selectedTruckBrand, value);
        }

        public string SearchTrailerString
        {
            get => _searchTrailerBrand;
            set => SetProperty(ref _searchTrailerBrand, value);
        }
        public IEnumerable<StringItem> TrailerBrands
        {
            get => _trailerBrands;
            set => SetProperty(ref _trailerBrands, value);
        }

        public StringItem SelectedTrailerBrand
        {
            get => _selectedTrailerBrand;
            set => SetProperty(ref _selectedTrailerBrand, value);
        }

        public IEnumerable<DataViewModel<CarrierDto>> Carriers
        {
            get => _carriers;
            set => SetProperty(ref _carriers, value);
        }

        public DataViewModel<CarrierDto> SelectedCarrier
        {
            get => _selectedCarrier;
            set
            {
                SetProperty(ref _selectedCarrier, value);
                _vehicle.Carrier = SelectedCarrier as CarrierViewModel;
            }
        }

        #endregion Public

        #region Commands

        public ICommand SearchCarrierCommand { get; set; }
        public ICommand SearchTruckBrandCommand { get; set; }
        public ICommand SearchTrailerBrandCommand { get; set; }

        #endregion Commands
        public EditVehicleViewModel(ISettingsRepository<Settings> repository, 
                                    IViewModelFactory<VehicleDto> factory,
                                    IViewModelFactory<CarrierDto> carrierFactory,
                                    IDialog dialog) : base(repository, factory, dialog)
        {

            _carrierFactory = carrierFactory;

            #region CommandsInit

            SearchCarrierCommand = new RelayCommand<string>(async (searchString) =>
            {
                await SearchCarrier(searchString);
            });

            SearchTruckBrandCommand = new RelayCommand<string>(async (searchString) =>
            {
                await SearchTruckBrand(searchString);
            });

            SearchTrailerBrandCommand = new RelayCommand<string>(async (searchString) =>
            {
                await SearchTrailerBrand(searchString);
            });

            #endregion CommandsInit
        }

        

        public override async Task Load(Guid id)
        {
            await base.Load(id);
            _vehicle = EditedViewModel as VehicleViewModel;
            SelectedCarrier = _vehicle.Carrier;
        }

        public override Task Save()
        {
            if (SelectedTruckBrand == null) 
            {
                _vehicle.TruckModel = SearchTruckString;
            }
            if (SelectedTrailerBrand == null) 
            { 
                _vehicle.TrailerModel = SearchTrailerString;
            }
            return base.Save();
        }

        private async Task SearchCarrier(string searchString)
        {
            await Task.Run(async () =>
            {
                RequestResult<IEnumerable<CarrierDto>> result = await _client.Search<CarrierDto>(searchString);

                SelectedCarrier = null;

                if (result.IsSuccess)
                {
                    Carriers = result.Result.Select(c => _carrierFactory.GetViewModel(c));
                }
                else
                {
                    Carriers = null;
                }
            });
        }

        private async Task SearchTruckBrand(string? searchString)
        {
            
        }

        private async Task SearchTrailerBrand(string? searchString)
        {
            
        }
    }
}
