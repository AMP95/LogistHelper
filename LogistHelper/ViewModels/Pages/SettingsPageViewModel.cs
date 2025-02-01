using CommunityToolkit.Mvvm.Input;
using HelpAPIs.Settings;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using MailSender;
using Shared;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Pages
{
    public class SettingsPageViewModel : BasePageViewModel
    {
        #region Private

        private ISettingsRepository<ApiSettings> _apiRepository;
        private ApiSettings _apiSettings;

        private ISettingsRepository<MailSettings> _mailRpository;
        private MailSettings _mailSettings;

        private IMessageDialog _dialog;

        #endregion Private

        #region Public

        public string ServerUri 
        { 
            get => _apiSettings.ServerUri;
            set 
            { 
                _apiSettings.ServerUri = value;
                OnPropertyChanged(nameof(ServerUri));
            }
        }
        public string DaDataApiKey
        {
            get => _apiSettings.DaDataApiKey;
            set
            {
                _apiSettings.DaDataApiKey = value;
                OnPropertyChanged(nameof(DaDataApiKey));
            }
        }
        public string YandexGeoSuggestApiKey
        {
            get => _apiSettings.YandexGeoSuggestApiKey;
            set
            {
                _apiSettings.YandexGeoSuggestApiKey = value;
                OnPropertyChanged(nameof(YandexGeoSuggestApiKey));
            }
        }


        public string FromAddress
        {
            get => _mailSettings.FromAddress;
            set
            {
                _mailSettings.FromAddress = value;
                OnPropertyChanged(nameof(FromAddress));
            }
        }
        public string FromName
        {
            get => _mailSettings.FromName;
            set
            {
                _mailSettings.FromName = value;
                OnPropertyChanged(nameof(FromName));
            }
        }
        public string Password
        {
            get => _mailSettings.Password;
            set
            {
                _mailSettings.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string Server
        {
            get => _mailSettings.Server;
            set
            {
                _mailSettings.Server = value;
                OnPropertyChanged(nameof(Server));
            }
        }
        public int Port
        {
            get => _mailSettings.Port;
            set
            {
                _mailSettings.Port = value;
                OnPropertyChanged(nameof(Port));
            }
        }

        #endregion Public

        #region Commands

        public ICommand CancelApiSettingCommad { get; }
        public ICommand SaveApiSettingCommad { get; }

        public ICommand CancelMailSettingCommad { get; }
        public ICommand SaveMailSettingCommad { get; }

        #endregion Commands

        public SettingsPageViewModel(ISettingsRepository<ApiSettings> apiRepository,
                                     ISettingsRepository<MailSettings> mailRepository, 
                                     IMessageDialog messageDialog)
        {
            _apiRepository = apiRepository;
            _mailRpository = mailRepository;

            _apiSettings = _apiRepository.GetSettings();
            _mailSettings = _mailRpository.GetSettings();

            _dialog = messageDialog;

            #region CommandInit

            CancelApiSettingCommad = new RelayCommand(() => 
            {
                _apiSettings = _apiRepository.GetSettings();
                OnPropertyChanged(nameof(ServerUri));
                OnPropertyChanged(nameof(DaDataApiKey));
                OnPropertyChanged(nameof(YandexGeoSuggestApiKey));
            });

            CancelMailSettingCommad = new RelayCommand(() =>
            {
                _mailSettings = _mailRpository.GetSettings();
                OnPropertyChanged(nameof(FromAddress));
                OnPropertyChanged(nameof(FromName));
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(Server));
                OnPropertyChanged(nameof(Port));
            });

            SaveApiSettingCommad = new RelayCommand(() => 
            {
                if (_apiRepository.SaveSettings(_apiSettings))
                {
                    _dialog.ShowSuccess("Сохранение");
                }
                else 
                {
                    _dialog.ShowSuccess("Не удалось сохранить настройки");
                }
            });

            SaveMailSettingCommad = new RelayCommand(() =>
            {
                if (_mailRpository.SaveSettings(_mailSettings))
                {
                    _dialog.ShowSuccess("Сохранение");
                }
                else
                {
                    _dialog.ShowSuccess("Не удалось сохранить настройки");
                }
            });


            #endregion CommandInit
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.MainMenu);
        }
    }
}
