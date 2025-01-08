using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Shared;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    public class EditVehicleViewModel : EditViewModel<VehicleDto>
    {
        #region Private

        private List<CarrierViewModel> _carriers;
        private CarrierViewModel _selectedCarrier;

        #endregion Private

        #region Public

        public List<CarrierViewModel> Carriers 
        { 
            get => _carriers;
            set => SetProperty(ref _carriers, value);
        }

        public CarrierViewModel SelectedCarrier 
        {
            get => _selectedCarrier;
            set => SetProperty(ref _selectedCarrier, value);
        }

        #endregion Public

        #region Commands

        public ICommand SearchCarrier { get; set; }

        #endregion Commands
        public EditVehicleViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<VehicleDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
        }
    }
}
