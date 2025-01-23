using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Models.Suggest;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Utilities;

namespace LogistHelper.ViewModels.Views
{
    public class EditVehicleViewModel : MainEditViewModel<VehicleDto>
    {
        #region Private

        private VehicleViewModel _vehicle;

        private IDataSuggest<TruckModelSuggestItem> _truckSuggest;
        private IDataSuggest<TrailerModelSuggestItem> _trailerSuggest;

        private IEnumerable<DataViewModel<CarrierDto>> _carriers;
        private DataViewModel<CarrierDto> _selectedCarrier;

        private IViewModelFactory<CarrierDto> _carrierFactory;

        private string _searchTruckBrand;
        private IEnumerable<ListItem<string>> _truckBrands;
        private IEnumerable<ListItem<string>> _trailerBrands;

        private string _searchTrailerBrand;
        private ListItem<string> _selectedTruckBrand;
        private ListItem<string> _selectedTrailerBrand;

        private ObservableCollection<FileViewModel> _files;

        #endregion Private

        #region Public

        public string SearchTruckString 
        {
            get => _searchTruckBrand;
            set => SetProperty(ref _searchTruckBrand, value);
        }
        public IEnumerable<ListItem<string>> TruckBrands 
        {
            get => _truckBrands;
            set => SetProperty(ref _truckBrands, value);
        }

        public ListItem<string> SelectedTruckBrand
        {
            get => _selectedTruckBrand;
            set
            {
                SetProperty(ref _selectedTruckBrand, value);
                if (SelectedTruckBrand != null)
                {
                    _vehicle.TruckModel = SelectedTruckBrand.Item;
                }
                else
                {
                    _vehicle.TruckModel = null;
                }
            }
        }

        public string SearchTrailerString
        {
            get => _searchTrailerBrand;
            set => SetProperty(ref _searchTrailerBrand, value);
        }
        public IEnumerable<ListItem<string>> TrailerBrands
        {
            get => _trailerBrands;
            set => SetProperty(ref _trailerBrands, value);
        }

        public ListItem<string> SelectedTrailerBrand
        {
            get => _selectedTrailerBrand;
            set
            {
                SetProperty(ref _selectedTrailerBrand, value);
                if (SelectedTrailerBrand != null)
                {
                    _vehicle.TrailerModel = SelectedTrailerBrand.Item;
                }
                else 
                {
                    _vehicle.TrailerModel = null;
                }
            }
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

        public ObservableCollection<FileViewModel> Files 
        {
            get => _files;
            set => SetProperty(ref _files, value);
        }

        #endregion Public

        #region Commands

        public ICommand SearchCarrierCommand { get; set; }
        public ICommand SearchTruckBrandCommand { get; set; }
        public ICommand SearchTrailerBrandCommand { get; set; }
        public ICommand DownloadFileCommand { get; set; }
        public ICommand RemoveFileCommand { get; set; }

        #endregion Commands
        public EditVehicleViewModel(IDataAccess repository, 
                                    IViewModelFactory<VehicleDto> factory,
                                    IViewModelFactory<CarrierDto> carrierFactory,
                                    IDialog dialog,
                                    IDataSuggest<TruckModelSuggestItem> truckSuggest,
                                    IDataSuggest<TrailerModelSuggestItem> trailerSuggest) : base(repository, factory, dialog)
        {

            _carrierFactory = carrierFactory;
            _truckSuggest = truckSuggest;
            _trailerSuggest = trailerSuggest;

            Files = new ObservableCollection<FileViewModel>();

            #region CommandsInit

            SearchCarrierCommand = new RelayCommand<string>(async (searchString) =>
            {
                await SearchCarrier(searchString);
            });

            SearchTruckBrandCommand = new RelayCommand<string>(async (searchString) =>
            {
                var trucks = await _truckSuggest.SuggestAsync(searchString);

                TruckBrands = trucks.Select(s => new ListItem<string>(s.TruckModel));
            });

            SearchTrailerBrandCommand = new RelayCommand<string>(async (searchString) =>
            {
                var trailers = await _trailerSuggest.SuggestAsync(searchString);

                TruckBrands = trailers.Select(s => new ListItem<string>(s.TrailerModel));
            });

            DownloadFileCommand = new RelayCommand<LoadPackage>((package) => 
            {
                if (package.FileToLoad.Any()) 
                { 
                
                }
            });

            RemoveFileCommand = new RelayCommand<FileViewModel>(async (file) => 
            {
                Files.Remove(file);

                if (file.Id != Guid.Empty) 
                { 
                    RequestResult<bool> result = await _client.Delete<FileDto>(file.Id);
                }
            });

            #endregion CommandsInit
        }

        

        public override async Task Load(Guid id)
        {
            await base.Load(id);
            _vehicle = EditedViewModel as VehicleViewModel;
            SelectedCarrier = _vehicle.Carrier;
            SelectedTruckBrand = new ListItem<string>(_vehicle.TruckModel);
            SelectedTrailerBrand = new ListItem<string>(_vehicle.TrailerModel);
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
                IAccessResult<IEnumerable<CarrierDto>> result = await _access.GetFilteredAsync<CarrierDto>(nameof(CarrierDto.Name), searchString);

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

        public override bool CheckSave()
        {
            if (string.IsNullOrWhiteSpace(_vehicle.TruckModel)) 
            {
                _dialog.ShowError("Необходимо указать модель тягача");
                return false;
            }
            if (string.IsNullOrWhiteSpace(_vehicle.TruckNumber))
            {
                _dialog.ShowError("Необходимо указать номер тягача");
                return false;
            }
            if (string.IsNullOrWhiteSpace(_vehicle.TrailerModel))
            {
                _dialog.ShowError("Необходимо указать модель прицепа");
                return false;
            }
            if (string.IsNullOrWhiteSpace(_vehicle.TrailerNumber))
            {
                _dialog.ShowError("Необходимо указать номер прицепа");
                return false;
            }
            return true;
        }
    }
}
