using CommunityToolkit.Mvvm.ComponentModel;
using LogistHelper.Models.Settings;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using ServerClient;
using Shared;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Pages
{
    public class CarrierMenuViewModel : BasePageViewModel
    {
        private ObservableObject _content;


        #region Public

        public ObservableObject Content 
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        #endregion Public

        public CarrierMenuViewModel()
        {

        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.Menu);
        }
    }
}
