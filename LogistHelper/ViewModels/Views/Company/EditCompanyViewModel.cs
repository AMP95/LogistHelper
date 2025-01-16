using CommunityToolkit.Mvvm.Input;
using Dadata;
using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Shared;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    public class EditCompanyViewModel<T> : MainEditViewModel<T> where T : CompanyDto
    {
        private CompanyViewModel<T> _company;

        private IEnumerable<CompanyItem> _companiesList;
        private CompanyItem _selectedCompany;

        #region Public

        public IEnumerable<CompanyItem> CompaniesList 
        {
            get => _companiesList;
            set => SetProperty(ref _companiesList, value);
        }

        public CompanyItem SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                SetProperty(ref _selectedCompany, value);
                if (SelectedCompany != null)
                {
                    _company.Name = value.Name;
                    _company.Inn = value.Inn;
                    _company.Kpp = value.Kpp;
                    _company.Address = value.Address;
                }
                else 
                {
                    _company.Name = string.Empty;
                    _company.Inn = string.Empty;
                    _company.Kpp = string.Empty;
                    _company.Address = string.Empty;
                }
            }
        }


        #endregion Public

        #region Commands

        public ICommand DeleteEmailCommand { get; set; }
        public ICommand AddEmailCommand { get; set; }
        public ICommand DeletePhoneCommand { get; set; }
        public ICommand AddPhoneCommand { get; set; }

        public ICommand SearchCompanyCommand { get; set; }

        #endregion Commands

        public EditCompanyViewModel(ISettingsRepository<Settings> repository, 
                                    IViewModelFactory<T> factory, 
                                    IDialog dialog) : base(repository, factory, dialog)
        {
            #region CommandsInit

            AddPhoneCommand = new RelayCommand(() =>
            {
                _company.Phones.Add(new StringItem());
            });

            AddEmailCommand = new RelayCommand(() =>
            {
                _company.Emails.Add(new StringItem());
            });

            DeleteEmailCommand = new RelayCommand<Guid>((id) =>
            {
                StringItem item = _company.Emails.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    _company.Emails.Remove(item);
                }
            });

            DeletePhoneCommand = new RelayCommand<Guid>((id) =>
            {
                StringItem item = _company.Phones.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    _company.Phones.Remove(item);
                }
            });

            SearchCompanyCommand = new RelayCommand<string>(async (searchString) =>
            {
                await Search(searchString);
            });

            #endregion CommandsInit
        }

        public override async Task Load(Guid id)
        {
            await base.Load(id);
            _company = EditedViewModel as CompanyViewModel<T>;
        }

        public async Task Search(string searchString)
        {
            var api = new SuggestClientAsync(_settings.DaDataApiKey);
            var response = await api.SuggestParty(searchString);
            CompaniesList = response.suggestions.Select(s => new CompanyItem() { Name = s.value, Inn = s.data.inn, Kpp = s.data.kpp, Address = s.data.address.value });
        }

        public override bool CheckSave()
        {
            if (string.IsNullOrWhiteSpace(_company.Name)) 
            {
                _dialog.ShowError("Необходимо указать название организации");
                return false;
            }
            if (string.IsNullOrWhiteSpace(_company.Address))
            {
                _dialog.ShowError("Необходимо указать адрес организации");
                return false;
            }
            if (string.IsNullOrWhiteSpace(_company.Inn))
            {
                _dialog.ShowError("Необходимо указать ИНН организации");
                return false;
            }
            if (!_company.Phones.Any()) 
            {
                _dialog.ShowError("Необходимо указать номер телефона для связи");
                return false;
            }
            if (_company.Phones.Any(p=> !RegexService.CheckPhone(p.Item)))
            {
                _dialog.ShowError("Необходимо верно указать все номера телефонов");
                return false;
            }
            if (!_company.Emails.Any()) 
            {
                _dialog.ShowError("Необходимо указать электронный адрес");
                return false;
            }
            if (_company.Phones.Any(p => !RegexService.CheckEmail(p.Item)))
            {
                _dialog.ShowError("Необходимо верно указать все электронные адреса");
                return false;
            }
            return true;
        }
    }

    public class EditClientViewModel : EditCompanyViewModel<ClientDto>
    {
        public EditClientViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<ClientDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
        }
    }

    public class EditCarrierViewModel : EditCompanyViewModel<CarrierDto>
    {
        public EditCarrierViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<CarrierDto> factory, IDialog dialog) : base(repository, factory, dialog)
        {
        }
    }
}
