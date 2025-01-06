using CommunityToolkit.Mvvm.Input;
using Dadata;
using Dadata.Model;
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
    public class EditCarrierViewModel : BlockedViewModel
    {
        #region Private

        private Settings _settings;
        private ApiClient _client;
        private IDialog _dialog;

        private CarrierViewModel _editedCarrier;

        #endregion Private

        #region Public

        public CarrierMenuViewModel Parent;

        public CarrierViewModel EditedCarrier
        {
            get => _editedCarrier;
            set => SetProperty(ref _editedCarrier, value);
        }

        public bool IsWithVat 
        {
            get => _editedCarrier?.Vat == VAT.With;
            set 
            { 
                if (value && EditedCarrier != null) 
                {
                    EditedCarrier.Vat = VAT.With;
                    OnPropertyChanged(nameof(IsWithVat));   
                    OnPropertyChanged(nameof(IsWithoutVat));   
                }
            }
        }

        public bool IsWithoutVat 
        {
            get => _editedCarrier?.Vat == VAT.Without;
            set 
            {
                if (value && EditedCarrier != null)
                {
                    EditedCarrier.Vat = VAT.Without;
                    OnPropertyChanged(nameof(IsWithVat));
                    OnPropertyChanged(nameof(IsWithoutVat));
                }
            }
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

        public EditCarrierViewModel(ISettingsRepository<Settings> repository, IDialog dialog)
        {
            _settings = repository.GetSettings();
            _client = new ApiClient(_settings.ServerUri);
            _dialog = dialog;

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
                EditedCarrier = new CarrierViewModel();
            }
            else 
            {
                IsBlock = true;
                BlockText = "Загрузка";

                RequestResult<CarrierDto> result = await _client.GetId<CarrierDto>(id);

                if (result.IsSuccess)
                {
                    EditedCarrier = new CarrierViewModel(result.Result);
                }

                IsBlock = false;
            }
            OnPropertyChanged(nameof(IsWithVat));
            OnPropertyChanged(nameof(IsWithoutVat));
        }

        public async Task Save() 
        {
            IsBlock = true;
            BlockText = "Сохранение";

            RequestResult<bool> result;

            if (EditedCarrier.Id == Guid.Empty)
            {
                result = await _client.Add<CarrierDto>((CarrierDto)EditedCarrier.GetDto());
            }
            else 
            {
                result = await _client.Update<CarrierDto>((CarrierDto)EditedCarrier.GetDto());
            }

            if (result.IsSuccess)
            {
                _dialog.ShowSuccess("Сохранение");
            }
            else 
            {
                _dialog.ShowError("Не удалось сохранить изменения","Сохранение");
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
}
