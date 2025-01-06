using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.DataViewModels;
using ServerClient;
using Shared;
using System.Collections.ObjectModel;

namespace LogistHelper.ViewModels.Base
{
    public class ListViewModel<T> : BlockedViewModel where T : class
    {
        #region Private

        IDialog _dialog;

        //private ICompanyVmFactory<T> _factory;
        private Settings _settings;
        private ApiClient _client;

        private int _startIndex = 0;
        private int _count = 20;

        private bool _isForwardAwaliable;
        private bool _isBackwardAwaliable;

        private string _searchString;
        private ObservableCollection<CompanyViewModel<T>> _list;

        #endregion Private
        public MenuPageViewModel<T> Parent { get; set; }

        public async Task Load() 
        { 
            
        }
    }
}
