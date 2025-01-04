using CommunityToolkit.Mvvm.ComponentModel;
using LogistHelper.Models.Settings;
using ServerClient;
using Shared;
using System.Collections.ObjectModel;

namespace LogistHelper.ViewModels.Views.Carrier
{
    public class CarrierListViewModel : ObservableObject
    {
        #region Private

        private Settings _settings;
        private ApiClient _client;

        private ObservableCollection<>

        #endregion Private

        #region Public

        #endregion Public

        public CarrierListViewModel(ISettingsRepository<Settings> repository)
        {
            _settings = repository.GetSettings();
            _client = new ApiClient(_settings.ServerUri);
        }
    }
}
