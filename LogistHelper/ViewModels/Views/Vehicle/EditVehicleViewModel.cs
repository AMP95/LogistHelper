using CommunityToolkit.Mvvm.Input;
using DTOs;
using DTOs.Dtos;
using LogistHelper.Models;
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

        private IFileLoader _fileLoader;

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

        private ObservableCollection<ListItem<FileViewModel>> _files;

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

        public ObservableCollection<ListItem<FileViewModel>> Files 
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
                                    IDataSuggest<TrailerModelSuggestItem> trailerSuggest,
                                    IFileLoader loader) : base(repository, factory, dialog)
        {

            _carrierFactory = carrierFactory;
            _truckSuggest = truckSuggest;
            _trailerSuggest = trailerSuggest;
            _fileLoader = loader;

            Files = new ObservableCollection<ListItem<FileViewModel>>();

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

                TrailerBrands = trailers.Select(s => new ListItem<string>(s.TrailerModel));
            });

            DownloadFileCommand = new RelayCommand<LoadPackage>(async(package) => 
            {
                if (package.FileToLoad.Any()) 
                {
                    if (await _fileLoader.DownloadFiles(package.SavePath, package.FileToLoad.Select(f => f.Id)))
                    {
                        _dialog.ShowSuccess("Файлы сохранены");
                    }
                    else 
                    {
                        _dialog.ShowError("Ошибка сохранения файлов");
                    }
                }
            });

            RemoveFileCommand = new RelayCommand<Guid>(async (id) => 
            {
                ListItem<FileViewModel> item = Files.FirstOrDefault(i => i.Id == id);  
                Files.Remove(item);

                if (item.Item.Id != Guid.Empty) 
                { 
                    IAccessResult<bool> result = await _access.DeleteAsync<FileDto>(item.Item.Id);
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

            IAccessResult<IEnumerable<FileDto>> files = await _access.GetFilteredAsync<FileDto>(nameof(FileDto.DtoId), EditedViewModel.Id.ToString());

            Files = new ObservableCollection<ListItem<FileViewModel>>(files.Result.Select(f => new ListItem<FileViewModel>(new FileViewModel(f))));
        }

        public async override Task Save()
        {
            IsBlock = true;
            BlockText = "Сохранение";

            if (await SaveEntity())
            {
                foreach (var file in Files) 
                {
                    file.Item.DtoId = EditedViewModel.Id;
                    file.Item.DtoType = nameof(VehicleDto);
                    file.Item.ServerCatalog = $"{_vehicle.TruckModel}_{_vehicle.TruckNumber}_{_vehicle.TrailerNumber}";
                }    

                if (await _fileLoader.UploadFiles(EditedViewModel.Id, Files.Select(f => f.Item).Where(f => f.Id == Guid.Empty)))
                {
                    _dialog.ShowSuccess("Файлы заргужены");
                }
                else 
                {
                    _dialog.ShowError("Ошибка загрузки файлов на сервер");
                }

                _dialog.ShowSuccess("ТС сохранено в базу данных");

                if (EditedViewModel.Id == Guid.Empty)
                {
                    Load(Guid.Empty);
                }
            }
            else
            {
                _dialog.ShowError("Не удалось сохранить изменения", "Сохранение");
            }

            IsBlock = false;
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
            if (SelectedTruckBrand == null)
            {
                _vehicle.TruckModel = SearchTruckString;
            }
            if (SelectedTrailerBrand == null)
            {
                _vehicle.TrailerModel = SearchTrailerString;
            }

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
