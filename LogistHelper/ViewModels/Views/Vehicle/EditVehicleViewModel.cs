using CommunityToolkit.Mvvm.Input;
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

        #endregion Private

        #region Public

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

            #endregion CommandsInit
        }

        public override async Task Load(Guid id)
        {
            await base.Load(id);
            _vehicle = EditedViewModel as VehicleViewModel;
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
    }
}
