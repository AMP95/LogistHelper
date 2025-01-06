using CommunityToolkit.Mvvm.Input;
using Dadata;
using DTOs;
using LogistHelper.Models.Settings;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Shared;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{
    public class EditCompanyViewModel<T> : EditViewModel<T> where T : CompanyDto
    {
        private CompanyViewModel<T> _company;

        #region Commands

        public ICommand DeleteEmailCommand { get; set; }
        public ICommand AddEmailCommand { get; set; }
        public ICommand DeletePhoneCommand { get; set; }
        public ICommand AddPhoneCommand { get; set; }

        public ICommand SearchDataCommand { get; set; }

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

            SearchDataCommand = new RelayCommand(async () =>
            {
                await Search();
            });

            #endregion CommandsInit
        }

        public override async Task Load(Guid id)
        {
            await base.Load(id);
            _company = EditedViewModel as CompanyViewModel<T>;
        }

        public async Task Search()
        {
            var api = new SuggestClientAsync(_settings.DaDataApiKey);
            var response = await api.SuggestParty($"{_company.Inn} {_company.Kpp}");
            var result = response.suggestions.FirstOrDefault();
            if (result != null)
            {
                _company.Name = result.value;
                _company.Address = result.data.address.value;
                _company.Inn = result.data.inn;
                _company.Kpp = result.data.kpp;
            }
        }
    }

    public class EditClientViewModel : EditCompanyViewModel<CompanyDto>
    {
        public EditClientViewModel(ISettingsRepository<Settings> repository, IViewModelFactory<CompanyDto> factory, IDialog dialog) : base(repository, factory, dialog)
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
