using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.DataViewModels;
using LogistHelper.ViewModels.Pages;
using ServerClient;
using Shared;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    public class EditCarrierViewModel : ObservableObject
    {
        #region Private

        private bool _isBlock;

        private Settings _settings;
        private ApiClient _client;
        private IDialog _dialog;

        private CarrierViewModel _editedCarrier;

        #endregion Private

        #region Public

        public CarrierMenuViewModel Parent;

        public bool IsBlock
        {
            get => _isBlock;
            set => SetProperty(ref _isBlock, value);
        }

        public CarrierViewModel EditedCarrier
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
        public ICommand SetVatCommand { get; set; }

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

            SetVatCommand = new RelayCommand<string>((vat) => 
            { 
                VAT vt = (VAT)Enum.Parse(typeof(VAT), vat);

                EditedCarrier.Vat = vt;
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

                RequestResult<CarrierDto> result = await _client.GetId<CarrierDto>(id);

                if (result.IsSuccess)
                {
                    EditedCarrier = new CarrierViewModel(result.Result);
                }

                IsBlock = false;
            }
        }

        public async Task Save() 
        {
            IsBlock = true;

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

    }
}
