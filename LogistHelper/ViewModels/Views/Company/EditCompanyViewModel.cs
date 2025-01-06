using CommunityToolkit.Mvvm.Input;
using Dadata;
using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using LogistHelper.ViewModels.Pages;
using ServerClient;
using Shared;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    public class EditCompanyViewModel<T> : BlockedViewModel where T : CompanyDto
    {
        #region Private

        private Settings _settings;
        private ApiClient _client;
        private IDialog _dialog;
        private ICompanyVmFactory<T> _factory;

        private CompanyViewModel<T> _editedCarrier;

        #endregion Private

        #region Public

        public CompanyMenuViewModel<T> Parent;

        public CompanyViewModel<T> EditedCarrier
        {
            get => _editedCarrier;
            set => SetProperty(ref _editedCarrier, value);
        }

        #endregion Public

        #region Commands

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public ICommand DeleteEmailCommand { get; set; }
        public ICommand AddEmailCommand { get; set; }
        public ICommand DeletePhoneCommand { get; set; }
        public ICommand AddPhoneCommand { get; set; }

        public ICommand SearchDataCommand { get; set; }

        #endregion Commands

        public EditCompanyViewModel(ISettingsRepository<Settings> repository, ICompanyVmFactory<T> factory, IDialog dialog)
        {
            _settings = repository.GetSettings();
            _client = new ApiClient(_settings.ServerUri);
            _dialog = dialog;
            _factory = factory;

            #region CommandsInit

            SaveCommand = new RelayCommand(async () =>
            {
                if (_dialog.ShowSure("Сохранить изменения"))
                {
                    await Save();
                }
            });

            CancelCommand = new RelayCommand(async () =>
            {
                if (_dialog.ShowSure("Отменить изменения"))
                {
                    await Load(EditedCarrier.Id);
                }
            });

            BackCommand = new RelayCommand(() =>
            {
                Parent.SwitchToList();
            });

            AddPhoneCommand = new RelayCommand(() =>
            {
                EditedCarrier.Phones.Add(new StringItem());
            });

            AddEmailCommand = new RelayCommand(() =>
            {
                EditedCarrier.Emails.Add(new StringItem());
            });

            DeleteEmailCommand = new RelayCommand<Guid>((id) =>
            {
                StringItem item = EditedCarrier.Emails.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    EditedCarrier.Emails.Remove(item);
                }
            });

            DeletePhoneCommand = new RelayCommand<Guid>((id) =>
            {
                StringItem item = EditedCarrier.Phones.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    EditedCarrier.Phones.Remove(item);
                }
            });

            SearchDataCommand = new RelayCommand(async () =>
            {
                await Search();
            });

            #endregion CommandsInit
        }

        public async Task Load(Guid id)
        {
            if (id == Guid.Empty)
            {
                EditedCarrier = _factory.GetViewModel();
            }
            else
            {
                IsBlock = true;
                BlockText = "Загрузка";

                RequestResult<T> result = await _client.GetId<T>(id);

                if (result.IsSuccess)
                {
                    EditedCarrier = _factory.GetViewModel(result.Result);
                }

                IsBlock = false;
            }
        }

        public async Task Save()
        {
            IsBlock = true;
            BlockText = "Сохранение";

            RequestResult<bool> result;

            if (EditedCarrier.Id == Guid.Empty)
            {
                result = await _client.Add<T>(EditedCarrier.GetDto());
            }
            else
            {
                result = await _client.Update<T>(EditedCarrier.GetDto());
            }

            if (result.IsSuccess)
            {
                _dialog.ShowSuccess("Сохранение");
            }
            else
            {
                _dialog.ShowError("Не удалось сохранить изменения", "Сохранение");
            }

            IsBlock = false;
        }

        public async Task Search()
        {
            var api = new SuggestClientAsync(_settings.DaDataApiKey);
            var response = await api.SuggestParty($"{EditedCarrier.Inn} {EditedCarrier.Kpp}");
            var result = response.suggestions.FirstOrDefault();
            if (result != null)
            {
                EditedCarrier.Name = result.value;
                EditedCarrier.Address = result.data.address.value;
                EditedCarrier.Inn = result.data.inn;
                EditedCarrier.Kpp = result.data.kpp;
            }
        }
    }

    public class EditClientViewModel : EditCompanyViewModel<CompanyDto>
    {
        public EditClientViewModel(ISettingsRepository<Settings> repository, ICompanyVmFactory<CompanyDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
        }
    }

    public class EditCarrierViewModel : EditCompanyViewModel<CarrierDto>
    {
        public EditCarrierViewModel(ISettingsRepository<Settings> repository, ICompanyVmFactory<CarrierDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
        }
    }
}
