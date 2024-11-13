using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Pages
{
    public class CarrierMenuViewModel : BasePageViewModel
    {
        private string _carrierName;


        public string CarrierName 
        {
            get => _carrierName;
            set => SetProperty(ref _carrierName, value);
        }


        public ICommand FindCarrierCommand { get; }

        public CarrierMenuViewModel()
        {
            
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.Menu);
        }
    }
}
